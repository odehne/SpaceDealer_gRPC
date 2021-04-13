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

    public class PlanetQuery : IRequest<PlanetDto> 
    { 
        [DataMember]
        public string PlanetId { get; private set; }

		public PlanetQuery(string planetId)
		{
			PlanetId = planetId;
		}
    }

    public class PlanetQueryHandler : IRequestHandler<PlanetQuery, PlanetDto>
    {
        private readonly IPlanetRepository _repository;

        public PlanetQueryHandler(IPlanetRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<PlanetDto> Handle(PlanetQuery request, CancellationToken cancellationToken)
        {
            var itm = await _repository.GetItem(request.PlanetId.ToGuid());

            return AutoMap.Mapper.Map<PlanetDto>(itm);
        }
    }
}
