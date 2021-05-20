using Cope.SpaceRogue.Fighting.API.Models;
using Cope.SpaceRogue.Fighting.API.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
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

            public ShipTookHitDomainEventHandler(ILogger<ShipTookHitDomainEventHandler> logger, IMediator mediator, IShipRepository shipRepo)
            {
                _logger = logger;
                _mediator = mediator;
                _shipRepo = shipRepo;
            }

            public async Task Handle(ShipTookHitDomainEvent domainEvent, CancellationToken cancellationToken)
            {
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
                        _mediator.Send(new ShipDestroyedCommand)
                    }
                }
            }
        }
    }
}