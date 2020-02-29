using SpaceDealerModels.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceDealerModels.Repositories
{
	public static class ShipFeatureRepository
	{
		public static List<string> ShipNames { get; set; }
		public static List<string> PlanetNames { get; set; }

		public static ShipFeatures Library { get; set; } 

		public static void Init()
		{
			NewFeatureLibary();
			NewSpaceShipNames();
			NewPlanetNames();
		}

		private static void NewPlanetNames()
		{
			PlanetNames = new List<string>();
			PlanetNames.Add("Onrigawa");
			PlanetNames.Add("Genduanov");
			PlanetNames.Add("Yenvarvis");
			PlanetNames.Add("Ulvomia");
			PlanetNames.Add("Yunerth");
			PlanetNames.Add("Roclite");
			PlanetNames.Add("Baoruta");
			PlanetNames.Add("Zacimia");
			PlanetNames.Add("Strore 3JBX");
			PlanetNames.Add("Llars 9EF2");
			PlanetNames.Add("Andonope");
			PlanetNames.Add("Yutroutania");
			PlanetNames.Add("Cucroria");
			PlanetNames.Add("Lanradus");
			PlanetNames.Add("Xeria");
			PlanetNames.Add("Viunides");
			PlanetNames.Add("Vokagawa");
			PlanetNames.Add("Noyabos");
			PlanetNames.Add("Chosie FPI9");
			PlanetNames.Add("Drichi A0K");
			PlanetNames.Add("Labbuizuno");
			PlanetNames.Add("Chegnuaphus");
			PlanetNames.Add("Indillon");
			PlanetNames.Add("Yecciuq");
			PlanetNames.Add("Xabos");
			PlanetNames.Add("Euria");
			PlanetNames.Add("Nichiruta");
			PlanetNames.Add("Crikulara");
			PlanetNames.Add("Thosie LLQ");
			PlanetNames.Add("Drillon CUQ");
			PlanetNames.Add("Chenduaphus");
			PlanetNames.Add("Enkelea");
			PlanetNames.Add("Kolmides");
			PlanetNames.Add("Lalrilia");
			PlanetNames.Add("Munides");
			PlanetNames.Add("Ruiria");
			PlanetNames.Add("Drizemia");
			PlanetNames.Add("Thaohines");
			PlanetNames.Add("Strypso CAM");
			PlanetNames.Add("Ninda 001B");
		}

		private static void NewSpaceShipNames()
		{
			ShipNames = new List<string>();
			ShipNames.Add("Dragontooth");
			ShipNames.Add("Anarchy");
			ShipNames.Add("Philadelphia");
			ShipNames.Add("Visitor");
			ShipNames.Add("Cain");
			ShipNames.Add("HWSS Deinonychus");
			ShipNames.Add("HMS Stalker");
			ShipNames.Add("HWSS Merkava");
			ShipNames.Add("BC Dark Phoenix");
			ShipNames.Add("HMS Kingfisher");
			ShipNames.Add("Pandora");
			ShipNames.Add("Oregon");
			ShipNames.Add("Tranquility");
			ShipNames.Add("Ark Royal");
			ShipNames.Add("BC Nihilus");
			ShipNames.Add("USS Trinity");
			ShipNames.Add("STS Loki");
			ShipNames.Add("STS Rebellion");
			ShipNames.Add("Shooting Star");
			ShipNames.Add("Gladiator");
			ShipNames.Add("Templar");
			ShipNames.Add("Voyager");
			ShipNames.Add("Atlas");
			ShipNames.Add("Providence");
			ShipNames.Add("USS The Messenger");
			ShipNames.Add("ISS Guardian");
			ShipNames.Add("SSE Starhunter");
			ShipNames.Add("SC Apollo");
			ShipNames.Add("CS Malta");
			ShipNames.Add("Enterprise");
		}

		public static string GetRandomShipName()
		{
			Random random = new Random();
			var i = random.Next(0, ShipNames.Count -1);
			return ShipNames[i];
		}

		private static void NewFeatureLibary()
		{
			Library = new ShipFeatures();
			Library.Add(new ShipFeature("Schilde", new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("Starke Verteidung", "Verteidigt mit d20") }));
			Library.Add(new ShipFeature("Erweitertes Waffenmodul", new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("Erweitertes Waffenmodul", "Angriff mit d20+3") }));
			Library.Add(new SignalRangeFeature("Signal Reichweite", new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("Reichweite [Sektoren]", "+1") }));
		}

		public static ShipFeatures GetFeatureSet(string[] featureNames )
		{
			var featureSet = new ShipFeatures();
			foreach (var featureName in featureNames)
			{
				featureSet.Add(GetFeature(featureName))
			}
			return featureSet;
		}

		public static ShipFeature GetFeature(string featureName)
		{
			return Library.FirstOrDefault(x => x.Name.Equals(featureName, StringComparison.InvariantCultureIgnoreCase);
		}

	}
}
