using Cope.SpaceRogue.Galaxy.API.Application.Commands;
using Cope.SpaceRogue.Galaxy.API.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Galaxy.Creator.Application.Commands
{

    public class ProductGroupsQuery : IRequest<IEnumerable<ProductGroupDTO>> { }

    public class ProductGroupsQueryHandler : IRequestHandler<ProductGroupsQuery, IEnumerable<ProductGroupDTO>>
    {
        private readonly IProductGroupRepository _repository;

        public ProductGroupsQueryHandler(IProductGroupRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<ProductGroupDTO>> Handle(ProductGroupsQuery request, CancellationToken cancellationToken)
        {
            {
                var itms = await _repository.GetItems();
                var dtos = new List<ProductGroupDTO>();

                foreach (var itm in itms)
                {
                    dtos.Add(ProductGroupDTO.MapToDto(itm));
                }

                return dtos;
            }
        }
    }
}
