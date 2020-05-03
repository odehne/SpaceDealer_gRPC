using SpaceDealer;
using SpaceDealerModels.Units;
using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;

namespace SpaceDealerService.Repos
{
    public class SqlPersistor
	{
        public string DbPath { get; set; }
        public ILogger Logger { get; set; }

        public PlanetsRepository PlanetsRepo { get; set; }
        public PlayersRepository PlayersRepo { get; set; }
        public ShipsRepository ShipsRepo { get; set; }
        public ProductsRepository ProductRepo { get; set; }
        public NeededProductsRepository NeededProductsRepo { get; set; }
        public GeneratedProductsRepository GeneratedProductsRepo { get; set; }
        public DiscoveredPlanetsRepository DiscoveredPlanetsRepo { get; set; }


        public SqlPersistor(ILogger logger, string dbPath)
        {
            Logger = logger;
            DbPath = dbPath;

            PlanetsRepo = new PlanetsRepository(Logger, DbPath);
            PlayersRepo = new PlayersRepository(Logger, DbPath);
            ShipsRepo = new ShipsRepository(Logger, DbPath);
            ProductRepo = new ProductsRepository(Logger, DbPath);
            NeededProductsRepo = new NeededProductsRepository(Logger, DbPath);
            GeneratedProductsRepo = new GeneratedProductsRepository(Logger, DbPath);
            DiscoveredPlanetsRepo = new DiscoveredPlanetsRepository(Logger, DbPath);


            if (!File.Exists(DbPath))
                CreateDatabase();
        }

        public bool SaveGalaxy(Planets galaxy)
        {
            foreach (var planet in galaxy)
            {
                PlanetsRepo.SavePlanet(planet);

                foreach (var np in planet.Industry.ProductsNeeded)
                {
                    ProductRepo.SaveProduct(np);
                    NeededProductsRepo.SaveNeededProduct(planet.Id, np.Id);
                }
                
                foreach (var np in planet.Industry.GeneratedProducts)
                {
                    ProductRepo.SaveProduct(np);
                    GeneratedProductsRepo.SaveGeneratedProduct(planet.Id, np.Id);
                }
            }
            return true;
        }

        public bool SavePlayers(Players players)
        {
            try
            {
                foreach (var player in players)
                {
                    PlayersRepo.SavePlayer(player);
                    foreach (var ship in player.Fleet)
                    {
                        ShipsRepo.SaveShip(ship);
                    }

                    foreach(var planet in player.Galaxy)
                    {
                        DiscoveredPlanetsRepo.SaveDiscoveredPlanet(player.Id, planet.Id);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to persist players {e.Message}.", TraceEventType.Error);
                return false;
            }
         
            return true;
        }

        public void CreateDatabase()
        {
            SQLiteConnection.CreateFile(DbPath);

            var dir = Path.GetDirectoryName(DbPath);
            var fils = Directory.GetFiles(dir, "Create*.sql");
            foreach (var fil in fils)
            {
                var sql = File.ReadAllText(fil);
                CreateTable(sql);
            }
        }

        public void CreateTable(string sql)
        {
            try
            {
                using (var connection = new SQLiteConnection("Data Source=" + DbPath))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand(connection))
                    {
                        // Erstellen der Tabelle, sofern diese noch nicht existiert.
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Failed to create table [" + e.Message + "].");
            }

        }


    }
}
