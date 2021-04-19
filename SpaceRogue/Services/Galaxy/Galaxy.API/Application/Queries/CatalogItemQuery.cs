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
	public class CatalogItemQuery : IRequest<CatalogItemDto>
    {
        [DataMember]
        public string Id { get; private set; }

        public CatalogItemQuery(string catalogItemId)
        {
            Id = catalogItemId;
        }
    }

    public class CatalogItemQueryHandler : IRequestHandler<CatalogItemQuery, CatalogItemDto>
    {
        private readonly ICatalogItemsRepository _repository;

        public CatalogItemQueryHandler(ICatalogItemsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<CatalogItemDto> Handle(CatalogItemQuery request, CancellationToken cancellationToken)
        {
            var itm = await _repository.GetItem(request.Id.ToGuid());
            return AutoMap.Mapper.Map<CatalogItemDto>(itm);
        }
    }
}
