using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceDealerModels.Units
{
	public abstract class BaseUnit : IUnit
	{

		public string Name { get; set; }
		public List<KeyValuePair<string, string>> Properties { get; set; }

		public BaseUnit()
		{

		}

		public BaseUnit(string name)
		{
			Name = name;
			Properties = new List<KeyValuePair<string, string>>();
		}

		public void Init()
		{
			
		}

		public virtual void Update()
		{

		}

	}
}
