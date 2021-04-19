using Cope.SpaceRogue.Galaxy.API.Repositories;
using MediatR;
using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{
	public class DeleteProductGroupComand : IRequest<bool>
	{
		[DataMember]
		public string Id { get; private set; }

		public DeleteProductGroupComand(string productId)
		{
			Id = productId;
		}
	}

	public class DeleteProductGroupComandHandler : IRequestHandler<DeleteProductGroupComand, bool>
	{
		private readonly IProductGroupRepository _repository;
		private readonly IMediator _mediator;

		public DeleteProductGroupComandHandler(IMediator mediator, ProductGroupRepository repository)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<bool> Handle(DeleteProductGroupComand request, CancellationToken cancellationToken)
		{
			var product = await _repository.GetItem(request.Id.ToGuid());
			if (product == null)
				throw new ArgumentException($"ProductGroup with id {request.Id} not found.");

			var result = await _repository.DeleteItem(product);

			return result;
		}

	}
}
