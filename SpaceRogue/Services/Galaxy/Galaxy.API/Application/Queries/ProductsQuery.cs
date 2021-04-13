using Cope.SpaceRogue.Galaxy.API;
using Cope.SpaceRogue.Galaxy.API.Application.Commands;
using Cope.SpaceRogue.Galaxy.API.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Galaxy.Creator.Application.Commands
{
	
    public class ProductsQuery : IRequest<IEnumerable<ProductDto>> { }

    public class ProductsQueryHandler : IRequestHandler<ProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductRepository _repository;

        public ProductsQueryHandler(IProductRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

		public async Task<IEnumerable<ProductDto>> Handle(ProductsQuery request, CancellationToken cancellationToken)
		{
            {
                var itms = await _repository.GetItems();
                var dtos = new List<ProductDto>();

                foreach (var itm in itms)
                {
                    dtos.Add(AutoMap.Mapper.Map<ProductDto>(itm));
                }

                return dtos;
            }
        }
	}
}
