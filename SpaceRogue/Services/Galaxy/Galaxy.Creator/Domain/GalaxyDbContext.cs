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
		public const string DbFileName = "c:\\temp\\SpaceRogue.db";
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite($"Data Source={DbFileName}");
			base.OnConfiguring(optionsBuilder);
		}

		public DbSet<Planet> Planets { get; set; }
		public DbSet<MarketPlace> MarketPlaces { get; set; }
		public DbSet<CatalogItem> CatalogItems { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Player> Players { get; set; }
		public DbSet<ProductGroup> ProductGroups { get; set; }

		internal void FirstOrDefault()
		{
			throw new NotImplementedException();
		}

		public DbSet<Ship> Ships { get; set; }
		public DbSet<Feature> Features { get; set; }
		
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Planet>().HasKey(p => p.ID);
			modelBuilder.Entity<Planet>().HasOne(p => p.Market);

			modelBuilder.Entity<MarketPlace>().HasKey(c => c.ID);
			modelBuilder.Entity<MarketPlace>().HasOne(c => c.Planet);
			modelBuilder.Entity<MarketPlace>().HasMany(c => c.ProductDemands);
			modelBuilder.Entity<MarketPlace>().HasMany(c => c.ProductOfferings);

			modelBuilder.Entity<CatalogItem>().HasKey(c => c.ID);
			modelBuilder.Entity<CatalogItem>().HasOne(c => c.Product);
			modelBuilder.Entity<CatalogItem>().HasOne(c => c.Market);

			modelBuilder.Entity<ProductGroup>().HasKey(c => c.ID);
			
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


			var earthOfferings = new List<CatalogItem>();
			var earthDemands = new List<CatalogItem>();

			var market = new MarketPlace(earthOfferings, earthDemands);

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

			base.OnModelCreating(modelBuilder);
		}

		
	}
}
