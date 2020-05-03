using SpaceDealerModels.Units;

namespace SpaceDealer
{
	public interface ISpaceDealerGame
	{
		ILogger Logger { get; set; }
		Planets Galaxy { get; set; }
		Players FleetCommanders { get; set; }
		Planets ScanPlanetsInNearbySectors(DbShip ship, double range = 1);
		Ships ScanShipsInNearbySectors(DbShip ship, double range = 1);
		void Init();
	}
}
