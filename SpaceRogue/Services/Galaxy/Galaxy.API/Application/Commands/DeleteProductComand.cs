using Cope.SpaceRogue.Galaxy.API.Repositories;
using MediatR;
using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{

	public class DeleteProductComand : IRequest<bool>
	{
		[DataMember]
		public string Id { get; private set; }

		public DeleteProductComand(string productId)
		{
			Id = productId;
		}
	}

	public class DeleteProductComandHandler : IRequestHandler<DeleteProductComand, bool>
	{
		private readonly IProductRepository _repository;
		private readonly IMediator _mediator;

		public DeleteProductComandHandler(IMediator mediator, ProductRepository repository)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<bool> Handle(DeleteProductComand request, CancellationToken cancellationToken)
		{
			var product = await _repository.GetItem(request.Id.ToGuid());
			if (product == null)
				throw new ArgumentException($"Product with id {request.Id} not found.");

			var result = await _repository.DeleteItem(product);

			return result;
		}

	}
}
