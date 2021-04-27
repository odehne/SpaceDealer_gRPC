using Cope.SpaceRogue.Galaxy.API.Repositories;
using Cope.SpaceRogue.Infrastructure.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{

	public class AddShipCommand : IRequest<bool>
	{
		[DataMember]
		public string Id { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public string PlayerId { get; set; }
		[DataMember]
		public int Shields { get; set; }
		[DataMember]
		public int Hull { get; set; }
	}

	public class AddShipCommandHandler : IRequestHandler<AddShipCommand, bool>
	{
		private readonly IShipRepository _repository;
		
		public AddShipCommandHandler(IShipRepository repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<bool> Handle(AddShipCommand request, CancellationToken cancellationToken)
		{
			var ship = new Ship()
			{
				ID = request.Id.ToGuid(),
				Name = request.Name,
				Hull = request.Hull,
				Shields = request.Shields
			};
			var b = await _repository.AddItem(ship);
			return b;
		}
	}
}
