using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Travelling.API.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Travelling.API.Application.Commands
{
	public class UpdateShipsPositionCommand : IRequest<bool>
	{
		[DataMember]
		public Guid ShipId { get; set; }

		[DataMember]
		public int CurrentPosX { get; set; }

		[DataMember]
		public int CurrentPosY { get; set; }

		[DataMember]
		public int CurrentPosZ { get; set; }

		public UpdateShipsPositionCommand(Guid shipId, int currentX, int currentY, int currentZ)
		{
			ShipId = shipId;
			CurrentPosX = currentX;
			CurrentPosY = currentY;
			CurrentPosZ = currentZ;
		}
	}

	public class UpdateShipsPositionCommandHandler : IRequestHandler<UpdateShipsPositionCommand, bool>
	{
		private readonly ILogger<UpdateShipsPositionCommandHandler> _logger;
		private readonly IShipRepository _repo;

		public UpdateShipsPositionCommandHandler(IShipRepository repo, ILogger<UpdateShipsPositionCommandHandler> logger)
		{
			_repo = repo;
			_logger = logger;
		}

		public async Task<bool> Handle(UpdateShipsPositionCommand request, CancellationToken cancellationToken)
		{
			var ship = await _repo.GetItem(request.ShipId);
			ship.CurrentPosX = request.CurrentPosX;
			ship.CurrentPosY = request.CurrentPosY; 
			ship.CurrentPosZ = request.CurrentPosZ;
			return await _repo.UpdatePosition(ship);
		}

	}
}
