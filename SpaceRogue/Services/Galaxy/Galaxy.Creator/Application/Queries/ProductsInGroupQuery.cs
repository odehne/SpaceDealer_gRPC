using Cope.SpaceRogue.Galaxy.Creator;
using Cope.SpaceRogue.Galaxy.Creator.Application.Commands;
using Cope.SpaceRogue.Galaxy.Creator.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;


namespace Galaxy.Creator.Application.Commands
{
	public class ProductsInGroupQuery : IRequest<IEnumerable<ProductDTO>>
    {
        [DataMember]
        public string GroupId { get; private set; }

        public ProductsInGroupQuery(string groupId)
        {
            GroupId = groupId;
        }
    }

    public class ProductsInGroupQueryHandler : IRequestHandler<ProductsInGroupQuery, IEnumerable<ProductDTO>>
    {
        private readonly IProductRepository _productRepository;

        public ProductsInGroupQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<IEnumerable<ProductDTO>> Handle(ProductsInGroupQuery request, CancellationToken cancellationToken)
        {
            var itms = await _productRepository.GetItems();

            var inGroup = itms.Where(x => x.GroupId.Equals(request.GroupId.ToGuid()));
            var lst = new List<ProductDTO>();

            foreach (var itm in inGroup)
            {
                lst.Add(ProductDTO.MapToDto(itm));
            }

            return lst;
        }
    }


}
