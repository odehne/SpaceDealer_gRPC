using Cope.SpaceRogue.Galaxy.API.Repositories;
using Cope.SpaceRogue.Infrastructure.Model;
using MediatR;
using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{
	public class AddPlanetCommand : IRequest<PlanetDto>
	{
		[DataMember]
		public string Id { get; private set; }
		[DataMember]
		public string PlanetName { get; private set; }
		[DataMember]
		public int PosX { get; private set; }
		[DataMember]
		public int PosY { get; private set; }
		[DataMember]
		public int PosZ { get; private set; }
		[DataMember]
		public MarketPlaceDto MarketPlace { get; private set; }

		public AddPlanetCommand(string planetName, int posX, int posY, int posZ)
		{
			Id = Guid.NewGuid().ToString();
			PlanetName = planetName;
			PosX = posX;
			PosY = posY;
			PosZ = posZ;
			MarketPlace = new MarketPlaceDto($"Markt auf {PlanetName}");
		}
	}

	public class AddPlanetCommandHandler : IRequestHandler<AddPlanetCommand, PlanetDto>
	{
		private readonly IPlanetRepository _repository;

		public AddPlanetCommandHandler(IPlanetRepository repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<PlanetDto> Handle(AddPlanetCommand request, CancellationToken cancellationToken)
		{
			var planet = AutoMap.Mapper.Map<Planet>(request);

			var b = await _repository.AddItem(planet);
			var p = await _repository.GetItem(planet.ID);

			return AutoMap.Mapper.Map<PlanetDto>(p);
		}
	}
}
