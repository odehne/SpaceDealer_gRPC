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
	
    public class ProductsQuery : IRequest<IEnumerable<ProductDTO>> { }

    public class ProductsQueryHandler : IRequestHandler<ProductsQuery, IEnumerable<ProductDTO>>
    {
        private readonly IProductRepository _repository;

        public ProductsQueryHandler(IProductRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

		public async Task<IEnumerable<ProductDTO>> Handle(ProductsQuery request, CancellationToken cancellationToken)
		{
            {
                var itms = await _repository.GetItems();
                var dtos = new List<ProductDTO>();

                foreach (var itm in itms)
                {
                    dtos.Add(ProductDTO.MapToDto(itm));
                }

                return dtos;
            }
        }
	}
}
