using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Shopping.API.Application.IntegrationEvents
{

	public class PlayerSoldProductIntegrationEvent : IntegrationEvent
	{
		public string PlayerId { get; set; }
		public double AccountBalance { get; set; }
		public string MarketPlaceId { get; set; }

	}

	public class PlayerSoldProductIntegrationEventHandler : IIntegrationEventHandler<PlayerSoldProductIntegrationEvent>
	{
		private readonly IEventBus _eventBus;
		private readonly ILogger<PlayerSoldProductIntegrationEventHandler> _logger;

		public PlayerSoldProductIntegrationEventHandler(IEventBus eventBus, ILogger<PlayerSoldProductIntegrationEventHandler> logger)
		{
			_eventBus = eventBus;
			_logger = logger;
		}

		public async Task Handle(PlayerSoldProductIntegrationEvent @event)
		{
			_logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
			_eventBus.Publish(@event);
			await Task.CompletedTask;
		}
	}

}
