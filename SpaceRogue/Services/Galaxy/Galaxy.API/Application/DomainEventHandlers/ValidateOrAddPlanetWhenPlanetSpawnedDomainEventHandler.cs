using Cope.SpaceRogue.Galaxy.Application.IntegrationEvents;
using Cope.SpaceRogue.Galaxy.API.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Cope.SpaceRogue.Infrastructure.Model;

namespace Cope.SpaceRogue.Galaxy.Application.DomainEventHandlers
{
	public class ValidateOrAddPlanetWhenPlanetSpawnedDomainEventHandler
		: INotificationHandler<PlanetSpawnedDomainEvent>
	{
		private readonly ILoggerFactory _logger;
		private readonly IRepository<Planet> _planetRepository;
		private readonly ICreationIntegrationEventService _creationIntegrationEventService;
		public ValidateOrAddPlanetWhenPlanetSpawnedDomainEventHandler(ILoggerFactory logger, IRepository<Planet> planetRepository)
		{
			_logger = logger;
			_planetRepository = planetRepository;
		}

		public async Task Handle(PlanetSpawnedDomainEvent planetSpawnedEvent, CancellationToken cancellationToken)
		{
			var gID = Guid.Parse(planetSpawnedEvent.ID);
			var p = await _planetRepository.GetItem(gID);
			if (p == null)
			{
				throw new ArgumentException("Planet not found.");
			}

			//p.ChangeState(Planet.PlanetStates.Spawned);

			if (p.Market != null)
			{
				//p.ChangeState(Planet.PlanetStates.MarketOpen);
				await _planetRepository.UpdateItem(p);

				var newMarketOpenEvent = new PlanetSpawnedIntegrationEvent(planetSpawnedEvent.ID, planetSpawnedEvent.Name, planetSpawnedEvent.PosX, planetSpawnedEvent.PosY, planetSpawnedEvent.PosZ);
				await _creationIntegrationEventService.AddAndSaveEventAsync(newMarketOpenEvent);
				_logger.CreateLogger<ValidateOrAddPlanetWhenPlanetSpawnedDomainEventHandler>()
					.LogTrace($"New market {p.Market.Name} on planet {planetSpawnedEvent.Name} just opened.", planetSpawnedEvent.ID, p.Market.ID);
			}

		
		}
	}
}
