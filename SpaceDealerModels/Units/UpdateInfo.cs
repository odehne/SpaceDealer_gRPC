using SpaceDealer.Enums;

namespace SpaceDealerModels.Units
{
	public class UpdateInfo
	{
		public Ship TheShip;
		public UpdateStates UpdateState {get; set;} 

		public UpdateInfo(Ship ship, UpdateStates state)
		{
			UpdateState = state;
			TheShip = ship;
		}

		public UpdateInfo()
		{
		}
	}
}