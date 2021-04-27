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
	public class PlayerByNameQuery : IRequest<PlayerDto>
    {
        [DataMember]
        public string Name { get; private set; }

        public PlayerByNameQuery(string name)
        {
            Name = name;
        }
    }

    public class PPlayerByNameQueryHandler : IRequestHandler<PlayerByNameQuery, PlayerDto>
    {
        private readonly IPlayerRepository _repository;

        public PPlayerByNameQueryHandler(IPlayerRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<PlayerDto> Handle(PlayerByNameQuery request, CancellationToken cancellationToken)
        {
            var itm = await _repository.GetItemByName(request.Name);

            return AutoMap.Mapper.Map<PlayerDto>(itm);
        }
    }
}
