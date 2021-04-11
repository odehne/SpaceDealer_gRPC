using Cope.SpaceRogue.Galaxy.API.Model;
using Cope.SpaceRogue.Galaxy.Application.DomainEventHandlers;
using Cope.SpaceRogue.Galaxy.Application.IntegrationEvents;
using Cope.SpaceRogue.Galaxy.API.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{
	[DataContract]
	public class SpawnPlanetCommand : IRequest<bool>
	{
		[DataMember]
		public string PlanetId { get; private set; }
		[DataMember]
		public string Name { get; private set; }
		[DataMember]
		public int PosX { get; private set; }
		[DataMember]
		public int PosY { get; private set; }
		[DataMember]
		public int PosZ { get; private set; }

		public SpawnPlanetCommand(string planetId, string name, int posX, int posY, int posZ)
		{
			PlanetId = planetId;
			Name = name;
			PosX = posX;
			PosY = posY;
			PosZ = posZ;
		}
	}

    public class SpawnPlanetCommandHandler : IRequestHandler<SpawnPlanetCommand, bool>
    {
        private readonly IRepository<Planet> _planetRepository;
        private readonly IMediator _mediator;
        private readonly ICreationIntegrationEventService _creationIntegrationEventService;
        private readonly ILogger<SpawnPlanetCommandHandler> _logger;

        // Using DI to inject infrastructure persistence Repositories
        public SpawnPlanetCommandHandler(IMediator mediator,
            ICreationIntegrationEventService creationIntegrationEventService,
            IRepository<Planet> planetRepository,
            ILogger<SpawnPlanetCommandHandler> logger)
        {
            _planetRepository = planetRepository ?? throw new ArgumentNullException(nameof(planetRepository));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _creationIntegrationEventService = creationIntegrationEventService ?? throw new ArgumentNullException(nameof(creationIntegrationEventService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(SpawnPlanetCommand message, CancellationToken cancellationToken)
        {
            // Add Integration event to clean the basket
            var planetSpawnedIntegrationEvent = new PlanetSpawnedIntegrationEvent(message.PlanetId, message.Name, message.PosX, message.PosY, message.PosZ );
            await _creationIntegrationEventService.AddAndSaveEventAsync(planetSpawnedIntegrationEvent);

            return true;
        }
    }
}
