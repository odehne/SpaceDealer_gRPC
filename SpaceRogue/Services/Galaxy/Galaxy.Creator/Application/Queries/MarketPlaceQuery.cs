using Cope.SpaceRogue.Galaxy.Creator;
using Cope.SpaceRogue.Galaxy.Creator.Application.Commands;
using Cope.SpaceRogue.Galaxy.Creator.Repositories;
using MediatR;
using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Galaxy.Creator.Application.Commands
{
    public class MarketPlaceQuery : IRequest<MarketPlaceDTO> 
    { 
        [DataMember]
        public string PlanetId { get; private set; }

		public MarketPlaceQuery(string planetId)
		{
			PlanetId = planetId;
		}
	}

    public class MarketPlaceQueryHandler : IRequestHandler<MarketPlaceQuery, MarketPlaceDTO>
    {
        private readonly IPlanetRepository _repository;
  
        public MarketPlaceQueryHandler(IPlanetRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<MarketPlaceDTO> Handle(MarketPlaceQuery request, CancellationToken cancellationToken)
        {
            var planet = await _repository.GetItem(request.PlanetId.ToGuid());
            return MarketPlaceDTO.MapToDto(planet.Market);
        }
    }
}
