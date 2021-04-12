using Cope.SpaceRogue.Galaxy.API.Domain;
using Cope.SpaceRogue.Galaxy.API.Repositories;
using Cope.SpaceRogue.InfraStructure;
using MediatR;
using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{
	public class AddProductGroupCommand : IRequest<ProductGroupDTO>
	{
		[DataMember]
		public string ProductGroupId { get; private set; }

		[DataMember]
		public string ProductGroupName { get; private set; }

		public AddProductGroupCommand(string productGroupId, string productGroupName)
		{
			ProductGroupId = productGroupId;
			ProductGroupName = productGroupName;
		}
	}

	public class AddProductGroupCommandHandler : IRequestHandler<AddProductGroupCommand, ProductGroupDTO>
    {
        private readonly IProductGroupRepository _repository;

        public AddProductGroupCommandHandler(IProductGroupRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ProductGroupDTO> Handle(AddProductGroupCommand request, CancellationToken cancellationToken)
        {
            ProductGroup grp = ProductGroupDTO.MapFromDto(request);

            grp = await _repository.UpdateItem(grp);

			return ProductGroupDTO.MapToDto(grp);
        }
    }

	public class ProductGroupDTO
	{
		public string ProductGroupId { get; private set; }
		public string ProductGroupName { get; private set; }

		public ProductGroupDTO(string productGroupId, string productGroupName)
		{
			ProductGroupId = productGroupId;
			ProductGroupName = productGroupName;
		}

		internal static ProductGroupDTO MapToDto(ProductGroup itm)
		{
			return new ProductGroupDTO(itm.ID.ToString(), itm.Name);
		}

        internal static ProductGroup MapFromDto(AddProductGroupCommand request)
        {
            return new ProductGroup(request.ProductGroupName);
        }
    }

}
