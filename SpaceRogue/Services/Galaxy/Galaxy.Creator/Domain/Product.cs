using Cope.SpaceRogue.Galaxy.Creator.Domain.SeedWork;
using Cope.SpaceRogue.InfraStructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace Cope.SpaceRogue.Galaxy.Creator.Domain
{
	public class Product : Entity
	{
		[Key]
		public Guid ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public double SizeInUnits { get; set; }
		public double Rarity { get; set; }
		public double PricePerUnit { get; set; }

		protected override void EnsureValidState()
		{
			var valid = ID != default && 
					    !string.IsNullOrEmpty(Name) &&
				SizeInUnits >= 0 &&
				Rarity > 0 &&
				PricePerUnit >= 0;

			if (!valid)
				throw new InvalidEntityStateException(this, $"Postchecks failed.");
		}
	}
}