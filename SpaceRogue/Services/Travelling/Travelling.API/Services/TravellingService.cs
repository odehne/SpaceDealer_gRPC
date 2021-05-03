using Cope.SpaceRogue.Travelling.API.Proto;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Traveling.API.Services
{
	public class TravellingService : TravelService.TravelServiceBase
	{
		private readonly IMediator _mediator;
		private readonly ILogger<TravellingService> _logger;

		public TravellingService(IMediator mediator, ILogger<TravellingService> logger)
		{
			_mediator = mediator;
			_logger = logger;
		}

	}
}