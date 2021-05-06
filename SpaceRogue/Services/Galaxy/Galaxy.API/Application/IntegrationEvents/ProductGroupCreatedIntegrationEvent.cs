using Cope.SpaceRogue.Galaxy.API;
using Cope.SpaceRogue.Galaxy.API.Repositories;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Galaxy.API.Application.IntegrationEvents
{
	public class ProductGroupCreatedIntegrationEvent : IntegrationEvent
	{
		public string ProductGroupId { get; set; }
		public string Name { get; set; }
	}

	public class ProductGroupCreatedIntegrationEventHandler : IIntegrationEventHandler<ProductGroupCreatedIntegrationEvent>
	{
		private readonly ILogger<ProductGroupCreatedIntegrationEventHandler> _logger;
		private readonly IProductGroupRepository _productGroupRepository;

		public ProductGroupCreatedIntegrationEventHandler(IProductGroupRepository productGroupRepository, ILogger<ProductGroupCreatedIntegrationEventHandler> logger)
		{
			_productGroupRepository = productGroupRepository;
			_logger = logger;
		}

		public async Task Handle(ProductGroupCreatedIntegrationEvent @event)
		{
			_logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
			//var planet = await _planetRepository.GetItem(@event.PlanetId.ToGuid());
			//Engine.Galaxy.AddPlanet(new PlanetModel { PlanetId = planet.ID, Name = planet.Name, Sector = new Position(planet.PosX, planet.PosY, planet.PosZ) });
		}
	}
}
