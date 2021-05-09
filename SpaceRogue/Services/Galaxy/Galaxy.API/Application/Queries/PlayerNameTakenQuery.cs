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
	public class PlayerNameTakenQuery : IRequest<bool>
    {
        [DataMember]
        public string Name { get; private set; }

        public PlayerNameTakenQuery(string name)
        {
            Name = name;
        }
    }

    public class PlayerNameTakenQueryHandler : IRequestHandler<PlayerNameTakenQuery, bool>
    {
        private readonly IPlayerRepository _repository;

        public PlayerNameTakenQueryHandler(IPlayerRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<bool> Handle(PlayerNameTakenQuery request, CancellationToken cancellationToken)
        {
            var itm = await _repository.GetItemByName(request.Name);
			return itm != null;
		}
    }
}
