﻿using System;

namespace Cope.SpaceRogue.Galaxy.API.Domain.Events
{
	public class CatalogItemTitleChangedEvent
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
	}
}
