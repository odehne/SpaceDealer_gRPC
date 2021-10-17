using Cope.SpaceRogue.Travelling.API;
using Cope.SpaceRogue.Travelling.API.Application.Commands;
using Cope.SpaceRogue.Travelling.API.Application.IntegrationEvents;
using Cope.SpaceRogue.Travelling.API.Proto;
using Grpc.Core;
using MediatR;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Traveling.API.Services
{
	public class TravellingService : TravelService.TravelServiceBase
	{
		private readonly IMediator _mediator;
		private readonly ILogger<TravellingService> _logger;
		private readonly IEventBus _eventBus;

		public TravellingService(IMediator mediator, ILogger<TravellingService> logger, IEventBus eventBus)
		{
			_mediator = mediator;
			_logger = logger;
			_eventBus = eventBus;
		}

		public override Task<GetPositionReply> GetPosition(GetPositionRequest request, ServerCallContext context)
		{
			return base.GetPosition(request, context);
		}

		public override Task<GetShipReply> GetShip(GetShipRequest request, ServerCallContext context)
		{
			return base.GetShip(request, context);
		}

		public override Task<GetShipsReply> GetShips(Empty request, ServerCallContext context)
		{
			return base.GetShips(request, context);
		}

		public async override Task<StartTravelReply> StartTravel(StartTravelRequest request, ServerCallContext context)
		{
			var command = new StartJourneyCommand (request.ShipId, request.CurrentPosX, request.CurrentPosY, 
													request.CurrentPosZ, request.TargetPosX, 
													request.TargetPosY, request.TargetPosZ);

			var reply = await _mediator.Send(command);
			return new StartTravelReply { OK = reply, Message = "Journey started." };
		}
	}
}