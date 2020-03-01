using System.Collections.Generic;
using System.Linq;

namespace SpaceDealerModels.Units
{
	public class Industries : List<Industry>
	{
		public Planet Parent { get; set; }

		public Industries()
		{

		}

		public Industries(Planet parent)
		{
			Parent = parent;
		}

		public Industry AddIndustry(string name)
		{
			var industry = new Industry(name);
			Add(industry);
			return industry;
		}

		public Industry GetIndustryByName(string name)
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
