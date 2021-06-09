using Cope.SpaceRogue.Fighting.API;
using Cope.SpaceRogue.Fighting.API.Application.IntegrationEvents.Events;
using Cope.SpaceRogue.Fighting.API.Models;
using Cope.SpaceRogue.Fighting.API.Repositories;
using MediatR;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fighting.API.Domain.DomainEvents
{
    public class ShipTookHitDomainEvent : INotification
    {
        public ShipModel Ship { get; }
        public int HitPoints { get; }

        public ShipTookHitDomainEvent(ShipModel ship, int hitPoints)
        {
            Ship = ship;
            HitPoints = hitPoints;
        }

        public class ShipTookHitDomainEventHandler: INotificationHandler<ShipTookHitDomainEvent>
        {
            private readonly ILogger<ShipTookHitDomainEventHandler> _logger;
            private readonly IMediator _mediator;
            private readonly IShipRepository _shipRepo;
            private readonly IEventBus _eventBus;

            public ShipTookHitDomainEventHandler(ILogger<ShipTookHitDomainEventHandler> logger, IMediator mediator, IShipRepository shipRepo, IEventBus eventBus)
            {
                _logger = logger;
                _mediator = mediator;
                _shipRepo = shipRepo;
                _eventBus = eventBus;
            }

            public async Task Handle(ShipTookHitDomainEvent domainEvent, CancellationToken cancellationToken)
            {
                var updated = false;
                _logger.LogTrace($"Ship with Id: {domainEvent.Ship.ShipId} took a hit with {domainEvent.HitPoints} hit points.",
                        domainEvent.Ship.ShipId, domainEvent.HitPoints);
                
                if (domainEvent.Ship.ShieldsValue > 0)
                {
                    domainEvent.Ship.ShieldsValue = domainEvent.Ship.ShieldsValue - domainEvent.HitPoints;
                    domainEvent.Ship.State = domainEvent.Ship.ShieldsValue == 0 ? ShipModel.ShipStates.ShieldsDestroyed : ShipModel.ShipStates.ShieldsDamaged;
                }
                else
                {
                    if (domainEvent.Ship.HullValue > 0)
                    {
                        domainEvent.Ship.HullValue = domainEvent.Ship.HullValue - domainEvent.HitPoints;
                        domainEvent.Ship.State = domainEvent.Ship.HullValue < 0 ? ShipModel.ShipStates.Destroyed : ShipModel.ShipStates.HullDamaged;
                    }
                    else
                    {
                        domainEvent.Ship.State = ShipModel.ShipStates.Destroyed;
                    }
                }
                IntegrationEvent @event;

                switch (domainEvent.Ship.State)
                {
                    case ShipModel.ShipStates.ShieldsDamaged:
                        updated = await _shipRepo.UpdateShieldvalue(domainEvent.Ship.ShipId, domainEvent.Ship.ShieldsValue);
                        @event = new ShipAttackedIntegrationEvent(domainEvent.Ship.ShipId.ToString());
                        break;
                    case ShipModel.ShipStates.ShieldsDestroyed:
                        updated = await _shipRepo.UpdateShieldvalue(domainEvent.Ship.ShipId, domainEvent.Ship.ShieldsValue);
                        @event = new ShieldsDownIntegrationEvent(domainEvent.Ship.ShipId.ToString());
                        break;
                    case ShipModel.ShipStates.HullDamaged:
                        updated = await _shipRepo.UpdateHullvalue(domainEvent.Ship.ShipId, domainEvent.Ship.ShieldsValue);
                        @event = new ShipAttackedIntegrationEvent(domainEvent.Ship.ShipId.ToString());
                        break;
                    case ShipModel.ShipStates.Destroyed:
                        updated = await _shipRepo.DestroyShip(domainEvent.Ship.ShipId);
                        @event = new ShipDestroyedIntegrationEvent(domainEvent.Ship.ShipId.ToString());
                        break;
                    default:
                        @event = new ShipAttackedIntegrationEvent(domainEvent.Ship.ShipId.ToString());
                        break;
                };

                //var eventMessage = new ShipAttackedIntegrationEvent(fight.ID.ToString(), attacker.ShipId.ToString(), defender.ShipId.ToString());
                try
                {
                    _eventBus.Publish(@event);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName}", @event.Id, Program.AppName);
                    throw;
                }
            }
        }
    }
}