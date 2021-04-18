using MediatR;
using System.Runtime.Serialization;
using Cope.SpaceRogue.Galaxy.API.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using Cope.SpaceRogue.Infrastructure.Domain;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{
	public class AddProductCommand : IRequest<ProductDto>
    {
		[DataMember]
		public string ProductId { get; private set; }

		[DataMember]
        public string ProductName { get; private set; }

        [DataMember]
        public string ProductGroupId { get; private set; }

        [DataMember]
        public double PricePerUnit { get; private set; }

		[DataMember]
		public double Capacity { get; private set; }

		[DataMember]
		public int Rarity { get; private set; }

		public AddProductCommand(string productId, string productName, string productGroupId, double pricePerUnit, double capacity, int rarity)
		{
			ProductId = productId;
			ProductName = productName;
			ProductGroupId = productGroupId;
			PricePerUnit = pricePerUnit;
			Capacity = capacity;
			Rarity = rarity;
		}
	}

	public class AddProductCommandHandler : IRequestHandler<AddProductCommand, ProductDto>
	{
		private readonly IProductRepository _repository;
		private readonly IProductGroupRepository _groupRepository;
		
		public AddProductCommandHandler(IMediator mediator, IProductRepository repository, IProductGroupRepository groupRepository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
		}

		public async Task<ProductDto> Handle(AddProductCommand request, CancellationToken cancellationToken)
		{
			var product = await _repository.GetItem(request.ProductId.ToGuid());
			var group = await _groupRepository.GetItem(request.ProductGroupId.ToGuid());

			if (group == null)
				throw new ArgumentException($"Group with id {request.ProductGroupId} not found.");

			if (product == null)
			{
				product = new Product
				{
					Group = group,
					GroupId = group.ID,
					ID = Guid.NewGuid(),
					Name = request.ProductName,
					PricePerUnit = request.PricePerUnit,
					Rarity = request.Rarity,
					SizeInUnits = request.Capacity
				};

				var b = await _repository.AddItem(product);
				if (!b)
					throw new Exception("Failed to add product.");
			}

			return AutoMap.Mapper.Map<ProductDto>(product);
		}
	}
}
