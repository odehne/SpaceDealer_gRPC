using Cope.SpaceRogue.Galaxy.API;
using Cope.SpaceRogue.Galaxy.API.Application.Commands;
using Cope.SpaceRogue.Galaxy.API.Repositories;
using MediatR;
using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Galaxy.Creator.Application.Commands
{
    public class MarketPlaceQuery : IRequest<MarketPlaceDto> 
    { 
        [DataMember]
        public string MarketPlaceId { get; private set; }

		public MarketPlaceQuery(string marketPlaceId)
		{
			MarketPlaceId = marketPlaceId;
		}
	}

    public class MarketPlaceQueryHandler : IRequestHandler<MarketPlaceQuery, MarketPlaceDto>
    {
        private readonly IMarketPlaceRepository _repository;
  
        public MarketPlaceQueryHandler(IMarketPlaceRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<MarketPlaceDto> Handle(MarketPlaceQuery request, CancellationToken cancellationToken)
        {
            var market = await _repository.GetItem(request.MarketPlaceId.ToGuid());
            return AutoMap.Mapper.Map<MarketPlaceDto>(market);
        }
    }
}
