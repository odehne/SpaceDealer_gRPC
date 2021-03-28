using Cope.SpaceRogue.Galaxy.API.Proto;
using Grpc.Core;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Services.Galaxy.API.Services
{
	public class PlanetService : PlanetsService.PlanetsServiceBase
	{	
		public override Task<PlanetReply> GetPlanet(PlanetRequest request, ServerCallContext context)
		{
			return base.GetPlanet(request, context);
		}

		public override Task<PlanetsReply> GetPlanets(SectorRequest request, ServerCallContext context)
		{
			return base.GetPlanets(request, context);
		}
	}
}
