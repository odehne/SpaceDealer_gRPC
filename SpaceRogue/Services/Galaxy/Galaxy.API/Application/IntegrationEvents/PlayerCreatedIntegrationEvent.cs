using Cope.SpaceRogue.Galaxy.API.Repositories;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;


namespace Cope.SpaceRogue.Galaxy.API.Application.IntegrationEvents
{
	public class PlayerCreatedIntegrationEvent : IntegrationEvent
	{
		public string PlayerId { get; set; }
		public string Name { get; set; }
		public string HomePlanetId { get; set; }
	}

	public class PlayerCreatedIntegrationEventHandler : IIntegrationEventHandler<PlayerCreatedIntegrationEvent>
	{
		private readonly ILogger<PlayerCreatedIntegrationEventHandler> _logger;
		private readonly IPlanetRepository _planetRepository;

		public PlayerCreatedIntegrationEventHandler(IPlanetRepository planetRepository, ILogger<PlayerCreatedIntegrationEventHandler> logger)
		{
			_planetRepository = planetRepository;
			_logger = logger;
		}

		public async Task Handle(PlayerCreatedIntegrationEvent @event)
		{
			_logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
			//var planet = await _planetRepository.GetItem(@event.PlanetId.ToGuid());
			//Engine.Galaxy.AddPlanet(new PlanetModel { PlanetId = planet.ID, Name = planet.Name, Sector = new Position(planet.PosX, planet.PosY, planet.PosZ) });
		}
	}
}
