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
		public static List<DbIndustry> IndustryLibrary { get; set; }
		public static DbProductsInStock ProductLibrary { get; set; }

		public static List<ProductCategory> ProductCategories { get; set; }
		//public static Planets PlanetLibrary { get; set; }

		public static void Init()
		{
			NewProductLibary();
            NewIndustryLibrary();
            NewSpaceShipNames();
			NewPlanetNames();
			NewPilotNames();
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
			p.Industry = CloneIndustry(GetRandomIndustry(), $"{p.Name}.Industry");
			p.Sector = sector;
			return p;
		}

		/// <summary>Maximale Entfernung (in Sektoren), in der ein Lieferant für einen Bedarf gesucht wird.</summary>
		public const double NeighbourhoodRadius = 30.0;
		/// <summary>Wird im Radius kein Lieferant gefunden, wird der Suchradius schrittweise um diesen Faktor erweitert.</summary>
		public const double NeighbourhoodGrowFactor = 2.0;
		public const int NeighbourhoodMaxGrowSteps = 3;

		private static readonly Random SharedRandom = new Random();

		/// <summary>Erzeugt eine unabhängige Kopie der Industrie, damit Planeten keine Bestände teilen.</summary>
		public static DbIndustry CloneIndustry(DbIndustry template, string name)
		{
			var industry = new DbIndustry(name);
			foreach (var p in template.GeneratedProducts.Where(x => x != null))
				industry.AddGeneratedProduct(CloneProduct(p, GetRandomGeneratedInterest()));
			foreach (var p in template.ProductsNeeded.Where(x => x != null))
				industry.AddNeededProduct(CloneProduct(p, GetRandomNeededInterest()));
			return industry;
		}

		/// <summary>Kopie mit Basispreis und ohne Marge.</summary>
		public static DbProductInStock CloneProduct(DbProductInStock p)
		{
			return new DbProductInStock(p.Name, p.AmountGeneratedPerRound, p.Amount, p.Weight, p.BasePricePerTon)
			{
				Id = p.Id,
				PicturePath = p.PicturePath
			};
		}

		/// <summary>Kopie, bei der die planetenspezifische Marge (in Prozent) auf den Basispreis aufgeschlagen wird.</summary>
		public static DbProductInStock CloneProduct(DbProductInStock p, double interest)
		{
			var clone = CloneProduct(p);
			clone.ApplyInterest(interest);
			return clone;
		}

		/// <summary>Marge für produzierte Waren (Ankauf durch den Spieler, günstiger als der Basispreis).</summary>
		public static double GetRandomGeneratedInterest() => SharedRandom.Next(-25, -5);

		/// <summary>Marge für benötigte Waren (Verkauf durch den Spieler, teurer als der Basispreis).</summary>
		public static double GetRandomNeededInterest() => SharedRandom.Next(15, 35);

		/// <summary>
		/// Ersetzt Bedarfe des Planeten, für die im Umkreis kein Lieferant existiert, durch Produkte,
		/// die dort tatsächlich produziert werden. Gibt die Anzahl ersetzter Bedarfe zurück.
		/// </summary>
		public static int AlignNeedsWithNeighbourhood(DbPlanet planet, IEnumerable<DbPlanet> galaxy)
		{
			var others = galaxy.Where(x => x.Id != planet.Id && x.Industry != null).ToList();
			var needs = planet.Industry.ProductsNeeded;
			var changed = 0;

			for (int i = 0; i < needs.Count; i++)
			{
				if (HasSupplierNearby(planet, needs[i].Name, others))
					continue;

				var candidates = GetProductsSuppliedNearby(planet, others)
					.Where(c => needs.All(n => n.Name != c.Name) && planet.Industry.GeneratedProducts.All(g => g.Name != c.Name))
					.ToList();
				if (candidates.Count == 0)
					continue;

				needs[i] = CloneProduct(candidates[SharedRandom.Next(candidates.Count)], GetRandomNeededInterest());
				changed++;
			}
			return changed;
		}

		private static bool HasSupplierNearby(DbPlanet planet, string productName, List<DbPlanet> others)
		{
			var radius = NeighbourhoodRadius;
			for (int step = 0; step <= NeighbourhoodMaxGrowSteps; step++, radius *= NeighbourhoodGrowFactor)
			{
				if (others.Any(o => DbCoordinates.GetDistanceLength(planet.Sector, o.Sector) <= radius
					&& o.Industry.GeneratedProducts.Any(g => g.Name == productName)))
					return true;
			}
			return false;
		}

		private static List<DbProductInStock> GetProductsSuppliedNearby(DbPlanet planet, List<DbPlanet> others)
		{
			var radius = NeighbourhoodRadius;
			for (int step = 0; step <= NeighbourhoodMaxGrowSteps; step++, radius *= NeighbourhoodGrowFactor)
			{
				var supplied = others
					.Where(o => DbCoordinates.GetDistanceLength(planet.Sector, o.Sector) <= radius)
					.SelectMany(o => o.Industry.GeneratedProducts)
					.GroupBy(g => g.Name)
					.Select(g => g.First())
					.ToList();
				if (supplied.Count > 0)
					return supplied;
			}
			return new List<DbProductInStock>();
		}

		/// <summary>Prüft die Industrie-Bibliothek: jeder Bedarf braucht einen Lieferanten, jedes Produkt einen Abnehmer.</summary>
		public static List<string> ValidateIndustryLibrary()
		{
			var problems = new List<string>();
			var produced = new HashSet<string>(IndustryLibrary.SelectMany(i => i.GeneratedProducts).Where(p => p != null).Select(p => p.Name));
			var needed = new HashSet<string>(IndustryLibrary.SelectMany(i => i.ProductsNeeded).Where(p => p != null).Select(p => p.Name));

			foreach (var industry in IndustryLibrary)
			{
				if (industry.GeneratedProducts.Any(p => p == null) || industry.ProductsNeeded.Any(p => p == null))
					problems.Add($"Industrie '{industry.Name}' enthält ein unbekanntes Produkt (Name nicht in ProductLibrary).");
			}
			foreach (var name in needed.Where(n => !produced.Contains(n)))
				problems.Add($"Produkt '{name}' wird benötigt, aber von keiner Industrie produziert.");
			foreach (var name in produced.Where(n => !needed.Contains(n)))
				problems.Add($"Produkt '{name}' wird produziert, aber von keiner Industrie benötigt.");
			return problems;
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
				weltraumSchrott.AddNeededProduct(Repository.GetProductByName("Kleines Raumschiff Kapazität (30t)"));
				weltraumSchrott.AddNeededProduct(Repository.GetProductByName("Sensor-Einheit"));
				weltraumSchrott.AddNeededProduct(Repository.GetProductByName("Pistolen"));
				weltraumSchrott.AddNeededProduct(Repository.GetProductByName("Phaser"));
				weltraumSchrott.AddNeededProduct(Repository.GetProductByName("Wein"));
				weltraumSchrott.AddNeededProduct(Repository.GetProductByName("Bier"));
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
				moonFactory.AddNeededProduct(Repository.GetProductByName("Tie-Fighter Flügel"));		
				moonFactory.AddNeededProduct(Repository.GetProductByName("Sternen-Zerstörer Triebwerke"));		
				moonFactory.AddNeededProduct(Repository.GetProductByName("Anti-Schwerkraft Generator"));		
				moonFactory.AddNeededProduct(Repository.GetProductByName("Solarpanele"));		
				moonFactory.AddNeededProduct(Repository.GetProductByName("Kunststoff"));		
				moonFactory.AddNeededProduct(Repository.GetProductByName("Torpedos"));		
			IndustryLibrary.Add(moonFactory);

			var farming = new DbIndustry("Landwirtschaft");
				farming.AddGeneratedProduct(Repository.GetProductByName("Kuh-Milch"));
				farming.AddGeneratedProduct(Repository.GetProductByName("Mais"));
				farming.AddGeneratedProduct(Repository.GetProductByName("Rindfleisch"));
				farming.AddGeneratedProduct(Repository.GetProductByName("Reis"));
				farming.AddGeneratedProduct(Repository.GetProductByName("Soya"));
				farming.AddGeneratedProduct(Repository.GetProductByName("Orangensaft"));
				farming.AddGeneratedProduct(Repository.GetProductByName("Bacon"));
				farming.AddGeneratedProduct(Repository.GetProductByName("Weizen"));
				farming.AddGeneratedProduct(Repository.GetProductByName("Holz"));
				farming.AddGeneratedProduct(Repository.GetProductByName("Schweinefleisch"));
				
				farming.AddNeededProduct(Repository.GetProductByName("Gitarren"));
				farming.AddNeededProduct(Repository.GetProductByName("Fische"));
				farming.AddNeededProduct(Repository.GetProductByName("Wasser"));
				farming.AddNeededProduct(Repository.GetProductByName("Bohrmaschinen"));
				farming.AddNeededProduct(Repository.GetProductByName("Wasser Evaporatoren"));
				farming.AddNeededProduct(Repository.GetProductByName("Solarpanele"));
				farming.AddNeededProduct(Repository.GetProductByName("Medizinische Produkte"));
				farming.AddNeededProduct(Repository.GetProductByName("Wasserbüffel"));
			IndustryLibrary.Add(farming);

			var fishing = new DbIndustry("Fischfang");
				fishing.AddGeneratedProduct(Repository.GetProductByName("Schalentiere"));
				fishing.AddGeneratedProduct(Repository.GetProductByName("Calmare"));
				fishing.AddGeneratedProduct(Repository.GetProductByName("Fische"));
				fishing.AddGeneratedProduct(Repository.GetProductByName("Wasser"));
				fishing.AddGeneratedProduct(Repository.GetProductByName("Algen/Seeigel"));
				fishing.AddNeededProduct(Repository.GetProductByName("Stahl"));
				fishing.AddNeededProduct(Repository.GetProductByName("Holz"));
				fishing.AddNeededProduct(Repository.GetProductByName("Mittleres Raumschiff Kapazität (60t)"));
				fishing.AddNeededProduct(Repository.GetProductByName("Schweinefleisch"));
                fishing.AddNeededProduct(Repository.GetProductByName("Soya"));
                fishing.AddNeededProduct(Repository.GetProductByName("Bier"));
				fishing.AddNeededProduct(Repository.GetProductByName("Schnecken"));
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
				musik.AddNeededProduct(Repository.GetProductByName("Rindfleisch"));
				musik.AddNeededProduct(Repository.GetProductByName("Weizen"));
            IndustryLibrary.Add(musik);
			
			var werkzeuge = new DbIndustry("Werkzeuge");
				werkzeuge.AddGeneratedProduct(Repository.GetProductByName("Wasser Evaporatoren"));
				werkzeuge.AddGeneratedProduct(Repository.GetProductByName("Bohrmaschinen"));
				werkzeuge.AddGeneratedProduct(Repository.GetProductByName("Abraumwerkzeuge"));
				werkzeuge.AddGeneratedProduct(Repository.GetProductByName("Anti-Schwerkraft Generator"));
            werkzeuge.AddNeededProduct(Repository.GetProductByName("Holz"));
            werkzeuge.AddNeededProduct(Repository.GetProductByName("Stahl"));
            werkzeuge.AddNeededProduct(Repository.GetProductByName("Reis"));
				werkzeuge.AddNeededProduct(Repository.GetProductByName("Soya"));
				werkzeuge.AddNeededProduct(Repository.GetProductByName("Orangensaft"));
				werkzeuge.AddNeededProduct(Repository.GetProductByName("Bacon"));


            IndustryLibrary.Add(werkzeuge);
			var bergbau = new DbIndustry("Bergbau");
				bergbau.AddGeneratedProduct(Repository.GetProductByName("Eisenerz"));
				bergbau.AddGeneratedProduct(Repository.GetProductByName("Kohle"));
				bergbau.AddGeneratedProduct(Repository.GetProductByName("Braunkohle"));
				bergbau.AddGeneratedProduct(Repository.GetProductByName("Titan"));
				bergbau.AddGeneratedProduct(Repository.GetProductByName("Aluminium"));
				bergbau.AddGeneratedProduct(Repository.GetProductByName("Öl"));
				bergbau.AddGeneratedProduct(Repository.GetProductByName("Erdöl"));
				bergbau.AddNeededProduct(Repository.GetProductByName("Bohrmaschinen"));
				bergbau.AddNeededProduct(Repository.GetProductByName("Abraumwerkzeuge"));
				bergbau.AddNeededProduct(Repository.GetProductByName("Wasser"));
				bergbau.AddNeededProduct(Repository.GetProductByName("Sensor-Einheit"));
				bergbau.AddNeededProduct(Repository.GetProductByName("Beton"));
				bergbau.AddNeededProduct(Repository.GetProductByName("Medizinische Produkte"));
				bergbau.AddNeededProduct(Repository.GetProductByName("Kreuzer (100t) +Bewaffnung"));
			IndustryLibrary.Add(bergbau);

			var schwerindustrie = new DbIndustry("Schwerindustrie");
				schwerindustrie.AddGeneratedProduct(Repository.GetProductByName("Stahl"));
				schwerindustrie.AddGeneratedProduct(Repository.GetProductByName("Transpari-Stahl"));
				schwerindustrie.AddGeneratedProduct(Repository.GetProductByName("Beton"));
				schwerindustrie.AddGeneratedProduct(Repository.GetProductByName("Glas"));
				schwerindustrie.AddGeneratedProduct(Repository.GetProductByName("Kunststoff"));
				schwerindustrie.AddGeneratedProduct(Repository.GetProductByName("Solarpanele"));
				schwerindustrie.AddNeededProduct(Repository.GetProductByName("Eisenerz"));
				schwerindustrie.AddNeededProduct(Repository.GetProductByName("Kohle"));
				schwerindustrie.AddNeededProduct(Repository.GetProductByName("Titan"));
				schwerindustrie.AddNeededProduct(Repository.GetProductByName("Aluminium"));
				schwerindustrie.AddNeededProduct(Repository.GetProductByName("Braunkohle"));
				schwerindustrie.AddNeededProduct(Repository.GetProductByName("Öl"));
            schwerindustrie.AddNeededProduct(Repository.GetProductByName("Erdöl"));
            IndustryLibrary.Add(schwerindustrie);

			var waffen = new DbIndustry("Waffenschmiede");
				waffen.AddGeneratedProduct(Repository.GetProductByName("Pistolen"));
				waffen.AddGeneratedProduct(Repository.GetProductByName("Phaser"));
				waffen.AddGeneratedProduct(Repository.GetProductByName("Torpedos"));

				waffen.AddGeneratedProduct(Repository.GetProductByName("Drohnen"));
				waffen.AddGeneratedProduct(Repository.GetProductByName("Panzer"));
				waffen.AddGeneratedProduct(Repository.GetProductByName("Kanonen"));
	    
		        waffen.AddNeededProduct(Repository.GetProductByName("Stahl"));
				waffen.AddNeededProduct(Repository.GetProductByName("Titan"));
				waffen.AddNeededProduct(Repository.GetProductByName("Cyberkristalle"));
				waffen.AddNeededProduct(Repository.GetProductByName("Kunststoff"));
				waffen.AddGeneratedProduct(Repository.GetProductByName("Glas"));
				waffen.AddNeededProduct(Repository.GetProductByName("Reis"));
				waffen.AddNeededProduct(Repository.GetProductByName("Wasser"));
            IndustryLibrary.Add(waffen);


			var genuss = new DbIndustry("Genussmittel & Medizin");
				genuss.AddGeneratedProduct(Repository.GetProductByName("Wein"));
				genuss.AddGeneratedProduct(Repository.GetProductByName("Bier"));
				genuss.AddGeneratedProduct(Repository.GetProductByName("Schnecken"));
				genuss.AddGeneratedProduct(Repository.GetProductByName("Wasserbüffel"));
				genuss.AddGeneratedProduct(Repository.GetProductByName("Medizinische Produkte"));
				genuss.AddNeededProduct(Repository.GetProductByName("Weizen"));
				genuss.AddNeededProduct(Repository.GetProductByName("Mais"));
				genuss.AddNeededProduct(Repository.GetProductByName("Glas"));
				genuss.AddNeededProduct(Repository.GetProductByName("Wasser"));
					genuss.AddNeededProduct(Repository.GetProductByName("Holzblasinstrumente"));
					genuss.AddNeededProduct(Repository.GetProductByName("Blechblasinstrumente"));
					genuss.AddNeededProduct(Repository.GetProductByName("Streichinstrumente"));
					genuss.AddNeededProduct(Repository.GetProductByName("Schlagzeug"));
					genuss.AddNeededProduct(Repository.GetProductByName("Rindfleisch"));
					genuss.AddNeededProduct(Repository.GetProductByName("Schweinefleisch"));
			IndustryLibrary.Add(genuss);
		}

		public static DbProductInStock GetRandomProduct()
		{
			Random random = new Random();
			var i = random.Next(0, ProductLibrary.Count - 1);
			return ProductLibrary[i];
		}

		public static DbIndustry GetRandomIndustry()
		{
			return IndustryLibrary[SharedRandom.Next(IndustryLibrary.Count)];
		}

		public static DbProductInStock GetProductByName(string name)
		{
			return ProductLibrary.SingleOrDefault(x=>x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
		}

        private static void NewProductLibary()
        {
            ProductLibrary = new DbProductsInStock();

            //Nahrungsmittel
			
            ProductLibrary.AddProduct("Fische", 1, 10, 1, 70.0);
            ProductLibrary.AddProduct("Schalentiere", 1, 10, 1, 95.0);
            ProductLibrary.AddProduct("Calmare", 1, 10, 1, 90.0);
            ProductLibrary.AddProduct("Algen/Seeigel", 1, 10, 1, 40.0);
            ProductLibrary.AddProduct("Schnecken", 1, 1, 1, 100.0);
            ProductLibrary.AddProduct("Wein", 1, 1, 1, 130.0);
            ProductLibrary.AddProduct("Bier", 1, 1, 1, 60.0);
            ProductLibrary.AddProduct("Wasserbüffel", 1, 1, 1, 120.0);
            ProductLibrary.AddProduct("Kuh-Milch", 0.2, 1.0, 1.0, 25.0);
            ProductLibrary.AddProduct("Wasser", 0.2, 1.0, 1.0, 15.0);
            ProductLibrary.AddProduct("Mais", 0.2, 1.0, 1.0, 20.0);
            ProductLibrary.AddProduct("Weizen", 0.2, 1.0, 1.0, 22.0);
            ProductLibrary.AddProduct("Rindfleisch", 0.1, 0.5, 1.0, 110.0);
            ProductLibrary.AddProduct("Schweinefleisch", 0.1, 0.5, 1.0, 75.0);
            ProductLibrary.AddProduct("Reis", 0.1, 0.5, 1.0, 30.0);
            ProductLibrary.AddProduct("Soya", 0.1, 0.5, 1.0, 35.0);
            ProductLibrary.AddProduct("Orangensaft", 0.1, 0.5, 1, 55.0);
            ProductLibrary.AddProduct("Bacon", 0.1, 0.5, 1, 85.0);
            
			//Energie
			ProductLibrary.AddProduct("Öl", 1, 15, 1, 90.0);
			ProductLibrary.AddProduct("Braunkohle", 1, 7, 1, 45.0);
            ProductLibrary.AddProduct("Solarpanele", 0.5, 10, 1, 450.0);
            ProductLibrary.AddProduct("Wasser Evaporatoren", .3, 0, 1, 900.0);
            ProductLibrary.AddProduct("Cyberkristalle", .001, .1, .1, 45000.0);

			//Bodenschätze
            ProductLibrary.AddProduct("Transpari-Stahl", .4, 10, 0.2, 650.0);
            ProductLibrary.AddProduct("Stahl", .5, 100, 1.0, 280.0);
            ProductLibrary.AddProduct("Holz", .5, 200, .7, 60.0);
            ProductLibrary.AddProduct("Kunststoff", .5, 200, .7, 220.0);
            ProductLibrary.AddProduct("Glas", .5, 300, .7, 130.0);
            ProductLibrary.AddProduct("Beton", .1, 750, 1, 80.0);
            ProductLibrary.AddProduct("Eisenerz", .5, 200, .7, 110.0);
            ProductLibrary.AddProduct("Kohle", .5, 300, .7, 70.0);
            ProductLibrary.AddProduct("Titan", .4, 10, 0.2, 900.0);
            ProductLibrary.AddProduct("Aluminium", .5, 200, 1.0, 260.0);
            ProductLibrary.AddProduct("Erdöl", 5, 200, 1.0, 120.0);

			//Sonstiges
            ProductLibrary.AddProduct("Medizinische Produkte", .001, 2, .01, 600.0);
            ProductLibrary.AddProduct("Gitarren", 0.2, 2, 0.05, 420.0);
            ProductLibrary.AddProduct("Holzblasinstrumente", 0.2, 2, 0.05, 480.0);
            ProductLibrary.AddProduct("Blechblasinstrumente", 0.2, 2, 0.05, 520.0);
            ProductLibrary.AddProduct("Streichinstrumente", 0.2, 2, 0.05, 450.0);
            ProductLibrary.AddProduct("Schlagzeug", 0.2, 2, 0.05, 380.0);

			//Werkzeuge
            ProductLibrary.AddProduct("Bohrmaschinen", 0.1, 1, 0.05, 700.0);
            ProductLibrary.AddProduct("Abraumwerkzeuge", 0.1, 1, 10.0, 1800.0);

			
			//Raumschiffe
            ProductLibrary.AddProduct("Kleines Raumschiff Kapazität (30t)", .1, 0, 50.0, 140000.0);
            ProductLibrary.AddProduct("Mittleres Raumschiff Kapazität (60t)", .05, 0, 100.0, 190000.0);
            ProductLibrary.AddProduct("Kreuzer (100t) +Bewaffnung", .02, 0, 150.0, 450000.0);
            ProductLibrary.AddProduct("Sensor-Einheit", .2, 0, 1, 1800.0);
            ProductLibrary.AddProduct("Raumsonde", .2, 10, 1, 900.0);
            ProductLibrary.AddProduct("Shuttle", .4, 20, 1, 12000.0);
            ProductLibrary.AddProduct("Tie-Fighter Flügel", .1, .1, 1, 4000.0);
            ProductLibrary.AddProduct("Sternen-Zerstörer Triebwerke", .3, 0, 700, 60000.0);
            ProductLibrary.AddProduct("Anti-Schwerkraft Generator", .3, 0, 0.5, 15000.0);
            ProductLibrary.AddProduct("Raumfrachter (150t)", .05, 0, 100.0, 320000.0);


			// Waffen
            ProductLibrary.AddProduct("Pistolen", 1, 1, 1, 650.0);
            ProductLibrary.AddProduct("Drohnen", 1, 10, 0.1, 1200.0);
            ProductLibrary.AddProduct("Panzer", 0.1, 10, 5, 3000.0);
            ProductLibrary.AddProduct("Kanonen", 0.1, 10, 3, 2200.0);
            ProductLibrary.AddProduct("Board-Kanone", .2, 0, 1, 3500.0);
            ProductLibrary.AddProduct("Phaser", 1, 1, 1, 1400.0);
            ProductLibrary.AddProduct("Torpedos", 1, 1, 1, 2600.0);



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


	}
}
