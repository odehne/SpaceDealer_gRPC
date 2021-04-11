using Cope.SpaceRogue.Galaxy.API.Proto;
using Galaxy.Creator.Application.Commands;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.API.Services
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

		public override Task<GetPlanetReply> GetPlanet(GetPlanetRequest request, ServerCallContext context)
		{
			return base.GetPlanet(request, context);
		}

		public async override Task<GetPlanetsReply> GetPlanets(GetPlanetsRequest request, ServerCallContext context)
		{
			var planets = await _mediator.Send(new PlanetsQuery());
			var rply = new GetPlanetsReply();

			foreach (var dto in planets)
			{
				rply.Planets.Add(new GetPlanetReply { Id = dto.PlanetId, Name = dto.PlanetName, PosX = dto.PosX, PosY = dto.PosY, PosZ = dto.PosZ });
			}

			return rply;
		}

		public override Task<UpdatePlanetReply> UpdatePlanet(UpdatePlanetRequest request, ServerCallContext context)
		{
			return base.UpdatePlanet(request, context);
		}
	}
}
