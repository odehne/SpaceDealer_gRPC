using Cope.SpaceRogue.Galaxy.Creator.API.Domain;
using Cope.SpaceRogue.Galaxy.Creator.Domain;
using Cope.SpaceRogue.InfraStructure;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Galaxy.Creator.Tests
{
	public class PlanetTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void ShouldAddAPlanet()
		{
			var planetId = Guid.NewGuid();
			var marketPlaceId = Guid.NewGuid();
			var offerings = new List<CatalogItem>();
			var demands = new List<CatalogItem>();

			var marketPlace = new MarketPlace(marketPlaceId, offerings, demands);



			Assert.Pass();
		}
	}
}