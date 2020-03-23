using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SpaceDealerCoreUi
{
	
	public class Assets : List<Asset>
	{
		public Assets(string path)
		{
			foreach (var fil in Directory.GetFiles(path))
			{
				Add(new Asset(fil));
			}
		}

		public Asset GetRandomAsset()
		{
			var index = Tools.GetRandomNumber(0, Count - 1);
			return this[index];
		}
	}

	public class Asset
	{
		public string Id { get; set; }
		public string Path { get; set; }

		public Asset(string path)
		{
			Id = Guid.NewGuid().ToString();
			Path = path;
		}
	}
}
