using Cope.SpaceRogue.Galaxy.API.Repositories;
using MediatR;
using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{

	public class DeletePlanetCommand : IRequest<bool>
	{
		[DataMember]
		public string Id { get; private set; }

		public DeletePlanetCommand(string planetId)
		{
			Id = planetId;
		}
	}

	public class DeletePlanetComandHandler : IRequestHandler<DeletePlanetCommand, bool>
	{
		private readonly IPlanetRepository _repository;
		private readonly IMediator _mediator;

		public DeletePlanetComandHandler(IMediator mediator, PlanetRepository repository)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<bool> Handle(DeletePlanetCommand request, CancellationToken cancellationToken)
		{
			var product = await _repository.GetItem(request.Id.ToGuid());
			if (product == null)
				throw new ArgumentException($"Planet with id {request.Id} not found.");

			var result = await _repository.DeleteItem(product);

			return result;
		}

	}
}
