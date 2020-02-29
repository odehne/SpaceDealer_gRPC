using SpaceDealerModels.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceDealerModels.Repositories
{
	public static class Repository
	{
		public static List<string> ShipNames { get; set; }
		public static List<string> PlanetNames { get; set; }

		public static ShipFeatures Library { get; set; } 

		public static ProductsInStock ProductLibrary { get; set; }

		public static void Init()
		{
			NewFeatureLibary();
			NewSpaceShipNames();
			NewPlanetNames();
			NewProductLibary();
		}

		private static void NewPlanetNames()
		{
			PlanetNames = new List<string>
			{
				"Onrigawa",
				"Genduanov",
				"Yenvarvis",
				"Ulvomia",
				"Yunerth",
				"Roclite",
				"Baoruta",
				"Zacimia",
				"Strore 3JBX",
				"Llars 9EF2",
				"Andonope",
				"Yutroutania",
				"Cucroria",
				"Lanradus",
				"Xeria",
				"Viunides",
				"Vokagawa",
				"Noyabos",
				"Chosie FPI9",
				"Drichi A0K",
				"Labbuizuno",
				"Chegnuaphus",
				"Indillon",
				"Yecciuq",
				"Xabos",
				"Euria",
				"Nichiruta",
				"Crikulara",
				"Thosie LLQ",
				"Drillon CUQ",
				"Chenduaphus",
				"Enkelea",
				"Kolmides",
				"Lalrilia",
				"Munides",
				"Ruiria",
				"Drizemia",
				"Thaohines",
				"Strypso CAM",
				"Ninda 001B"
			};
		}

		private static void NewSpaceShipNames()
		{
			ShipNames = new List<string>
			{
				"Dragontooth",
				"Anarchy",
				"Philadelphia",
				"Visitor",
				"Cain",
				"HWSS Deinonychus",
				"HMS Stalker",
				"HWSS Merkava",
				"BC Dark Phoenix",
				"HMS Kingfisher",
				"Pandora",
				"Oregon",
				"Tranquility",
				"Ark Royal",
				"BC Nihilus",
				"USS Trinity",
				"STS Loki",
				"STS Rebellion",
				"Shooting Star",
				"Gladiator",
				"Templar",
				"Voyager",
				"Atlas",
				"Providence",
				"USS The Messenger",
				"ISS Guardian",
				"SSE Starhunter",
				"SC Apollo",
				"CS Malta",
				"Enterprise"
			};
		}

		public static string GetRandomShipName()
		{
			Random random = new Random();
			var i = random.Next(0, PlanetNames.Count -1);
			return PlanetNames[i];
		}

		public static string GetRandomPlanetName()
		{
			Random random = new Random();
			var i = random.Next(0, ShipNames.Count - 1);
			return ShipNames[i];
		}

		public static ProductInStock GetRandomProduct()
		{
			Random random = new Random();
			var i = random.Next(0, ShipNames.Count - 1);
			return ProductLibrary[i];
		}

		public static ProductInStock GetProductByName(string name)
		{
			return ProductLibrary.SingleOrDefault(x=>x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
		}

		private static void NewProductLibary()
		{
			ProductLibrary = new ProductsInStock();
			ProductLibrary.AddProduct("Kuh-Milch", null, 0.2, 1.0, 1.0, 180);
			ProductLibrary.AddProduct("Mais", null, 0.2, 1.0, 1.0, 165.0);
			ProductLibrary.AddProduct("Weizen", null, 0.2, 1.0, 1.0, 171.58);
			ProductLibrary.AddProduct("Rindfleisch", null, 0.1, 0.5, 1.0, 3750.0);
			ProductLibrary.AddProduct("Schweinefleisch", null, 0.1, 0.5, 1.0, 1700.0);
			ProductLibrary.AddProduct("Reis", null, 0.1, 0.5, 1.0, 240.00);
			ProductLibrary.AddProduct("Soya", null, 0.1, 0.5, 1.0, 379.54);
			ProductLibrary.AddProduct("Öl", null, 1, 15, 1, 332.74);
			ProductLibrary.AddProduct("Braunkohle", null, 1, 7, 1, 40.0);
			ProductLibrary.AddProduct("Solarpanele", null, 0.5, 10, 1, 8780.0);
			ProductLibrary.AddProduct("Orangensaft", null, 0.1, 0.5, 1, 1900.0);
			ProductLibrary.AddProduct("Bacon", null, 0.1, 0.5, 1, 2800.0);
			ProductLibrary.AddProduct("Kleines Raumschiff Kapazität (30t)", null, .1, 0, 50.0, 13000000000.0);
			ProductLibrary.AddProduct("Mittleres Raumschiff Kapazität (60t)", null, .05, 0, 100.0, 17000000000.0);
			ProductLibrary.AddProduct("Kreuzer (100t) +Bewaffnung", null, .02, 0, 150.0, 23000000000.0);
			ProductLibrary.AddProduct("Sensor-Einheit", null, .2, 0, 1, 7996.0);
			ProductLibrary.AddProduct("Board-Kanone", null, .2, 0, 1, 79960.0);
			ProductLibrary.AddProduct("Tie-Fighter Flügel", null, .1, .1, 1, 2000.0);
			ProductLibrary.AddProduct("Wasser Evaporatoren", null, .3, 0, 1, 17456.0);
			ProductLibrary.AddProduct("Sternen-Zerstörer Triebwerke", null, .3, 0, 700, 450000000000.0);
			ProductLibrary.AddProduct("Cyberkristalle", null, .001, .1, .1, 450000000);
			ProductLibrary.AddProduct("Medizinische Produkte", null, .001, 2, .01, 29000.0);
			ProductLibrary.AddProduct("Anti-Schwerkraft Generator", null, .3, 0, 0.5, 350000.0);
			ProductLibrary.AddProduct("Musikinstrumente", null, 0.2, 2, 0.05, 87400.0);
			ProductLibrary.AddProduct("Bohrmaschinen", null, 0.1, 1, 0.05, 87500.0);
			ProductLibrary.AddProduct("Fische", null, 1, 10, 1, 5750.0);
			ProductLibrary.AddProduct("Schalentiere", null, 1, 10, 1, 6750.0);
			ProductLibrary.AddProduct("Schnecken", null, 1, 1, 1, 3750.0);
			ProductLibrary.AddProduct("Wein", null, 1, 1, 1, 3200.0);
			ProductLibrary.AddProduct("Bier", null, 1, 1, 1, 2900.0);
			ProductLibrary.AddProduct("Wasserbüffel", null, 1, 1, 1, 2100.0);
			ProductLibrary.AddProduct("Pistolen", null, 1, 1, 1, 47000.0);
		}

		private static void NewFeatureLibary()
		{
			Library = new ShipFeatures();
			Library.Add(new ShipFeature("Schilde", new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("Starke Verteidung", "Verteidigt mit d20") }));
			Library.Add(new ShipFeature("Erweitertes Waffenmodul", new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("Erweitertes Waffenmodul", "Angriff mit d20+3") }));
			Library.Add(new SignalRangeFeature("Signal Reichweite", new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("Reichweite [Sektoren]", "+1") }));
		}

		public static ProductsInStock GetRandomProducts(int howMany, bool needed)
		{
			var ret = new ProductsInStock();
			var multiplier = 1.0;
			if (needed)
				multiplier = 1.3;
			for (int i = 0; i < howMany-1; i++)
			{
				var p = GetRandomProduct();
				p.PricePerTon = p.PricePerTon * multiplier;
			}
			return ret;
		}

		public static ShipFeatures GetFeatureSet(string[] featureNames )
		{
			var featureSet = new ShipFeatures();
			foreach (var featureName in featureNames)
			{
				featureSet.Add(GetFeature(featureName));
			}
			return featureSet;
		}

		public static ShipFeature GetFeature(string featureName)
		{
			return Library.FirstOrDefault(x => x.Name.Equals(featureName, StringComparison.InvariantCultureIgnoreCase));
		}

	}
}
