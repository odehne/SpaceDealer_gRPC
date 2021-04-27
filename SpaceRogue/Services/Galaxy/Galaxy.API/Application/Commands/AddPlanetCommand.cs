using Cope.SpaceRogue.Galaxy.API.Repositories;
using Cope.SpaceRogue.Infrastructure.Domain;
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
		public string MarketPlaceId { get; private set; }
		public AddPlanetCommand()
		{

		}

		public AddPlanetCommand(string planetName, int posX, int posY, int posZ, string marketPlaceId)
		{
			Id = Guid.NewGuid().ToString();
			PlanetName = planetName;
			PosX = posX;
			PosY = posY;
			PosZ = posZ;
			MarketPlaceId = marketPlaceId;
		}
	}

	public class AddPlanetCommandHandler : IRequestHandler<AddPlanetCommand, PlanetDto>
	{
		private readonly IPlanetRepository _repository;
		private readonly IMarketPlaceRepository _marketRepository;

		public AddPlanetCommandHandler(IPlanetRepository repository, IMarketPlaceRepository marketPlaceRepository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_marketRepository = marketPlaceRepository ?? throw new ArgumentNullException(nameof(marketPlaceRepository));
		}

		public async Task<PlanetDto> Handle(AddPlanetCommand request, CancellationToken cancellationToken)
		{

			var market = await _marketRepository.GetItem(request.MarketPlaceId.ToGuid());

			var planet = new Planet
			{
				ID = request.Id.ToGuid(),
				Name = request.PlanetName,
				PosX = request.PosX,
				PosY = request.PosY,
				PosZ = request.PosZ,
				Market = market
			};
		
			var b = await _repository.AddItem(planet);
			var p = await _repository.GetItem(planet.ID);

			return AutoMap.Mapper.Map<PlanetDto>(p);
		}
	}
}
