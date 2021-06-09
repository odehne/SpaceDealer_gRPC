using Cope.SpaceRogue.Fighting.API.Application.IntegrationEvents.Events;
using MediatR;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Fighting.API.Application.Commands
{

	public class StartFightCommand : IRequest<bool>
	{
		[DataMember]
		public string AttackerShipId { get; set; }
		[DataMember]
		public string DefenderShipId { get; set; }

		public StartFightCommand()
		{

		}

		public StartFightCommand(string attackerShipId, string defenderShipId)
		{
			AttackerShipId = attackerShipId;
			DefenderShipId = defenderShipId;
		}
	}

	public class StartFightCommandHandler : IRequestHandler<StartFightCommand, bool>
	{
		private readonly IMediator _mediator;
		private readonly ILogger<StartFightCommandHandler> _logger;
		private readonly IEventBus _eventBus;

		public StartFightCommandHandler(IMediator mediator, ILogger<StartFightCommandHandler> logger, IEventBus eventBus)
		{
			_mediator = mediator;
			_eventBus = eventBus;
			_logger = logger;
		}

		public async Task<bool> Handle(StartFightCommand request, CancellationToken cancellationToken)
		{
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

			var fight = Engine.Galaxy.AddFight(attacker, defender);
			var attackCommand = new AttackCommand(fight.ID.ToString(), attacker.ShipId.ToString(), defender.ShipId.ToString());
			await _mediator.Send(attackCommand);
			return true;
		}
	}
}
