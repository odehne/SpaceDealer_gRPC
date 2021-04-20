using Cope.SpaceRogue.Galaxy.API.Repositories;
using Cope.SpaceRogue.Infrastructure.Domain;
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
		public string ID { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public string PlanetId { get; set; }
		[DataMember]
		public CatalogDto ProductOfferings { get; set; }
		[DataMember]
		public CatalogDto ProductDemands { get; set; }

		public AddMarketPlaceCommand()
		{
		}

		public AddMarketPlaceCommand(string marketPlaceId, string marketPlaceName, string planetId, CatalogDto offerings, CatalogDto demands)
		{
			ID = marketPlaceId;
			Name = marketPlaceName;
			PlanetId = planetId;
			ProductOfferings = offerings;
			ProductDemands = demands;
		}
	}

	public class AddMarketPlaceCommandHandler : IRequestHandler<AddMarketPlaceCommand, MarketPlaceDto>
	{
		private readonly IMarketPlaceRepository _repository;
		private readonly IMediator _mediator;

		public AddMarketPlaceCommandHandler(IMediator mediator, IMarketPlaceRepository repository)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<MarketPlaceDto> Handle(AddMarketPlaceCommand request, CancellationToken cancellationToken)
		{
			var offerings = new Catalog { ID = request.ProductOfferings.ID.ToGuid() };
			var demands= new Catalog { ID = request.ProductDemands.ID.ToGuid() };

			foreach (var item in request.ProductOfferings.CatalogItems)
			{
				offerings.CatalogItems.Add(new CatalogItem { ID = item.ID.ToGuid(), Price = (decimal)item.Price, ProductId = item.ProductId.ToGuid(), Title = item.Title });
			}

			foreach (var item in request.ProductDemands.CatalogItems)
			{
				demands.CatalogItems.Add(new CatalogItem { ID = item.ID.ToGuid(), Price = (decimal)item.Price, ProductId = item.ProductId.ToGuid(), Title = item.Title });
			}
			var market = new MarketPlace { 
											ID = request.ID.ToGuid(), 
											Name = request.Name, 
											ProductDemands = demands, 
											ProductOfferings = offerings 
										  };
			var result = await _repository.AddItem(market);
			return new MarketPlaceDto(request.ID.ToString(), market.Name);
		}

	}
}
