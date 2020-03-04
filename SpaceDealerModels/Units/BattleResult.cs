namespace SpaceDealerModels.Units
{
	public class BattleResult
	{
		public string Message { get; set; }
		public int Value { get; set; }
		public bool DefenderWasHit { get; set; }
		public bool CriticalHit { get; set; }
		public double Treasure { get; set; }
		public bool Defeaded { get; set; }
	}
}
