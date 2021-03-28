using Cope.SpaceRogue.Galaxy.Creator.Proto;
using Grpc.Core;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.Creator.Services
{
	public class MarketPlaceService : MarketPlacesService.MarketPlacesServiceBase
	{
		public override Task<AddMarketPlaceReply> AddMarketPlace(AddMarketPlacegRequest request, ServerCallContext context)
		{
			return base.AddMarketPlace(request, context);
		}

		public override Task<DeleteMarketPlaceReply> DeleteMarketPlace(DeleteMarketPlaceRequest request, ServerCallContext context)
		{
			return base.DeleteMarketPlace(request, context);
		}

		public override Task<UpdateMarketPlaceReply> UpdateMarketPlace(UpdateMarketPlaceRequest request, ServerCallContext context)
		{
			return base.UpdateMarketPlace(request, context);
		}
	}
}
