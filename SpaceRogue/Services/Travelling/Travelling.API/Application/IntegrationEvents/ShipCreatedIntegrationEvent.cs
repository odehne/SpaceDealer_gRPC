using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Travelling.API.Models;
using Cope.SpaceRogue.Travelling.API.Repositories;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Travelling.API.Application.IntegrationEvents
{
	public class ShipCreatedIntegrationEvent : IntegrationEvent
	{
		public string ShipId { get; set; }
		public string Name { get; set; }
		public int Hull { get; set; }
		public int Shields { get; set; }
		public int PosX { get; set; }
		public int PosY { get; set; }
		public int PosZ { get; set; }
	}
	public class ShipCreatedIntegrationEventHandler : IIntegrationEventHandler<ShipCreatedIntegrationEvent>
	{
		private readonly ILogger<ShipCreatedIntegrationEventHandler> _logger;
		private readonly IShipRepository _shipRepository;

		public ShipCreatedIntegrationEventHandler(IShipRepository shipRepository, ILogger<ShipCreatedIntegrationEventHandler> logger)
		{
			_shipRepository = shipRepository;
			_logger = logger;
		}

		public async Task Handle(ShipCreatedIntegrationEvent @event)
		{
			_logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
			var ship = await _shipRepository.GetItem(@event.ShipId.ToGuid());
			var currentSector = new Position(@event.PosX, @event.PosY, @event.PosZ);
			Engine.Galaxy.AddShip(new ShipModel { ShipId = ship.ID, Name = ship.Name, CurrentSector = currentSector, SensorRange = 1, Speed = 1 });
		}
	}
}
