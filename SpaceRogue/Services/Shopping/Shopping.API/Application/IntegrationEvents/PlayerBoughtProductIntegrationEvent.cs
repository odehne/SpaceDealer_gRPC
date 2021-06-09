using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Shopping.API.Application.IntegrationEvents
{
	public class PlayerBoughtProductIntegrationEvent : IntegrationEvent
	{
		public string PlayerId { get; set; }
		public double AccountBalance { get; set; }
		public string MarketPlaceId { get; set; }
	}
	public class PlayerBoughtProductIntegrationEventHandler : IIntegrationEventHandler<PlayerBoughtProductIntegrationEvent>
	{
		private readonly IEventBus _eventBus;
		private readonly ILogger<PlayerBoughtProductIntegrationEventHandler> _logger;

		public PlayerBoughtProductIntegrationEventHandler(IEventBus eventBus, ILogger<PlayerBoughtProductIntegrationEventHandler> logger)
		{
			_eventBus = eventBus;
			_logger = logger;
		}

		public async Task Handle(PlayerBoughtProductIntegrationEvent @event)
		{
			_logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
			_eventBus.Publish(@event);
			await Task.CompletedTask;
		}
	}
}
