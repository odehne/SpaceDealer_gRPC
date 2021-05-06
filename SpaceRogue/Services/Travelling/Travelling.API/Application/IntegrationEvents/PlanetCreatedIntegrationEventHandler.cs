﻿using Cope.SpaceRogue.InfraStructure;
using Cope.SpaceRogue.Travelling.API.Models;
using Cope.SpaceRogue.Travelling.API.Repositories;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Travelling.API.Application.IntegrationEvents
{
	public class PlanetCreatedIntegrationEvent : IntegrationEvent
	{
		public string PlanetId { get; set; }
		public string Name { get; set; }
		public int PosX { get; set; }
		public int PosY { get; set; }
		public int PosZ { get; set; }
	}

	public class PlanetAddedIntegrationEventHandler : IIntegrationEventHandler<PlanetCreatedIntegrationEvent>
	{
		private readonly ILogger<PlanetAddedIntegrationEventHandler> _logger;
		private readonly IPlanetRepository _planetRepository;

		public PlanetAddedIntegrationEventHandler(IPlanetRepository planetRepository, ILogger<PlanetAddedIntegrationEventHandler> logger)
		{
			_planetRepository = planetRepository;
			_logger = logger;
		}

		public async Task Handle(PlanetCreatedIntegrationEvent @event)
		{
			_logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
			var planet = await _planetRepository.GetItem(@event.PlanetId.ToGuid());
			Engine.Galaxy.AddPlanet(new PlanetModel { PlanetId = planet.ID, Name = planet.Name, Sector = new Position(planet.PosX, planet.PosY, planet.PosZ) });
		}
	}

}
