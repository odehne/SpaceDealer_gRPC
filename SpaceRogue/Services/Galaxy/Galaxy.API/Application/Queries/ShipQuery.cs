using Cope.SpaceRogue.Galaxy.API;
using Cope.SpaceRogue.Galaxy.API.Application.Commands;
using Cope.SpaceRogue.Galaxy.API.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Galaxy.Creator.Application.Commands
{
	public class ShipQuery : IRequest<ShipDto> 
    {
        [DataMember]
        public string Id { get; private set; }

        public ShipQuery(string id)
        {
            Id = id;
        }
    }

    public class ShipQueryHandler : IRequestHandler<ShipQuery, ShipDto>
    {
        private readonly IShipRepository _repository;

        public ShipQueryHandler(IShipRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ShipDto> Handle(ShipQuery request, CancellationToken cancellationToken)
        {
            var itm = await _repository.GetItem(request.Id.ToGuid());
            return AutoMap.Mapper.Map<ShipDto>(itm);
        }
    }
}
