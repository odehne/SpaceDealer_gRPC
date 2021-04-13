﻿using Cope.SpaceRogue.Galaxy.API.Domain;
using Cope.SpaceRogue.InfraStructure;
using System;

namespace Cope.SpaceRogue.Galaxy.API.Domain
{
	public class Payload : Entity
	{
		public Guid ID { get; set; }
		public Product Product { get; set; }
		public double Quantity { get; set; }

		protected override void EnsureValidState()
		{
			throw new System.NotImplementedException();
		}
	}
}