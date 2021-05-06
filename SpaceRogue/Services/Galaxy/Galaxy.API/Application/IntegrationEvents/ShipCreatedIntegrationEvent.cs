using Cope.SpaceRogue.Galaxy.API;
using Cope.SpaceRogue.Galaxy.API.Repositories;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Galaxy.API.Application.IntegrationEvents
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
			//var planet = await _planetRepository.GetItem(@event.PlanetId.ToGuid());
			//Engine.Galaxy.AddPlanet(new PlanetModel { PlanetId = planet.ID, Name = planet.Name, Sector = new Position(planet.PosX, planet.PosY, planet.PosZ) });
		}
	}
}
