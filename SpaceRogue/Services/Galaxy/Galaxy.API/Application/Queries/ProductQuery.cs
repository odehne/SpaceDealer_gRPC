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


    public class ProductQuery : IRequest<ProductDto> 
    { 
        [DataMember]
        public string ProductId { get; private set; }

		public ProductQuery(string productId)
		{
			ProductId = productId;
		}
    }

      public class ProductQueryHandler : IRequestHandler<ProductQuery, ProductDto>
    {
        private readonly IProductRepository _repository;

        public ProductQueryHandler(IProductRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ProductDto> Handle(ProductQuery request, CancellationToken cancellationToken)
        {
            var itm = await _repository.GetItem(request.ProductId.ToGuid());
            return AutoMap.Mapper.Map<ProductDto>(itm);
        }
    }
}
