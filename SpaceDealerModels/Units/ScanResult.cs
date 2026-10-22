namespace SpaceDealerModels.Units
{
    public class ScanResult
	{
		public Planets ScannedPlanets { get; set; }
        public Ships ScannedShips { get; set; }

        public ScanResult()
        {
            ScannedPlanets = new Planets();
            ScannedShips = new Ships();
        }
    }
}
