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

	public class PlayerQuery : IRequest<PlayerDto> 
    { 
        [DataMember]
        public string Id { get; private set; }

		public PlayerQuery(string id)
		{
			Id = id;
		}
    }
  public class PlayerQueryHandler : IRequestHandler<PlayerQuery, PlayerDto>
    {
        private readonly IPlayerRepository _repository;

        public PlayerQueryHandler(IPlayerRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<PlayerDto> Handle(PlayerQuery request, CancellationToken cancellationToken)
        {
                var itm = await _repository.GetItem(request.Id.ToGuid());
                
                return AutoMap.Mapper.Map<PlayerDto>(itm);
        }
    }
    
}
