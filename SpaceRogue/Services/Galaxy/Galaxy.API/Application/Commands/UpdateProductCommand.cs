using Cope.SpaceRogue.Galaxy.API.Repositories;
using MediatR;
using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{

	public class UpdateProductCommand : IRequest<ProductDto>
	{
		[DataMember]
		public string Id { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public double SizeInUnits { get; set; }
		[DataMember]
		public double Rarity { get; set; }
		[DataMember]
		public double PricePerUnit { get; set; }

		public UpdateProductCommand()
		{
		}

		public UpdateProductCommand(string id, string name, double sizeInUnits, double rarity, double pricePerUnit)
		{
			Id = id;
			Name = name;
			SizeInUnits = sizeInUnits;
			Rarity = rarity;
			PricePerUnit = pricePerUnit;
		}
	}

	public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
	{
		private readonly IProductRepository _repository;
		private readonly IMediator _mediator;

		public UpdateProductCommandHandler(IMediator mediator, ProductRepository repository)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
		{
			var product = await _repository.GetItem(request.Id.ToGuid());
			if (product == null)
				throw new ArgumentException($"Product with id {request.Id} not found.");

			product.Name = request.Name;
			product.PricePerUnit = request.PricePerUnit;
			product.Rarity = request.Rarity;
			product.SizeInUnits = request.SizeInUnits;

			var result = await _repository.UpdateItem(product);

			return AutoMap.Mapper.Map<ProductDto>(result);
		}

	}
}
