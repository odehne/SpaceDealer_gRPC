using System.Text.Json.Serialization;
using System.Xml.Serialization;
using System.Collections.Concurrent;

namespace SpaceDealerModels.Units
{
    public class Probes : ConcurrentDictionary<string, Probe>
	{
        [XmlIgnore]
        [JsonIgnore]
        public DbShip Parent { get; set; }

        public Probes(DbShip parent)
        {
            Parent = parent;
        }
    }
}
