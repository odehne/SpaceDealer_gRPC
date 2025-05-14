using SpaceDealerModels.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceDealerModels.Repositories
{
	public static class Repository
	{
		public static List<string> ShipNames { get; set; }
		public static List<string> PlanetNames { get; set; }
        public static List<string> FleetCommanders { get; set; }
		public static DbFeatures Features { get; set; }
		public static List<DbIndustry> IndustryLibrary { get; set; }

		public static DbProductsInStock ProductLibrary { get; set; }
		//public static Planets PlanetLibrary { get; set; }

		public static void Init()
		{
            NewProductLibary();
            NewIndustryLibrary();
            NewSpaceShipNames();
			NewPlanetNames();
			LoadFeatures();
			NewPilotNames();
			
        }

		public static void LoadFeatures()
		{
			if (Features == null)
				Features = new DbFeatures();
			if (Features.Count < 5)
			{
				Features.Add(new DbFeature("ShieldBonus+1", "Verbesserte Schile (+1)", 0, 1, 0, 0));
				Features.Add(new DbFeature("Phasers+1", "Phaser-Bänke", 1, 0, 0, 0));
				Features.Add(new DbFeature("Torpedos+2", "Torpedos", 2, 1, 0, 0));
				Features.Add(new DbFeature("AdvancedWarp+1", "Verbesserte Geschwindigkeit", 0, 0, 0, 1));
				Features.Add(new DbFeature("SignalRange+1", "Verbesserter Signalempfang", 0, 0, 1, 0));
			}
		}

		public static void NewPilotNames()
		{
			FleetCommanders = new List<string>
				{
					"Stery Gonzal",
					"Raymy Reson",
					"Jeffry Watson",
					"Johny Whelley",
					"Kenne Barner",
					"Danio Parking",
					"Raymy Ander",
					"Jery Clery",
					"Jamy Ganes",
					"Phardy Hillee",
					"Justeph Hughy",
					"Tine Coopet",
					"Rege Belley",
					"Wardy Rodra",
					"Johnne Pera",
					"Aadan Jenkell",
					"Randy Hernes",
					"Justev Finels",
					"Peteph Sonett",
					"Grence Bennels",
					"Orion Blaze",
					"Jaxon Starfire",
					"Kael Nova",
					"Zane Eclipse",
					"Rylan Comet",
					"Lyra Skye",
					"Nova Quinn",
					"Zara Celeste",
					"Aria Nebula",
					"Vega Storm",
					"Talon Vortex",
					"Axel Orion",
					"Dax Zenith",
					"Finn Starhawk",
					"Jett Cosmos",
					"Nyx Astra",
					"Luna Vega",
					"Mira Solara",
					"Cassia Starwind",
					"Elara Phoenix",
					"Thane Solaris",
					"Kade Nebula",
					"Orion Voss",
					"Zephyr Quasar",
					"Jarek Pulsar",
					"Draven Starlight",
					"Corbin Meteor",
					"Xander Eclipse",
					"Raiden Astro",
					"Blaze Hyperion",
					"Selene Astraea",
					"Nyssa Comet",
					"Thalia Vortex",
					"Lyra Zenith",
					"Seraphina Starfire",
					"Kaida Nova",
					"Elara Eclipse",
					"Vesper Celeste",
					"Astrid Nebula",
					"Callista Vega"
				};
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
				"Enterprise",
				"Stellar Voyager",
				"Nebula Nomad",
				"Galactic Trader",
				"Cosmic Explorer",
				"Starbound Merchant",
				"Celestial Navigator",
				"Astro Pioneer",
				"Quantum Cruiser",
				"Interstellar Hauler",
				"Solar Seeker",
				"Lunar Ranger",
				"Eclipse Runner",
				"Meteor Marauder",
				"Orbit Raider",
				"Comet Carrier",
				"Planet Hopper",
				"Galaxy Drifter",
				"Spacefarer",
				"Asteroid Adventurer",
				"Warp Wanderer",
				"Photon Freighter",
				"Dark Matter Dealer",
				"Gravity Grappler",
				"Starship Trader",
				"Void Voyager",
            };
		}

		public static string GetRandomShipName()
		{
			Random random = new Random();
			var i = random.Next(0, ShipNames.Count -1);
			return ShipNames[i];
		}

        public static string GetRandomFleetCommanderName()
        {
            Random random = new Random();
            var i = random.Next(0, FleetCommanders.Count - 1);
            return FleetCommanders[i];
        }

        public static string GetRandomNumber(int minN, int maxN)
		{
			Random random = new Random();
			var i = random.Next(minN, maxN);
			return $"{i}".PadLeft(3, '0');
		}

		public static string GetRandomPlanetName()
		{
			Random random = new Random();
			var i = random.Next(0, PlanetNames.Count - 1);
			return PlanetNames[i];
		}

		public static DbPlanet GenerateRandomPlanet(DbCoordinates sector)
		{
			var planetName = GetRandomPlanetName();
			var p = new DbPlanet(planetName);
			p.Market = new DbMarket("Marktplatz von " + planetName, p);
			p.Industry = GetRandomIndustry();
			p.Sector = sector;
			return p;
		}

		public static DbIndustry GetIndustryByName(string industryName)
		{
			return IndustryLibrary.FirstOrDefault(x => x.Name.Equals(industryName, StringComparison.CurrentCultureIgnoreCase));
		}

		public static void NewIndustryLibrary()
		{
			IndustryLibrary = new List<DbIndustry>();
			var weltraumSchrott = new DbIndustry("Weltraumschrott Sammler");
				weltraumSchrott.AddGeneratedProduct(Repository.GetProductByName("Tie-Fighter Flügel"));
				weltraumSchrott.AddGeneratedProduct(Repository.GetProductByName("Wasser Evaporatoren"));
				weltraumSchrott.AddGeneratedProduct(Repository.GetProductByName("Sternen-Zerstörer Triebwerke"));
				weltraumSchrott.AddGeneratedProduct(Repository.GetProductByName("Cyberkristalle"));
				weltraumSchrott.AddNeededProduct(Repository.GetProductByName("Kuh-Milch"));
				weltraumSchrott.AddNeededProduct(Repository.GetProductByName("Reis"));
				weltraumSchrott.AddNeededProduct(Repository.GetProductByName("Wasser"));
			IndustryLibrary.Add(weltraumSchrott);
			var moonFactory = new DbIndustry("Raumschiff Fabrik");
				moonFactory.AddGeneratedProduct(Repository.GetProductByName("Kleines Raumschiff Kapazität (30t)"));
				moonFactory.AddGeneratedProduct(Repository.GetProductByName("Mittleres Raumschiff Kapazität (60t)"));
				moonFactory.AddGeneratedProduct(Repository.GetProductByName("Kreuzer (100t) +Bewaffnung"));
				moonFactory.AddGeneratedProduct(Repository.GetProductByName("Sensor-Einheit"));
				moonFactory.AddGeneratedProduct(Repository.GetProductByName("Board-Kanone"));
				moonFactory.AddNeededProduct(Repository.GetProductByName("Stahl"));
				moonFactory.AddNeededProduct(Repository.GetProductByName("Transpari-Stahl"));
				moonFactory.AddNeededProduct(Repository.GetProductByName("Cyberkristalle"));
				moonFactory.AddGeneratedProduct(Repository.GetProductByName("Reis"));
				moonFactory.AddGeneratedProduct(Repository.GetProductByName("Soya"));
				moonFactory.AddGeneratedProduct(Repository.GetProductByName("Orangensaft"));
				moonFactory.AddGeneratedProduct(Repository.GetProductByName("Bacon"));
            IndustryLibrary.Add(moonFactory);
			var farming = new DbIndustry("Landwirtschaft");
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
			IndustryLibrary.Add(farming);
			var fishing = new DbIndustry("Fishfang");
				fishing.AddGeneratedProduct(Repository.GetProductByName("Schalentiere"));
				fishing.AddGeneratedProduct(Repository.GetProductByName("Calmare"));
				fishing.AddGeneratedProduct(Repository.GetProductByName("Fische"));
				fishing.AddGeneratedProduct(Repository.GetProductByName("Wasser"));
				fishing.AddGeneratedProduct(Repository.GetProductByName("Algen/Seeigel"));
				fishing.AddNeededProduct(Repository.GetProductByName("Stahl"));
				fishing.AddNeededProduct(Repository.GetProductByName("Holz"));
				fishing.AddGeneratedProduct(Repository.GetProductByName("Reis"));
				fishing.AddGeneratedProduct(Repository.GetProductByName("Soya"));
				fishing.AddGeneratedProduct(Repository.GetProductByName("Orangensaft"));
				fishing.AddGeneratedProduct(Repository.GetProductByName("Bacon"));
            IndustryLibrary.Add(fishing);
			var musik = new DbIndustry("Musikinstrumente");
				musik.AddGeneratedProduct(Repository.GetProductByName("Gitarren"));
				musik.AddGeneratedProduct(Repository.GetProductByName("Holzblasinstrumente"));
				musik.AddGeneratedProduct(Repository.GetProductByName("Blechblasinstrumente"));
				musik.AddGeneratedProduct(Repository.GetProductByName("Streichinstrumente"));
				musik.AddGeneratedProduct(Repository.GetProductByName("Schlagzeug"));
				musik.AddNeededProduct(Repository.GetProductByName("Holz"));
				musik.AddNeededProduct(Repository.GetProductByName("Schalentiere"));
				musik.AddNeededProduct(Repository.GetProductByName("Calmare"));
				musik.AddNeededProduct(Repository.GetProductByName("Fische"));
				musik.AddNeededProduct(Repository.GetProductByName("Wasser"));
				musik.AddNeededProduct(Repository.GetProductByName("Algen/Seeigel"));
            IndustryLibrary.Add(musik);
			var werkzeuge = new DbIndustry("Werkzeuge");
				werkzeuge.AddGeneratedProduct(Repository.GetProductByName("Wasser Evaporatoren"));
				werkzeuge.AddGeneratedProduct(Repository.GetProductByName("Bohrmaschinen"));
				werkzeuge.AddGeneratedProduct(Repository.GetProductByName("Abraumwerkzeuge"));
				werkzeuge.AddGeneratedProduct(Repository.GetProductByName("Anti-Schwerkraft Generator"));
				werkzeuge.AddNeededProduct(Repository.GetProductByName("Reis"));
				werkzeuge.AddNeededProduct(Repository.GetProductByName("Soya"));
				werkzeuge.AddNeededProduct(Repository.GetProductByName("Orangensaft"));
				werkzeuge.AddNeededProduct(Repository.GetProductByName("Bacon"));
            IndustryLibrary.Add(werkzeuge);
		}

		public static DbProductInStock GetRandomProduct()
		{
			Random random = new Random();
			var i = random.Next(0, ProductLibrary.Count - 1);
			return ProductLibrary[i];
		}

		public static DbIndustry GetRandomIndustry()
		{
			Random random = new Random();
			var i = random.Next(0, IndustryLibrary.Count - 1);
			return IndustryLibrary[i];
		}

		public static DbProductInStock GetProductByName(string name)
		{
			return ProductLibrary.SingleOrDefault(x=>x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
		}

        private static void NewProductLibary()
        {
            ProductLibrary = new DbProductsInStock();
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

            ProductLibrary.AddProduct("Transpari-Stahl", .4, 10, 0.2, 710.0);
            ProductLibrary.AddProduct("Stahl", .5, 100, 1.0, 187.0);

            ProductLibrary.AddProduct("Holz", .5, 200, .7, 50);
            ProductLibrary.AddProduct("Kunststoff", .5, 200, .7, 50);
            ProductLibrary.AddProduct("Glas", .5, 300, .7, .05);
            ProductLibrary.AddProduct("Beton", .1, 750, 1, 0.1);
            ProductLibrary.AddProduct("Eisenerz", .5, 200, .7, 50);
            ProductLibrary.AddProduct("Kohle", .5, 300, .7, .05);
            ProductLibrary.AddProduct("Titan", .4, 10, 0.2, 710.0);
            ProductLibrary.AddProduct("Aluminium", .5, 200, 1.0, 114.0);

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

        public static DbProductsInStock GetRandomProducts(int howMany, bool needed)
		{
			var ret = new DbProductsInStock();
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

		public static DbFeatures GetFeatureSet(string[] featureNames )
		{
			var featureSet = new DbFeatures();
			foreach (var featureName in featureNames)
			{
				featureSet.Add(GetFeature(featureName));
			}
			return featureSet;
		}

		public static DbFeature GetFeature(string featureName)
		{
			return Features.FirstOrDefault(x => x.Name.Equals(featureName, StringComparison.InvariantCultureIgnoreCase));
		}

	}
}
