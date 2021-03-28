using Cope.SpaceRogue.Galaxy.Creator.API.Domain;
using Cope.SpaceRogue.Galaxy.Creator.Domain.SeedWork;
using Cope.SpaceRogue.InfraStructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cope.SpaceRogue.Galaxy.API.Model
{
	public class Planet : Entity
	{
		public Guid ID { get; set; }

		public virtual Guid MarketPlaceId { get; set; }
		public virtual MarketPlace Market { get; set; }

		public string Name { get; set; }
		public string Description { get; set; }

		public int PosX { get; set; }
		public int PosY { get; set; }
		public int PosZ { get; set; }

		protected override void EnsureValidState()
		{
			var valid = ID != default &&
				Market != null &&
				!string.IsNullOrEmpty(Name) &&
				PosX >= 0 &&
				PosY > 0 &&
				PosZ >= 0;

			if (!valid)
				throw new InvalidEntityStateException(this, $"Postchecks failed.");
		}
	}
}
