using Cope.SpaceRogue.Galaxy.API;
using Cope.SpaceRogue.Galaxy.API.Application.Commands;
using Cope.SpaceRogue.Galaxy.API.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace Galaxy.Creator.Application.Commands
{
	public class CatalogItemsQuery : IRequest<IEnumerable<CatalogItemDto>> { }

    public class CatalogItemsQueryHandler : IRequestHandler<CatalogItemsQuery, IEnumerable<CatalogItemDto>>
    {
        private readonly ICatalogItemsRepository _repository;

        public CatalogItemsQueryHandler(ICatalogItemsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<CatalogItemDto>> Handle(CatalogItemsQuery request, CancellationToken cancellationToken)
        {
            {
                var itms = await _repository.GetItems();
                var dtos = new List<CatalogItemDto>();

                foreach (var itm in itms)
                {
                    dtos.Add(AutoMap.Mapper.Map<CatalogItemDto>(itm));
                }

                return dtos;
            }
        }
    }
}
