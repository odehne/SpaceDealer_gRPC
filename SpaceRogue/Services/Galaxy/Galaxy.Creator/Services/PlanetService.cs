using Cope.SpaceRogue.Galaxy.Creator.Proto;
using Grpc.Core;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.Creator.Services
{
	public class PlanetService : PlanetsService.PlanetsServiceBase
	{
		public override Task<AddPlanetReply> AddPlanet(AddPlanetRequest request, ServerCallContext context)
		{
			return base.AddPlanet(request, context);
		}

		public override Task<DeletePlanetReply> DeletePlanet(DeletePlanetRequest request, ServerCallContext context)
		{
			return base.DeletePlanet(request, context);
		}

		public override Task<UpdatePlanetReply> UpdatePlanet(UpdatePlanetRequest request, ServerCallContext context)
		{
			return base.UpdatePlanet(request, context);
		}
	}
}
