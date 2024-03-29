﻿using Cope.SpaceRogue.Galaxy.API;
using Cope.SpaceRogue.Galaxy.API.Application.Commands;
using Cope.SpaceRogue.Galaxy.API.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;


namespace Galaxy.Creator.Application.Commands
{
	public class ProductsInGroupQuery : IRequest<IEnumerable<ProductDto>>
    {
        [DataMember]
        public string GroupId { get; private set; }

        public ProductsInGroupQuery(string groupId)
        {
            GroupId = groupId;
        }
    }

    public class ProductsInGroupQueryHandler : IRequestHandler<ProductsInGroupQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductRepository _productRepository;

        public ProductsInGroupQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<IEnumerable<ProductDto>> Handle(ProductsInGroupQuery request, CancellationToken cancellationToken)
        {
            var itms = await _productRepository.GetItems();

            var inGroup = itms.Where(x => x.GroupId.Equals(request.GroupId.ToGuid()));
            var lst = new List<ProductDto>();

            foreach (var itm in inGroup)
            {
                lst.Add(AutoMap.Mapper.Map<ProductDto>(itm));
            }

            return lst;
        }
    }


}
