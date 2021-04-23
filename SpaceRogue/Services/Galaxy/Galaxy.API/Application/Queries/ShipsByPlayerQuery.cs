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
	public class ShipsByPlayerQuery : IRequest<IEnumerable<ShipDto>> 
    {
        [DataMember]
        public string Id { get; private set; }

        public ShipsByPlayerQuery(string id)
        {
            Id = id;
        }

        public class ShipsQueryHandler : IRequestHandler<ShipsByPlayerQuery, IEnumerable<ShipDto>>
        {
            private readonly IShipRepository _repository;

            public ShipsQueryHandler(IShipRepository repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            public async Task<IEnumerable<ShipDto>> Handle(ShipsByPlayerQuery request, CancellationToken cancellationToken)
            {
                var itms = await _repository.GetItems();
                var dtos = new List<ShipDto>();

                foreach (var itm in itms)
                {
                    dtos.Add(AutoMap.Mapper.Map<ShipDto>(itm));
                }

                return dtos;
            }
        }
    }
}
