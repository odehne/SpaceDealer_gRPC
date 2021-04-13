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
    public class PlayersQuery : IRequest<IEnumerable<PlayerDto>> { }

    public class PlayersQueryHandler : IRequestHandler<PlayersQuery, IEnumerable<PlayerDto>>
    {
        private readonly IPlayerRepository _repository;

        public PlayersQueryHandler(IPlayerRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<PlayerDto>> Handle(PlayersQuery request, CancellationToken cancellationToken)
        {
            {
                var itms = await _repository.GetItems();
                var dtos = new List<PlayerDto>();

                foreach (var itm in itms)
                {
                    dtos.Add(AutoMap.Mapper.Map<PlayerDto>(itm));
                }

                return dtos;
            }
        }
    }
}
