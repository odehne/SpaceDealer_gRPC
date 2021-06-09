using Cope.SpaceRogue.Shopping.API.Models;
using Cope.SpaceRogue.Shopping.API.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Shopping.Application.Queries
{
	public class ProductsQuery : IRequest<List<ProductModel>> { }

    public class ProductsQueryHandler : IRequestHandler<ProductsQuery, List<ProductModel>>
    {
        private readonly IProductRepository _repository;

        public ProductsQueryHandler(IProductRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<List<ProductModel>> Handle(ProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _repository.GetItems();
            var lst = new List<ProductModel>();

            foreach (var product in products)
            {
                var model = new ProductModel
                {
                    ID = product.ID,
                    GroupId = product.GroupId,
                    Name = product.Name,
                    PricePerUnit = product.PricePerUnit,
                    Rarity = product.Rarity,
                    SizeInUnits = product.SizeInUnits
                };
                lst.Add(model);
            }

            return lst;
        }
    }
}
