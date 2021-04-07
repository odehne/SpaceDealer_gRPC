using Cope.SpaceRogue.Galaxy.Creator.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.Creator.Application.Commands
{
	public class AddMarketPlaceCommandHandler : IRequestHandler<CreateMarketPlaceCommand, MarketPlaceDTO>
	{
		private readonly PlanetRepository _repository;
		private readonly IMediator _mediator;

		public AddMarketPlaceCommandHandler(IMediator mediator)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		}

		public async Task<MarketPlaceDTO> Handle(CreateMarketPlaceCommand request, CancellationToken cancellationToken)
		{
			var market = await _repository.GetItem(request.PlanetId.ToGuid());
			
			return new MarketPlaceDTO(request.MarketPlaceId, request.PlanetId, market.Name);
		}

	}
}
