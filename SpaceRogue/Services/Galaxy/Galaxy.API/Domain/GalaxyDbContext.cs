using Cope.SpaceRogue.Galaxy.API.Model;
using Cope.SpaceRogue.InfraStructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.API.Domain
{
	public class GalaxyDbContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite("Data Source=c:\\temp\\SpaceRouge.db");
			base.OnConfiguring(optionsBuilder);
		}

		public DbSet<Planet> Planets { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			var planets = new Planet[]
			{
				new Planet { PlanetId = Guid.NewGuid(), PosX =0, PosY = 0, PosZ = 0, Description = "Erde", Name = "Erde", 
					MarketPlaceId = Guid.NewGuid() },
				new Planet { PlanetId = Guid.NewGuid(), PosX =0, PosY = 0, PosZ = 1, Description = "Mond", Name = "Mond", 
					MarketPlaceId = Guid.NewGuid() }
			};

			modelBuilder.Entity<Planet>().HasData(planets);

			base.OnModelCreating(modelBuilder);
		}
	}
}
