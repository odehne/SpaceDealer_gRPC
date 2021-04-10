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
    public class PlayerQuery : IRequest<PlayerDTO> 
    { 
        [DataMember]
        public string PlayerId { get; private set; }

		public PlayerQuery(string playerId)
		{
			PlayerId = playerId;
		}
    }
  public class PlayerQueryHandler : IRequestHandler<PlayerQuery, PlayerDTO>
    {
        private readonly IPlayerRepository _repository;

        public PlayerQueryHandler(IPlayerRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<PlayerDTO> Handle(PlayerQuery request, CancellationToken cancellationToken)
        {
                var player = await _repository.GetItem(request.PlayerId.ToGuid());
                
                return PlayerDTO.MapToDto(player);
        }
    }
    
}
