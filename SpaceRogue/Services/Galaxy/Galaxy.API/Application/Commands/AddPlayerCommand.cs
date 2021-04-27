using Cope.SpaceRogue.Galaxy.API.Repositories;
using Cope.SpaceRogue.Infrastructure.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{
	public class AddPlayerCommand : IRequest<bool>
	{
		[DataMember]
		public string Id { get; set; }
		[DataMember]
		public string Name { get;  set; }
		[DataMember]
		public string PlanetId{ get;  set; }
		[DataMember]
		public string[] ShipIds { get;  set; }
		[DataMember]
		public int PlayerType{ get; set; }
		[DataMember]
		public double Credits{ get; set; }

	}

	public class AddPlayerCommandHandler : IRequestHandler<AddPlayerCommand, bool>
	{
		private readonly IPlayerRepository _repository;
		private readonly IPlanetRepository _planetRepository;
		private readonly IShipRepository _shipRepository;

		public AddPlayerCommandHandler(IPlayerRepository repository, IPlanetRepository planetRepository, IShipRepository shipRepository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_planetRepository = planetRepository ?? throw new ArgumentNullException(nameof(planetRepository));
			_shipRepository = shipRepository ?? throw new ArgumentNullException(nameof(shipRepository));
		}

		public async Task<bool> Handle(AddPlayerCommand request, CancellationToken cancellationToken)
		{
			var planet = await _planetRepository.GetItem(request.PlanetId.ToGuid());
			var player = new Player()
			{
				ID = request.Id.ToGuid(),
				HomePlanet = planet,
				PlayerType = (Player.PlayerTypes)request.PlayerType,
				Credits = (decimal)request.Credits,
				Name = request.Name,
				Fleet = new List<Ship>()
			};

			foreach (var shipId in request.ShipIds)
			{
				var ship = await _shipRepository.GetItem(shipId.ToGuid());
				player.Fleet.Add(ship);
			}
			var b = await _repository.AddItem(player);
			return b;
		}
	}

}
