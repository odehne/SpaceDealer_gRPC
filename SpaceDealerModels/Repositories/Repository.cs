using SpaceDealer.Enums;
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
		public static Industries IndustryLibrary { get; set; }

		public static ProductsInStock ProductLibrary { get; set; }
		//public static Planets PlanetLibrary { get; set; }

		public static void Init()
		{
			NewFeatureLibary();
			NewSpaceShipNames();
			NewPlanetNames();
			NewProductLibary();
			NewIndustryLibrary();
			//NewPlanetLibrary();
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
			var i = random.Next(0, PlanetNames.Count - 1);
			return PlanetNames[i];
		}

		public static Planet GetRandomPlanet(Coordinates sector)
		{
			var planetName = GetRandomPlanetName();
			var p = new Planet(planetName);
			p.Market = new Market("Marktplatz von " + planetName, p);
			p.Industries = new Industries(p);
			p.Sector = sector;
			p.Industries.Add(GetRandomIndustry());
			p.Industries[0].ProductsNeeded.AddProduct(GetRandomProduct());
			p.Industries[0].ProductsNeeded.AddProduct(GetRandomProduct());
			p.Industries[0].ProductsNeeded.AddProduct(GetRandomProduct());
			p.Industries[0].ProductsNeeded.AddProduct(GetRandomProduct());
			return p;
		}

		public static void NewIndustryLibrary()
		{
			IndustryLibrary = new Industries();
			var weltraumSchrott = IndustryLibrary.AddIndustry("Weltraumschrott Sammler");
				weltraumSchrott.AddGeneratedProduct(Repository.GetProductByName("Tie-Fighter Flügel"));
				weltraumSchrott.AddGeneratedProduct(Repository.GetProductByName("Wasser Evaporatoren"));
				weltraumSchrott.AddGeneratedProduct(Repository.GetProductByName("Sternen-Zerstörer Triebwerke"));
				weltraumSchrott.AddGeneratedProduct(Repository.GetProductByName("Cyberkristalle"));
				weltraumSchrott.AddNeededProduct(Repository.GetProductByName("Kuh-Milch"));
				weltraumSchrott.AddNeededProduct(Repository.GetProductByName("Reis"));
				weltraumSchrott.AddNeededProduct(Repository.GetProductByName("Wasser"));
			var moonFactory = IndustryLibrary.AddIndustry("Raumschiff Fabrik");
				moonFactory.AddGeneratedProduct(Repository.GetProductByName("Kleines Raumschiff Kapazität (30t)"));
				moonFactory.AddGeneratedProduct(Repository.GetProductByName("Mittleres Raumschiff Kapazität (60t)"));
				moonFactory.AddGeneratedProduct(Repository.GetProductByName("Kreuzer (100t) +Bewaffnung"));
				moonFactory.AddGeneratedProduct(Repository.GetProductByName("Sensor-Einheit"));
				moonFactory.AddGeneratedProduct(Repository.GetProductByName("Board-Kanone"));
			var farming = IndustryLibrary.AddIndustry("Landwirtschaft");
				farming.AddGeneratedProduct(Repository.GetProductByName("Kuh-Milch"));
				farming.AddGeneratedProduct(Repository.GetProductByName("Mais"));
				farming.AddGeneratedProduct(Repository.GetProductByName("Rindfleisch"));
				farming.AddGeneratedProduct(Repository.GetProductByName("Reis"));
				farming.AddGeneratedProduct(Repository.GetProductByName("Soya"));
				farming.AddGeneratedProduct(Repository.GetProductByName("Orangensaft"));
				farming.AddGeneratedProduct(Repository.GetProductByName("Bacon"));
				farming.AddNeededProduct(Repository.GetProductByName("Gitarren"));
				farming.AddNeededProduct(Repository.GetProductByName("Fische"));
				farming.AddNeededProduct(Repository.GetProductByName("Bohrmaschinen"));
			var fishing = IndustryLibrary.AddIndustry("Fishfang");
				fishing.AddGeneratedProduct(Repository.GetProductByName("Schalentiere"));
				fishing.AddGeneratedProduct(Repository.GetProductByName("Calmare"));
				fishing.AddGeneratedProduct(Repository.GetProductByName("Fische"));
				fishing.AddGeneratedProduct(Repository.GetProductByName("Wasser"));
				fishing.AddGeneratedProduct(Repository.GetProductByName("Algen/Seeigel"));
			var musik = IndustryLibrary.AddIndustry("Musikinstrumente");
				musik.AddGeneratedProduct(Repository.GetProductByName("Gitarren"));
				musik.AddGeneratedProduct(Repository.GetProductByName("Holzblasinstrumente"));
				musik.AddGeneratedProduct(Repository.GetProductByName("Blechblasinstrumente"));
				musik.AddGeneratedProduct(Repository.GetProductByName("Streichinstrumente"));
				musik.AddGeneratedProduct(Repository.GetProductByName("Schlagzeug"));
			var werkzeuge = IndustryLibrary.AddIndustry("Werkzeuge");
				werkzeuge.AddGeneratedProduct(Repository.GetProductByName("Wasser Evaporatoren"));
				werkzeuge.AddGeneratedProduct(Repository.GetProductByName("Bohrmaschinen"));
				werkzeuge.AddGeneratedProduct(Repository.GetProductByName("Abraumwerkzeuge"));
				werkzeuge.AddGeneratedProduct(Repository.GetProductByName("Anti-Schwerkraft Generator"));
		}

		public static ProductInStock GetRandomProduct()
		{
			Random random = new Random();
			var i = random.Next(0, ProductLibrary.Count - 1);
			return ProductLibrary[i];
		}

		public static Industry GetRandomIndustry()
		{
			Random random = new Random();
			var i = random.Next(0, IndustryLibrary.Count - 1);
			return IndustryLibrary[i];
		}

		public static ProductInStock GetProductByName(string name)
		{
			return ProductLibrary.SingleOrDefault(x=>x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
		}

		private static void NewProductLibary()
		{
			ProductLibrary = new ProductsInStock();
			ProductLibrary.AddProduct("Kuh-Milch", 0.2, 1.0, 1.0, 0.18);
			ProductLibrary.AddProduct("Wasser", 0.2, 1.0, 1.0, 0.54);
			ProductLibrary.AddProduct("Mais", 0.2, 1.0, 1.0, 0.16);
			ProductLibrary.AddProduct("Weizen", 0.2, 1.0, 1.0, 0.17);
			ProductLibrary.AddProduct("Rindfleisch", 0.1, 0.5, 1.0, 3.75);
			ProductLibrary.AddProduct("Schweinefleisch", 0.1, 0.5, 1.0, 1.7);
			ProductLibrary.AddProduct("Reis", 0.1, 0.5, 1.0, 0.24);
			ProductLibrary.AddProduct("Soya", 0.1, 0.5, 1.0, 0.37);
			ProductLibrary.AddProduct("Öl", 1, 15, 1, 0.33);
			ProductLibrary.AddProduct("Braunkohle", 1, 7, 1, 0.04);
			ProductLibrary.AddProduct("Solarpanele", 0.5, 10, 1, 8.78);
			ProductLibrary.AddProduct("Orangensaft", 0.1, 0.5, 1, 1.9);
			ProductLibrary.AddProduct("Bacon", 0.1, 0.5, 1, 2.8);
			ProductLibrary.AddProduct("Kleines Raumschiff Kapazität (30t)", .1, 0, 50.0, 13000000.0);
			ProductLibrary.AddProduct("Mittleres Raumschiff Kapazität (60t)", .05, 0, 100.0, 17000000.0);
			ProductLibrary.AddProduct("Kreuzer (100t) +Bewaffnung", .02, 0, 150.0, 23000000.0);
			ProductLibrary.AddProduct("Sensor-Einheit", .2, 0, 1, 7.99);
			ProductLibrary.AddProduct("Board-Kanone", .2, 0, 1, 79.96);
			ProductLibrary.AddProduct("Tie-Fighter Flügel", .1, .1, 1, 2.00);
			ProductLibrary.AddProduct("Wasser Evaporatoren", .3, 0, 1, 17.45);
			ProductLibrary.AddProduct("Sternen-Zerstörer Triebwerke", .3, 0, 700, 450000000.0);
			ProductLibrary.AddProduct("Cyberkristalle", .001, .1, .1, 450000.0);
			ProductLibrary.AddProduct("Medizinische Produkte", .001, 2, .01, 29.0);
			ProductLibrary.AddProduct("Anti-Schwerkraft Generator", .3, 0, 0.5, 350.0);
			
			ProductLibrary.AddProduct("Gitarren", 0.2, 2, 0.05, 87.4);
			ProductLibrary.AddProduct("Holzblasinstrumente", 0.2, 2, 0.05, 82.7);
			ProductLibrary.AddProduct("Blechblasinstrumente", 0.2, 2, 0.05, 93.4);
			ProductLibrary.AddProduct("Streichinstrumente", 0.2, 2, 0.05, 67.9);
			ProductLibrary.AddProduct("Schlagzeug", 0.2, 2, 0.05, 52.7);

			ProductLibrary.AddProduct("Bohrmaschinen", 0.1, 1, 0.05, 87.5);
			ProductLibrary.AddProduct("Abraumwerkzeuge", 0.1, 1, 10.0, 1932.5);
			
			ProductLibrary.AddProduct("Fische", 1, 10, 1, 5.75);
			ProductLibrary.AddProduct("Schalentiere", 1, 10, 1, 6.75);
			ProductLibrary.AddProduct("Calmare", 1, 10, 1, 6.35);
			ProductLibrary.AddProduct("Algen/Seeigel", 1, 10, 1, 1.75);
			ProductLibrary.AddProduct("Schnecken", 1, 1, 1, 3.75);
			ProductLibrary.AddProduct("Wein", 1, 1, 1, 3.20);
			ProductLibrary.AddProduct("Bier", 1, 1, 1, 2.90);
			ProductLibrary.AddProduct("Wasserbüffel", 1, 1, 1, 2.10);
			ProductLibrary.AddProduct("Pistolen", 1, 1, 1, 47.0);
			ProductLibrary.AddProduct("Phaser", 1, 1, 1, 147.00);
			ProductLibrary.AddProduct("Torpedos", 1, 1, 1, 928.0);

		}

		private static void NewFeatureLibary()
		{
			Library = new ShipFeatures();
			Library.Add(new ShipFeature("ShieldBonus+1", "Verbesserte Schile (+1)", 0, 1, 0,0));
			Library.Add(new ShipFeature("Phasers+1", "Phaser-Bänke", 1, 0, 0,0));
			Library.Add(new ShipFeature("Torpedos+2", "Torpedos", 2, 1, 0,0));
			Library.Add(new ShipFeature("AdvancedWarp+1", "Verbesserte Geschwindigkeit", 0, 0, 0, 1));
			Library.Add(new ShipFeature("SignalRange+1", "Verbesserter Signalempfang", 0, 0, 1,0));
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
