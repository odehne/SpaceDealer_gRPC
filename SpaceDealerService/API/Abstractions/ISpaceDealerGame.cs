using Serilog;
using SpaceDealerModels.Units;

namespace SpaceDealerService.API.Abstractions
{
	public interface ISpaceDealerGame
	{
		ILogger Logger { get; set; }
		//Planets Galaxy { get; set; }
		//Players FleetCommanders { get; set; }
		
		void Init();
	}
}
