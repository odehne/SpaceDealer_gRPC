using Cope.SpaceRogue.Galaxy.Creator.Application.Commands;
using Cope.SpaceRogue.Galaxy.Creator.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Galaxy.Creator.Application.Commands
{
    public class PlayersQuery : IRequest<IEnumerable<PlayerDTO>> { }

    public class PlayersQueryHandler : IRequestHandler<PlayersQuery, IEnumerable<PlayerDTO>>
    {
        private readonly IPlayerRepository _repository;

        public PlayersQueryHandler(IPlayerRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<PlayerDTO>> Handle(PlayersQuery request, CancellationToken cancellationToken)
        {
            {
                var itms = await _repository.GetItems();
                var dtos = new List<PlayerDTO>();

                foreach (var itm in itms)
                {
                    dtos.Add(PlayerDTO.MapToDto(itm));
                }

                return dtos;
            }
        }
    }
}
