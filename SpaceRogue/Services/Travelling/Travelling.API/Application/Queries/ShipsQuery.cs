using Cope.SpaceRogue.Travelling.API.Models;
using Cope.SpaceRogue.Travelling.API;
using Cope.SpaceRogue.Travelling.API.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Cope.SpaceRogue.Travelling.Application.Commands
{
	public class ShipsQuery : IRequest<List<ShipModel>> { }

    public class ShipsQueryHandler : IRequestHandler<ShipsQuery, List<ShipModel>>
    {
        private readonly IShipRepository _repository;

        public ShipsQueryHandler(IShipRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<List<ShipModel>> Handle(ShipsQuery request, CancellationToken cancellationToken)
        {
            var itms = await _repository.GetItems();
            var dtos = new List<ShipModel>();

            foreach (var itm in itms)
            {
                dtos.Add(AutoMap.Mapper.Map<ShipModel>(itm));
            }

            return dtos;
        }
    }

}
