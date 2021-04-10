using Cope.SpaceRogue.Galaxy.Creator.Application.Commands;
using Cope.SpaceRogue.Galaxy.Creator.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Galaxy.Creator.Application.Commands
{
    public class PlanetsQuery : IRequest<IEnumerable<PlanetDTO>> { }

    public class PlanetsQueryHandler : IRequestHandler<PlanetsQuery, IEnumerable<PlanetDTO>>
    {
        private readonly IPlanetRepository _repository;

        public PlanetsQueryHandler(IPlanetRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<PlanetDTO>> Handle(PlanetsQuery request, CancellationToken cancellationToken)
        {
            {
                var itms = await _repository.GetItems();
                var dtos = new List<PlanetDTO>();

                foreach (var itm in itms)
                {
                    dtos.Add(PlanetDTO.MapToDto(itm));
                }

                return dtos;
            }
        }
    }
}
