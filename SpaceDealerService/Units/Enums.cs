namespace SpaceDealer.Enums
{
	public enum PlayerTypes
	{
		NPC = 0,
		Real = 1
	}

	public enum ShipState
	{
		InSpaceDock = 0,
		Moving = 1,
		UnderAttack = 4,
		InSpacePit = 8
	}

	public enum CargoBaySize
	{
		Small = 0,
		Medium = 1,
		Large = 2,
		ExtraLarge = 4,
		SuperVessle = 8
	}

	public enum ResultState
	{
		Failed = 0,
		OK = 1
	}
}