using System.Collections.Generic;

namespace SpaceDealerModels.Units
{
	public interface IUnit
	{
		List<KeyValuePair<string, string>> Properties { get; set; }
		string Name { get; set; }
		void Init();
		void Update();
	}
}
