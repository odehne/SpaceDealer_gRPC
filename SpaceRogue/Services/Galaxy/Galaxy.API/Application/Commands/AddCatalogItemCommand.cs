using MediatR;
using System;
using System.Runtime.Serialization;
using Cope.SpaceRogue.Galaxy.API.Repositories;
using System.Threading;
using System.Threading.Tasks;
using Cope.SpaceRogue.Infrastructure.Domain;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{
	public class AddCatalogItemCommand : IRequest<CatalogItemDto>
	{
		[DataMember]
		public string CatalogId { get; private set; }
		[DataMember]
		public string ProductId { get; private set; }
		[DataMember]
		public string Title { get; private set; }
		[DataMember]
		public double Price { get; private set; }

		public AddCatalogItemCommand(string catalogId, string productId, string title, double price)
		{
			CatalogId = catalogId;
			ProductId = productId;
			Title = title;
			Price = price;
		}
	}

	public class AddCatalogItemCommandHandler : IRequestHandler<AddCatalogItemCommand, CatalogItemDto>
	{
		private readonly IProductRepository _productsRepo;
		private readonly ICatalogItemsRepository _repository;
		private readonly IMediator _mediator;

		public AddCatalogItemCommandHandler(IMediator mediator, ICatalogItemsRepository repository, IProductRepository productRepo)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_productsRepo = productRepo ?? throw new ArgumentNullException(nameof(productRepo));
		}

		public async Task<CatalogItemDto> Handle(AddCatalogItemCommand request, CancellationToken cancellationToken)
		{
			var product = await _productsRepo.GetItem(request.ProductId.ToGuid());
			var item = new CatalogItem(product, request.Title, (decimal)request.Price, request.CatalogId.ToGuid());
			var result = await _repository.AddItem(item);
			return AutoMap.Mapper.Map<CatalogItemDto>(item);
		}
	}
}
