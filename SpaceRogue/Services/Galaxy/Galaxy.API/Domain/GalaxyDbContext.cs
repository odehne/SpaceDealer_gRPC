using Cope.SpaceRogue.Galaxy.API.Model;
using Cope.SpaceRogue.Galaxy.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace Cope.SpaceRogue.Galaxy.API
{
	public class GalaxyDbContext : DbContext
	{
		public string ConnectionString { get; }

		public GalaxyDbContext(string connectionString)
		{
			ConnectionString = connectionString;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite($"Data Source={ConnectionString}");
			base.OnConfiguring(optionsBuilder);
		}

		public DbSet<Planet> Planets { get; set; }
		public DbSet<MarketPlace> MarketPlaces { get; set; }
		public DbSet<CatalogItem> CatalogItems { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Player> Players { get; set; }
		public DbSet<ProductGroup> ProductGroups { get; set; }
		public DbSet<Ship> Ships { get; set; }
		public DbSet<Feature> Features { get; set; }


		
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Planet>().HasKey(p => p.ID);
			modelBuilder.Entity<Planet>().HasOne(p => p.Market);

			modelBuilder.Entity<MarketPlace>().HasKey(c => c.ID);
			modelBuilder.Entity<MarketPlace>().HasOne(c => c.ProductDemands);
			modelBuilder.Entity<MarketPlace>().HasOne(c => c.ProductOfferings);

			modelBuilder.Entity<CatalogItem>().HasKey(c => c.ID);
			modelBuilder.Entity<CatalogItem>().HasOne(c => c.Product);
			modelBuilder.Entity<CatalogItem>().HasOne(c => c.Market);

			modelBuilder.Entity<ProductGroup>().HasKey(c => c.ID);
			modelBuilder.Entity<ProductGroup>().HasMany(c => c.Products);
			
			modelBuilder.Entity<Product>().HasKey(c => c.ID);
			modelBuilder.Entity<Product>().HasOne(c => c.Group);

			modelBuilder.Entity<Payload>().HasKey(c => c.ID);
			modelBuilder.Entity<Payload>().HasOne(c => c.Product);

			modelBuilder.Entity<Feature>().HasKey(c => c.ID);

			modelBuilder.Entity<Ship>().HasKey(c => c.ID);
			modelBuilder.Entity<Ship>().HasMany(c => c.Features);
			modelBuilder.Entity<Ship>().HasMany(c => c.Cargo);

			modelBuilder.Entity<Player>().HasKey(c => c.ID);
			modelBuilder.Entity<Player>().HasMany(c => c.Fleet);

			var groups = new ProductGroup[]
			{
				new ProductGroup("Baumaterialien"),
				new ProductGroup("Grundnahrungsmittel"),
				new ProductGroup("Waffen"),
				new ProductGroup("Spielzeug")
			};

			modelBuilder.Entity<ProductGroup>().HasData(groups);

			// var earthOfferings = new List<CatalogItem>();
			// var earthDemands = new List<CatalogItem>();

			// var market = new MarketPlace(earthOfferings, earthDemands);

			// modelBuilder.Entity<MarketPlace>().HasData(market);

			// var earth = new Planet { ID = Guid.NewGuid(), PosX = 0, PosY = 0, PosZ = 0, Description = "Erde", Name = "Erde", Market = market };
			// var moon = new Planet { ID = Guid.NewGuid(), PosX = 0, PosY = 0, PosZ = 1, Description = "Mond", Name = "Mond", Market = market };
		
			// var planets = new Planet[]
			// {
			// 	earth,
			// 	moon
			// };

			//modelBuilder.Entity<Planet>().HasData(planets);

			// var player = new Player(Guid.NewGuid(), "Olli", earth, 1000, Player.PlayerTypes.Human);

			base.OnModelCreating(modelBuilder);
		}

		
	}
}
