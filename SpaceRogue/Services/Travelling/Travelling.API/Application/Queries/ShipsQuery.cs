using Cope.SpaceRogue.Travelling.API.Models;
using Cope.SpaceRogue.Travelling.API.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cope.SpaceRogue.Infrastructure;

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
            var model = new List<ShipModel>();

            foreach (var itm in itms)
            {
                model.Add(new ShipModel
                {
                    ShipId = itm.ID,
                    Name = itm.Name,
                    CurrentSector = new Position(0, 0, 0),
                    TargetSector = new Position(0, 0, 0),
                    SensorRange = 1,
                    Speed = 1
                });
            }

            return model;
        }
    }

}
