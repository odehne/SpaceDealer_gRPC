using Cope.SpaceRogue.Maintenance.API.Proto;
using Grpc.Core;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Services.Maintenance.API.Services
{
	public class ShipService : ShipsService.ShipsServiceBase
	{
		public override Task<AddShipReply> AddShip(AddShipRequest request, ServerCallContext context)
		{
			return base.AddShip(request, context);
		}

		public override Task<ShipReply> GetShip(ShipRequest request, ServerCallContext context)
		{
			return base.GetShip(request, context);
		}

		public override Task<ShipsReply> GetShips(ShipsRequest request, ServerCallContext context)
		{
			return base.GetShips(request, context);
		}
	}
}
