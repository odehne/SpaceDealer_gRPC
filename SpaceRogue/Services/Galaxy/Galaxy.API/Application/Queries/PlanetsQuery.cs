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
    public class PlanetsQuery : IRequest<IEnumerable<PlanetDto>> { }

    public class PlanetsQueryHandler : IRequestHandler<PlanetsQuery, IEnumerable<PlanetDto>>
    {
        private readonly IPlanetRepository _repository;

        public PlanetsQueryHandler(IPlanetRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<PlanetDto>> Handle(PlanetsQuery request, CancellationToken cancellationToken)
        {
            {
                var itms = await _repository.GetItems();
                var dtos = new List<PlanetDto>();

                foreach (var itm in itms)
                {
                    dtos.Add(AutoMap.Mapper.Map<PlanetDto>(itm));
                }

                return dtos;
            }
        }
    }
}
