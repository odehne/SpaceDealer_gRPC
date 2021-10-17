using Cope.SpaceRogue.Galaxy.API.Repositories;
using Cope.SpaceRogue.Infrastructure.Domain;
using Galaxy.API.Application.IntegrationEvents;
using MediatR;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using static Cope.SpaceRogue.Infrastructure.Domain.Ship;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{

	public class AddShipCommand : IRequest<bool>
	{
		[DataMember]
		public string Id { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public string PlayerId { get; set; }
		[DataMember]
		public int Shields { get; set; }
		[DataMember]
		public int Hull { get; set; }
		[DataMember]
		public ShipTypes ShipType { get; set; }
	}

	public class AddShipCommandHandler : IRequestHandler<AddShipCommand, bool>
	{
		private readonly IShipRepository _repository;
		private readonly IEventBus _eventBus;
		private readonly ILogger<AddShipCommandHandler> _logger;

		public AddShipCommandHandler(IShipRepository repository, ILogger<AddShipCommandHandler> logger, IEventBus eventBus)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_eventBus = eventBus;
			_logger = logger;
		}

		public async Task<bool> Handle(AddShipCommand request, CancellationToken cancellationToken)
		{
			var ship = new Ship()
			{
				ID = request.Id.ToGuid(),
				Name = request.Name,
				Hull = request.Hull,
				Shields = request.Shields,
				PlayerID = request.PlayerId.ToGuid(),
				ShipType = request.ShipType
			};
			var b = await _repository.AddItem(ship);

			var eventMessage = new ShipCreatedIntegrationEvent
			{
				ShipId = ship.ID.ToString(),
				Name = ship.Name,
				Shields = ship.Shields,
				Hull = ship.Hull,
				PosX = 0,
				PosY = 0,
				PosZ = 1
			};

			try
			{
				_eventBus.Publish(eventMessage);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName}", eventMessage.Id, Program.AppName);
				throw;
			}



			return b;
		}
	}
}
