using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceDealerModels.Units
{
	public abstract class BaseUnit : IUnit
	{

		public string Name { get; set; }
		public List<KeyValuePair<string, string>> Properties { get; set; }

		public BaseUnit(string name, List<KeyValuePair<string, string>> properties)
		{
			Name = name;
			Properties = properties;
		}

		public void Init()
		{
			
		}

		public virtual void Update()
		{

		}

	}
}
