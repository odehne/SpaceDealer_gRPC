using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceDealerModels.Units
{
	public class Players : List<Player>
	{
		public Player GetPlayerByName(string name)
		{
			return this.FirstOrDefault(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
		}

		public override string ToString()
		{
			var ret = "";
			foreach (var player in this)
			{
				ret += player.ToString() + "\n";
			}
			return ret.TrimEnd('\n');
		}
	}
}
