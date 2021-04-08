using Cope.SpaceRogue.Galaxy.Creator.Proto;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.Creator.Services
{
	public class PlanetService : PlanetsService.PlanetsServiceBase
	{
		private readonly IMediator _mediator;
		private readonly ILogger<PlanetService> _logger;

		public PlanetService(IMediator mediator, ILogger<PlanetService> logger)
		{
			_mediator = mediator;
			_logger = logger;
		}

		public override Task<AddPlanetReply> AddPlanet(AddPlanetRequest request, ServerCallContext context)
		{
			return base.AddPlanet(request, context);
		}

		public override Task<DeletePlanetReply> DeletePlanet(DeletePlanetRequest request, ServerCallContext context)
		{
			return base.DeletePlanet(request, context);
		}

		public override Task<UpdatePlanetReply> UpdatePlanet(UpdatePlanetRequest request, ServerCallContext context)
		{
			return base.UpdatePlanet(request, context);
		}
	}
}
