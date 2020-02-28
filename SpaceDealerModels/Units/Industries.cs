using System.Collections.Generic;
using System.Linq;

namespace SpaceDealerModels.Units
{
	public class Industries : List<Industry>
	{
		public Planet Parent { get; set; }

		public Industries(Planet parent)
		{
			Parent = parent;
		}

		public Industry AddIndustry(string name, List<KeyValuePair<string, string>> properties)
		{
			var industry = new Industry(name, properties);
			Add(industry);
			return industry;
		}

		public Industry GetIndustrByName(string name)
		{
			return this.FirstOrDefault(x => x.Name.Equals(name));
		}

		public override string ToString()
		{
			var ret = "";
			foreach (var industry in this)
			{
				ret += industry.Name + ",";
			}
			return ret.TrimEnd(',');
		}
	}
}
