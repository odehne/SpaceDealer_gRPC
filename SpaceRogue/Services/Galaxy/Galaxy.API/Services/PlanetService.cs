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

		public override Task<GetPlayerReply> AddPlayer(AddPlayerRequest request, ServerCallContext context)
		{
			return base.AddPlayer(request, context);
		}

		public override Task<GetShipReply> AddShip(AddShipRequest request, ServerCallContext context)
		{
			return base.AddShip(request, context);
		}

		public async override Task<PlanetOkReply> DeletePlanet(DeletePlanetRequest request, ServerCallContext context)
		{
			var result = await _mediator.Send(new DeletePlanetCommand(request.Id));
			return new PlanetOkReply { Ok = result };
		}

		public override Task<PlayerOkReply> DeletePlayer(DeletePlayerRequest request, ServerCallContext context)
		{
			return base.DeletePlayer(request, context);
		}

		public override Task<ShipOkReply> DeleteShip(DeleteShipRequest request, ServerCallContext context)
		{
			return base.DeleteShip(request, context);
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

		public override Task<GetPlayerReply> GetPlayer(GetPlayerRequest request, ServerCallContext context)
		{
			return base.GetPlayer(request, context);
		}

		public async override Task<GetPlayersReply> GetPlayers(PlanetsEmpty request, ServerCallContext context)
		{
			var players = await _mediator.Send(new PlayersQuery());
			var rply = new GetPlayersReply();

			foreach (var player in players)
			{
				rply.Players.Add(AutoMap.Mapper.Map<GetPlayerReply>(player));
			}

			return rply;
		}

		public override Task<GetShipReply> GetShip(GetShipRequest request, ServerCallContext context)
		{
			return base.GetShip(request, context);
		}

		public async override Task<GetShipsReply> GetShips(PlanetsEmpty request, ServerCallContext context)
		{
			var ships = await _mediator.Send(new ShipsQuery());
			var rply = new GetShipsReply();

			foreach (var ship in ships)
			{
				rply.Ships.Add(AutoMap.Mapper.Map<GetShipReply>(ship));
			}

			return rply;
		}

		public override Task<GetShipsReply> GetShipsByPlayer(GetPlayerRequest request, ServerCallContext context)
		{
			return base.GetShipsByPlayer(request, context);
		}

		public override Task<UpdatePlanetReply> UpdatePlanet(UpdatePlanetRequest request, ServerCallContext context)
		{
			return base.UpdatePlanet(request, context);
		}

		public override Task<GetPlayerReply> UpdatePlayer(UpdatePlayerRequest request, ServerCallContext context)
		{
			return base.UpdatePlayer(request, context);
		}

		public override Task<GetShipReply> UpdateShip(AddShipRequest request, ServerCallContext context)
		{
			return base.UpdateShip(request, context);
		}

		public async override Task<GetFeaturesReply> GetFeatures(PlanetsEmpty request, ServerCallContext context)
		{
			var features = await _mediator.Send(new FeaturesQuery());
			var rply = new GetFeaturesReply();

			foreach (var feat in features)
			{
				rply.Features.Add(AutoMap.Mapper.Map<GetFeatureReply>(feat));
			}

			return rply;
		}
	}
}
