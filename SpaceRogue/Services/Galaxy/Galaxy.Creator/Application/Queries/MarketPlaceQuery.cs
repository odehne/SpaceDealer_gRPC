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
        public string MarketPlaceId { get; private set; }

		public MarketPlaceQuery(string marketPlaceId)
		{
			MarketPlaceId = marketPlaceId;
		}
	}

    public class MarketPlaceQueryHandler : IRequestHandler<MarketPlaceQuery, MarketPlaceDTO>
    {
        private readonly IMarketPlaceRepository _repository;
  
        public MarketPlaceQueryHandler(IMarketPlaceRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<MarketPlaceDTO> Handle(MarketPlaceQuery request, CancellationToken cancellationToken)
        {
            var market = await _repository.GetItem(request.MarketPlaceId.ToGuid());
            return MarketPlaceDTO.MapToDto(market);
        }
    }
}
