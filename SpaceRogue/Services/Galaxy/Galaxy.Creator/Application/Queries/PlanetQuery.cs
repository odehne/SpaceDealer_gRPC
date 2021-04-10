using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Cope.SpaceRogue.Galaxy.Creator;
using Cope.SpaceRogue.Galaxy.Creator.Application.Commands;
using Cope.SpaceRogue.Galaxy.Creator.Repositories;
using MediatR;

namespace Galaxy.Creator.Application.Commands
{

    public class PlanetQuery : IRequest<PlanetDTO> 
    { 
        [DataMember]
        public string PlanetId { get; private set; }

		public PlanetQuery(string planetId)
		{
			PlanetId = planetId;
		}
    }

      public class PlanetQueryHandler : IRequestHandler<PlanetQuery, PlanetDTO>
    {
        private readonly IPlanetRepository _repository;

        public PlanetQueryHandler(IPlanetRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<PlanetDTO> Handle(PlanetQuery request, CancellationToken cancellationToken)
        {
            var itm = await _repository.GetItem(request.PlanetId.ToGuid());
            return PlanetDTO.MapToDto(itm);
        }
    }
}
