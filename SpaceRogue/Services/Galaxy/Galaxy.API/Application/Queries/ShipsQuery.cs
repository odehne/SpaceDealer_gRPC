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


	public class ShipsQuery : IRequest<IEnumerable<ShipDto>> { }

    public class ShipsQueryHandler : IRequestHandler<ShipsQuery, IEnumerable<ShipDto>>
    {
        private readonly IShipRepository _repository;

        public ShipsQueryHandler(IShipRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<ShipDto>> Handle(ShipsQuery request, CancellationToken cancellationToken)
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
