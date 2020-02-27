using System.Collections.Generic;

namespace SpaceDealer
{
	public class Properties : List<KeyValuePair<string, string>>
	{
		public void AddItem(string key, string value)
		{
			Add(new KeyValuePair<string, string>(key, value));
		}

		public Properties(string key, string value)
		{
			AddItem(key, value);
		}
	}
	
}