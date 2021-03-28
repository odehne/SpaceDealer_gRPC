using Cope.SpaceRogue.Galaxy.API.Model;
using Cope.SpaceRogue.Galaxy.Creator.API.Domain;
using Cope.SpaceRogue.Galaxy.Creator.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Galaxy.API.Domain
{
	public class GalaxyDbContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite("Data Source=c:\\temp\\SpaceRogue.db");
			base.OnConfiguring(optionsBuilder);
		}

		public DbSet<Planet> Planets { get; set; }
		public DbSet<MarketPlace> MarketPlaces { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Player> Players { get; set; }
		public DbSet<Ship> Ships { get; set; }
		public DbSet<Feature> Features { get; set; }
		
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Planet>().HasKey(p => p.ID);
			modelBuilder.Entity<Planet>().HasOne(p => p.Market);
			modelBuilder.Entity<MarketPlace>().HasKey(c => c.ID);
			modelBuilder.Entity<MarketPlace>().HasOne(c => c.Planet);
			modelBuilder.Entity<CatalogItem>().HasKey(c => c.ID);
			modelBuilder.Entity<CatalogItem>().HasOne(c => c.Product);
			modelBuilder.Entity<CatalogItem>().HasOne(c => c.Market);


			//modelBuilder.Entity<CatalogItem>().HasKey(c => c.ID);
			//modelBuilder.Entity<CatalogItem>().HasOne(c => c.Product);

			//modelBuilder.Entity<Catalog>().HasKey(c => c.ID);
			//modelBuilder.Entity<Catalog>().HasMany(c => c.CatalogItems);

			//modelBuilder.Entity<Product>().HasKey(c => c.ID);

			//modelBuilder.Entity<Player>().HasKey(c => c.ID);
			//modelBuilder.Entity<Player>().HasMany(c => c.Fleet);

			//modelBuilder.Entity<Ship>().HasKey(c => c.ID);
			//modelBuilder.Entity<Ship>().HasMany(c => c.Features);
			//modelBuilder.Entity<Ship>().HasMany(c => c.Cargo);

			//modelBuilder.Entity<Feature>().HasKey(c => c.ID);

			//modelBuilder.Entity<Payload>().HasKey(c => c.ID);
			//modelBuilder.Entity<Payload>().HasOne(c => c.Product);

			var earthOfferings = new List<CatalogItem>();
			var earthDemands = new List<CatalogItem>();

			var market = new MarketPlace(Guid.NewGuid(), earthOfferings, earthDemands);

			modelBuilder.Entity<MarketPlace>().HasData(market);

			var earth = new Planet { ID = Guid.NewGuid(), PosX = 0, PosY = 0, PosZ = 0, Description = "Erde", Name = "Erde", Market = market };
			var moon = new Planet { ID = Guid.NewGuid(), PosX = 0, PosY = 0, PosZ = 1, Description = "Mond", Name = "Mond", Market = market };
		
			var planets = new Planet[]
			{
				earth,
				moon
			};

			//modelBuilder.Entity<Planet>().HasData(planets);

			var player = new Player(Guid.NewGuid(), "Olli", earth, 1000, Player.PlayerTypes.Human);

			var products = new Product[]
			{
				new Product { ID = Guid.NewGuid(), Name = "Eisen", Description = "Metallverarbeitung", PricePerUnit = 10.0, Rarity = 600.0, SizeInUnits = 1.0 }
			};

			base.OnModelCreating(modelBuilder);
		}
	}
}
