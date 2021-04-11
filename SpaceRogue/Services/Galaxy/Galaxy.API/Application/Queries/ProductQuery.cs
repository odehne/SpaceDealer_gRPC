using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Cope.SpaceRogue.Galaxy.API;
using Cope.SpaceRogue.Galaxy.API.Application.Commands;
using Cope.SpaceRogue.Galaxy.API.Repositories;
using MediatR;

namespace Galaxy.Creator.Application.Commands
{


    public class ProductQuery : IRequest<ProductDTO> 
    { 
        [DataMember]
        public string ProductId { get; private set; }

		public ProductQuery(string productId)
		{
			ProductId = productId;
		}
    }

      public class ProductQueryHandler : IRequestHandler<ProductQuery, ProductDTO>
    {
        private readonly IProductRepository _repository;

        public ProductQueryHandler(IProductRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ProductDTO> Handle(ProductQuery request, CancellationToken cancellationToken)
        {
            var itm = await _repository.GetItem(request.ProductId.ToGuid());
            return ProductDTO.MapToDto(itm);
        }
    }
}
