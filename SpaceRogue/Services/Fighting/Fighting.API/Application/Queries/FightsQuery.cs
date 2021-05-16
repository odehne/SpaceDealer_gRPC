using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Fighting.API.Models;
using Cope.SpaceRogue.Fighting.API.Repositories;

namespace Cope.SpaceRogue.Fighting.Application.Queries
{
	public class FightsQuery : IRequest<List<FightModel>> { }

    public class FightsQueryHandler : IRequestHandler<FightsQuery, List<FightModel>>
    {
        private readonly IFightRepository _repository;

        public FightsQueryHandler(IFightRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<List<FightModel>> Handle(FightsQuery request, CancellationToken cancellationToken)
        {
            var itms = await _repository.GetItems();
            var model = new List<FightModel>();

            foreach (var itm in itms)
            {
                model.Add(new FightModel(itm.RoundNumber, ShipModel.MapTo(itm.Attacker), ShipModel.MapTo(itm.Defender)));
            }

            return model;
        }
    }
}
