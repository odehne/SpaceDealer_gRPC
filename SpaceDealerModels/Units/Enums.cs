namespace SpaceDealer.Enums
{
	public enum PlayerTypes
	{
		NPC = 0,
		Real = 1
	}

	public enum JourneyState
	{
		Travelling = 0,
		Arrived = 1,
		InBattle = 2,
		NewPlanetInRange = 3,
		OtherShipInRange = 4
	}

	public enum ShipState
	{
		Idle = 0,
		InJourney = 1,
		UnderConstruction = 2
	}

	public enum UpdateStates
	{
		ArrivedOnTarget = 0,
		UnderAttack = 1,
		OnRescueMission = 2,
		ShipIsGettingRepaired = 3,
		NewPlanetDiscovered = 4
	}

	public enum InterruptionType
	{
		AttackedByPirates = 0,
		OtherShipWantsToTrade = 1,
		DistressSignal = 2,
		DiscoveredNewPlanet = 3
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