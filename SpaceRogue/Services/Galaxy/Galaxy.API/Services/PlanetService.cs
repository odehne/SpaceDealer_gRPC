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
			var command = new AddPlanetCommand(request.Name, request.PosX, request.PosY, request.PosZ, request.MarketPlaceId);
			var planetDto = await _mediator.Send(command);
			return new AddPlanetReply { Id = planetDto.ID, Message = "OK" };
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

			foreach (var planetDto in planets)
			{
				rply.Planets.Add(new GetPlanetReply { Id = planetDto.ID, Name = planetDto.Name, PosX = planetDto.PosX, PosY = planetDto.PosY, PosZ = planetDto.PosZ });
			}

			return rply;
		}

		public override Task<UpdatePlanetReply> UpdatePlanet(UpdatePlanetRequest request, ServerCallContext context)
		{
			return base.UpdatePlanet(request, context);
		}
	}
}
