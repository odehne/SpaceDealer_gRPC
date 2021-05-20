using Cope.SpaceRogue.Fighting.API.Application.IntegrationEvents.Events;
using Cope.SpaceRogue.Fighting.API.Repositories;
using MediatR;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;


namespace Cope.SpaceRogue.Fighting.API.Application.Commands
{
	public class DestroyShipCommandHandler : IRequestHandler<DestroyShipCommand, bool>
	{
		private readonly ILogger<AttackCommandHandler> _logger;
		private readonly IEventBus _eventBus;
		private readonly IShipRepository _shipRepository;

		public DestroyShipCommandHandler(ILogger<AttackCommandHandler> logger, IEventBus eventBus, IShipRepository shipRepository)
		{
			_logger = logger;
			_shipRepository = shipRepository;
			_eventBus = eventBus;
		}

		public async Task<bool> Handle(DestroyShipCommand request, CancellationToken cancellationToken)
		{
			return await _shipRepository.DestroyShip(request.ShipId.ToGuid());
		}
	}

	public class UpdateHullCommand : IRequest<bool>
	{
		[DataMember]
		public string ShipId { get; set; }
		[DataMember]
		public int NewHullValue { get; set; }
	}
	public class UpdateHullCommandHandler : IRequestHandler<UpdateHullCommand, bool>
	{
		private readonly ILogger<AttackCommandHandler> _logger;
		private readonly IEventBus _eventBus;
		private readonly IShipRepository _shipRepository;

		public UpdateHullCommandHandler(ILogger<AttackCommandHandler> logger, IEventBus eventBus, IShipRepository shipRepository)
		{
			_logger = logger;
			_shipRepository = shipRepository;
			_eventBus = eventBus;
		}

		public async Task<bool> Handle(UpdateHullCommand request, CancellationToken cancellationToken)
		{
			return await _shipRepository.UpdateHullvalue(request.ShipId.ToGuid(), request.NewHullValue);
		}
	}


}
