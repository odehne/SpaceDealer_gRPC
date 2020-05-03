using SpaceDealer.Enums;

namespace SpaceDealerModels.Units
{
	public class UpdateInfo
	{
		public DbShip TheShip;
		public UpdateStates UpdateState {get; set;} 

		public UpdateInfo(DbShip ship, UpdateStates state)
		{
			UpdateState = state;
			TheShip = ship;
		}

		public UpdateInfo()
		{
		}
	}
}