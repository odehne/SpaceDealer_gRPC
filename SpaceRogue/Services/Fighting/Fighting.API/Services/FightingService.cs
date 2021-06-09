using Cope.SpaceRogue.Fighting.API;
using Cope.SpaceRogue.Fighting.API.Application.Commands;
using Cope.SpaceRogue.Fighting.API.Application.IntegrationEvents;
using Cope.SpaceRogue.Fighting.API.Proto;
using Grpc.Core;
using MediatR;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Fighting.API.Services
{
	public class FightingService : FightService.FightServiceBase
	{
		private readonly IMediator _mediator;
		private readonly ILogger<FightingService> _logger;
		private readonly IEventBus _eventBus;

		public FightingService(IMediator mediator, ILogger<FightingService> logger, IEventBus eventBus)
		{
			_mediator = mediator;
			_logger = logger;
			_eventBus = eventBus;
		}


		public async override Task<FightReply> Attack(AttackRequest request, ServerCallContext context)
		{
			var attackCommand = new AttackCommand(request.FightId, request.AttackerId, request.DefenderId);
			var fight = await _mediator.Send(attackCommand);
			return new FightReply { FightId = fight.ID.ToString(), AttackerId = fight.Attacker.ShipId.ToString() };
		}

		public override Task<FightReply> Defend(DefendRequest request, ServerCallContext context)
		{
			return base.Defend(request, context);
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override Task<FleeReply> Flee(FleeRequest request, ServerCallContext context)
		{
			return base.Flee(request, context);
		}

		public override Task<FightReply> GetFight(GetFightRequest request, ServerCallContext context)
		{
			return base.GetFight(request, context);
		}

		public override Task<FightsReply> GetFights(FightingEmpty request, ServerCallContext context)
		{
			return base.GetFights(request, context);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override Task<FightReply> StartFight(StartFightRequest request, ServerCallContext context)
		{
			return base.StartFight(request, context);
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}
