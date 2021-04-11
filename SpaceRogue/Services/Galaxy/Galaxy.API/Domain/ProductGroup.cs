using Cope.SpaceRogue.Galaxy.API.Domain.SeedWork;
using Cope.SpaceRogue.InfraStructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cope.SpaceRogue.Galaxy.API.Domain
{
	public class ProductGroup : Entity
	{
		[Key]
		public Guid ID { get; set; }

		public string Name { get; set; }

		public virtual ICollection<Product> Products {get; set;}

	
		public ProductGroup()
		{
			
		}


		public ProductGroup(string name)
		{
			ID = Guid.NewGuid();
			Name = name;
		}

		protected override void EnsureValidState()
		{
			var valid = ID != default &&
						!string.IsNullOrEmpty(Name);

			if (!valid)
				throw new InvalidEntityStateException(this, $"Postchecks failed.");
		}
	}
}