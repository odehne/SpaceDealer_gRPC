using System;

namespace Shopping.API.Domain
{
	public class PlanetId : Value<PlanetId>
	{
		public PlanetId(Guid value) => _value = value;
	}
}