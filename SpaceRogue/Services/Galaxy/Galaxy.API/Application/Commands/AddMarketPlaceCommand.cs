using Cope.SpaceRogue.Galaxy.API.Repositories;
using MediatR;
using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{

	public class AddMarketPlaceCommand : IRequest<MarketPlaceDto>
	{
		[DataMember]
		public string MarketPlaceId { get; set; }
		[DataMember]
		public string MarketPlaceName { get; set; }
		[DataMember]
		public string PlanetId { get; set; }
		[DataMember]
		public CatalogDto Offerings { get; set; }
		[DataMember]
		public CatalogDto Demands { get; set; }

		public AddMarketPlaceCommand()
		{
		}

		public AddMarketPlaceCommand(string marketPlaceId, string marketPlaceName, string planetId, CatalogDto offerings, CatalogDto demands)
		{
			MarketPlaceId = marketPlaceId;
			MarketPlaceName = marketPlaceName;
			PlanetId = planetId;
			Offerings = offerings;
			Demands = demands;
		}
	}

	public class AddMarketPlaceCommandHandler : IRequestHandler<AddMarketPlaceCommand, MarketPlaceDto>
	{
		private readonly MarketPlaceRepository _repository;
		private readonly IMediator _mediator;

		public AddMarketPlaceCommandHandler(IMediator mediator, MarketPlaceRepository repository)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<MarketPlaceDto> Handle(AddMarketPlaceCommand request, CancellationToken cancellationToken)
		{
			var market = await _repository.GetItem(request.PlanetId.ToGuid());

			return new MarketPlaceDto(request.MarketPlaceId.ToString(), market.Name);
		}

	}
}
