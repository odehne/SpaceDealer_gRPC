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
    public class FeaturesQuery : IRequest<IEnumerable<FeatureDto>> { }

    public class FeaturesQueryHandler : IRequestHandler<FeaturesQuery, IEnumerable<FeatureDto>>
    {
        private readonly IFeatureRepository _repository;

        public FeaturesQueryHandler(IFeatureRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<FeatureDto>> Handle(FeaturesQuery request, CancellationToken cancellationToken)
        {
            {
                var itms = await _repository.GetItems();
                var dtos = new List<FeatureDto>();

                foreach (var itm in itms)
                {
                    dtos.Add(AutoMap.Mapper.Map<FeatureDto>(itm));
                }

                return dtos;
            }
        }
    }
}
