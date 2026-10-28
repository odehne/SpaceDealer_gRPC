namespace SpaceDealerModels.Units
{
    public class GameStatistics
	{
        public int TotalPlayers { get; set; }
        public int TotalShips { get; set; }
        public int TotalPlanets { get; set; }
        public int TotalBattles { get; set; }
        public int TotalBattlesWon { get; set; }
        public int TotalBattlesLost { get; set; }
        public int TotalBattlesDraw { get; set; }
        public int TotalCriticalHits { get; set; }
        public int TotalTreasureCollected { get; set; } = 0;

    }
}
