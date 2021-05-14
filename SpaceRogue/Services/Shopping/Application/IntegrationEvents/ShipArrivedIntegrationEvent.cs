using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Shopping.API.Models;
using Cope.SpaceRogue.Shopping.API.Repositories;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Shopping.API.Application.IntegrationEvents
{
	public class ShipArrivedIntegrationEvent : IntegrationEvent
	{
		public string ShipId { get; set; }
		public string Name { get; set; }
		public int Hull { get; set; }
		public int Shields { get; set; }
		public int PosX { get; set; }
		public int PosY { get; set; }
		public int PosZ { get; set; }
	}
	public class ShipArrivedIntegrationEventHandler : IIntegrationEventHandler<ShipArrivedIntegrationEvent>
	{
		private readonly ILogger<ShipArrivedIntegrationEventHandler> _logger;
		private readonly IShipRepository _shipRepository;

		public ShipArrivedIntegrationEventHandler(IShipRepository shipRepository, ILogger<ShipArrivedIntegrationEventHandler> logger)
		{
			_shipRepository = shipRepository;
			_logger = logger;
		}

		public async Task Handle(ShipArrivedIntegrationEvent @event)
		{
			_logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
			var ship = await _shipRepository.GetItem(@event.ShipId.ToGuid());
			var currentSector = new Position(@event.PosX, @event.PosY, @event.PosZ);
			Engine.Galaxy.AddShip(new ShipModel { ID = ship.ID, Name = ship.Name, CurrentSector = currentSector });
		}
	}
}
