using Cope.SpaceRogue.Fighting.API.Models;
using Cope.SpaceRogue.Fighting.API.Repositories;
using Cope.SpaceRogue.Infrastructure;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Fighting.API.Application.IntegrationEvents.Events
{
	public class ObjectInSensorRangeIntegrationEvent : IntegrationEvent
	{
		public string ShipId { get; set; }
		public string Name { get; set; }
		public int Hull { get; set; }
		public int Shields { get; set; }
		public int PosX { get; set; }
		public int PosY { get; set; }
		public int PosZ { get; set; }
	}

	public class ObjectInSensorRangeIntegrationEventHandler : IIntegrationEventHandler<ObjectInSensorRangeIntegrationEvent>
	{
		private readonly ILogger<ObjectInSensorRangeIntegrationEventHandler> _logger;
		private readonly IShipRepository _shipRepository;

		public ObjectInSensorRangeIntegrationEventHandler(IShipRepository shipRepository, ILogger<ObjectInSensorRangeIntegrationEventHandler> logger)
		{
			_shipRepository = shipRepository;
			_logger = logger;
		}

		public async Task Handle(ObjectInSensorRangeIntegrationEvent @event)
		{
			_logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
			var ship = await _shipRepository.GetItem(@event.ShipId.ToGuid());
			var currentSector = new Position(@event.PosX, @event.PosY, @event.PosZ);

			var sm = ShipModel.MapTo(ship);
			sm.CurrentSector = currentSector;

			Engine.Galaxy.AddShip(sm);
		}
	}


}
