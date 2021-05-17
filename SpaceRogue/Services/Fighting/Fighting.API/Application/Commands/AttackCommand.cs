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
	public class AttackCommand : IRequest<string>
	{
		[DataMember]
		public string AttackerShipId { get; set; }
		[DataMember]
		public string DefenderShipId { get; set; }

		public AttackCommand()
		{

		}

		public AttackCommand(string attackerShipId, string defenderShipId)
		{
			AttackerShipId = attackerShipId;
			DefenderShipId = defenderShipId;
		}
	}

	public class AttackCommandHandler : IRequestHandler<AttackCommand, string>
	{
		private readonly IMediator _mediator;
		private readonly ILogger<AttackCommandHandler> _logger;
		private readonly IEventBus _eventBus;

		public AttackCommandHandler(IMediator mediator, ILogger<AttackCommandHandler> logger, IEventBus eventBus)
		{
			_mediator = mediator;
			_eventBus = eventBus;
			_logger = logger;
		}

		public async Task<string> Handle(AttackCommand request, CancellationToken cancellationToken)
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
			var eventMessage = new ShipAttackedIntegrationEvent(fight.ID.ToString(), attacker.ShipId.ToString(), defender.ShipId.ToString());

			try
			{
				_eventBus.Publish(eventMessage);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName}", eventMessage.Id, Program.AppName);

				throw;
			}
			return fight.ID.ToString();
		}
	}
}
