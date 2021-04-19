using Cope.SpaceRogue.Galaxy.API.Application.Commands;
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

		public async override Task<AddPlanetReply> AddPlanet(AddPlanetRequest request, ServerCallContext context)
		{
			var command = new AddPlanetCommand(request.Name, request.PosX, request.PosY, request.PosX);
			var planetDto = await _mediator.Send(command);
			return AutoMap.Mapper.Map<AddPlanetReply>(planetDto);
		}

		public async override Task<PlanetOkReply> DeletePlanet(DeletePlanetRequest request, ServerCallContext context)
		{
			var result = await _mediator.Send(new DeletePlanetCommand(request.Id));
			return new PlanetOkReply { Ok = result };
		}

		public async override Task<GetPlanetReply> GetPlanet(GetPlanetRequest request, ServerCallContext context)
		{
			var planetDto = await _mediator.Send(new PlanetQuery(request.Id));
			return AutoMap.Mapper.Map<GetPlanetReply>(planetDto);
		}

		public async override Task<GetPlanetsReply> GetPlanets(PlanetsEmpty request, ServerCallContext context)
		{
			var planets = await _mediator.Send(new PlanetsQuery());
			var rply = new GetPlanetsReply();

			foreach (var dto in planets)
			{
				rply.Planets.Add(AutoMap.Mapper.Map<GetPlanetReply>(dto));
			}

			return rply;
		}

		public override Task<UpdatePlanetReply> UpdatePlanet(UpdatePlanetRequest request, ServerCallContext context)
		{
			return base.UpdatePlanet(request, context);
		}
	}
}
