using Cope.SpaceRogue.Galaxy.API.Domain.SeedWork;
using Cope.SpaceRogue.InfraStructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace Cope.SpaceRogue.Galaxy.API.Domain
{
	public class Product : Entity
	{
		[Key]
		public Guid ID { get; set; }
		public string Name { get; set; }
		public virtual ProductGroup Group { get; set; }
		public virtual Guid GroupId { get; set; }


		public double SizeInUnits { get; set; }
		public double Rarity { get; set; }
		public double PricePerUnit { get; set; }

		public Product()
		{
		}

		public Product(string name, Guid groupId, double sizeInUnits, double rarity, double pricePerUnit)
		{
			ID = Guid.NewGuid();
			Name = name;
			GroupId = groupId;
			SizeInUnits = sizeInUnits;
			Rarity = rarity;
			PricePerUnit = pricePerUnit;
		}

		protected override void EnsureValidState()
		{
			var valid = ID != default &&
						GroupId != default &&
						!string.IsNullOrEmpty(Name) &&
				SizeInUnits >= 0 &&
				Rarity > 0 &&
				PricePerUnit >= 0;

			if (!valid)
				throw new InvalidEntityStateException(this, $"Postchecks failed.");
		}
	}
}