using Cope.SpaceRogue.Fighting.API.Application.IntegrationEvents.Events;
using Cope.SpaceRogue.Fighting.API.Models;
using Cope.SpaceRogue.Fighting.API.Repositories;
using Cope.SpaceRogue.Infrastructure.Domain;
using MediatR;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Fighting.API.Application.Commands
{

	public class AttackCommand : IRequest<FightModel>
	{
		[DataMember]
		public string FightId { get; set; }
		[DataMember]
		public string AttackerShipId { get; set; }
		[DataMember]
		public string DefenderShipId { get; set; }

		public AttackCommand()
		{

		}

		public AttackCommand(string fightId, string attackerShipId, string defenderShipId)
		{
			FightId = FightId;
			AttackerShipId = attackerShipId;
			DefenderShipId = defenderShipId;
		}
	}

	public class AttackCommandHandler : IRequestHandler<AttackCommand, FightModel>
	{
		private readonly IMediator _mediator;
		private readonly IFightRepository _fightRepository;
		private readonly ILogger<AttackCommandHandler> _logger;
		private readonly IEventBus _eventBus;

		public AttackCommandHandler(IMediator mediator, ILogger<AttackCommandHandler> logger, IEventBus eventBus, IFightRepository fightRepository)
		{
			_mediator = mediator;
			_fightRepository = fightRepository;
			_eventBus = eventBus;
			_logger = logger;
		}

		public async Task<FightModel> Handle(AttackCommand request, CancellationToken cancellationToken)
		{

			var fight = Engine.Galaxy.GetFight(request.FightId.ToGuid());
			var attacker = Engine.Galaxy.GetShip(request.AttackerShipId.ToGuid());
			var defender = Engine.Galaxy.GetShip(request.DefenderShipId.ToGuid());

			if (attacker == null)
			{
				_logger.LogError($"Attacking ship [{request.AttackerShipId}] not found.");
				throw new ArgumentException($"Attacking ship [{request.AttackerShipId}] not found.");
			}

			if (defender == null)
			{
				_logger.LogError($"Defending ship [{request.DefenderShipId}] not found.");
				throw new ArgumentException($"Defending ship [{request.DefenderShipId}] not found.");
			}

			if (fight == null)
			{
				_logger.LogInformation($"Registering new fight [{request.FightId}].");
				fight = Engine.Galaxy.AddFight(attacker, defender);
			}

			if (fight != null)
			{
				IntegrationEvent @event;

				var state = await _fightRepository.Battle(request.FightId.ToGuid(), attacker, defender);
				
			
			}

			_logger.LogError("Failed to register fight.", fight.ID, Program.AppName);
			return null;
		}
	}
}
