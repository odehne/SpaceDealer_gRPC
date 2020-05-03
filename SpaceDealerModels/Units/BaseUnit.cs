using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceDealerModels.Units
{
	public abstract class BaseUnit : IUnit
	{
		public string Id { get; set; }
		public string CargoSize { get; set; }
		public string PicturePath { get; set; }
		public string PlayerId { get; set; }
		public string Name { get; set; }
		public List<KeyValuePair<string, string>> Properties { get; set; }

		public BaseUnit()
		{
			Id = Guid.NewGuid().ToString();
		}

		public BaseUnit(string name)
		{
			if(string.IsNullOrEmpty(Id))
				Id = Guid.NewGuid().ToString();
			Name = name;
			PicturePath = "";
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
