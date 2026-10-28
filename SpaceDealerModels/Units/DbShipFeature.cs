using Serilog;
using SpaceDealerModels.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SpaceDealerModels.Units
{
	public class DbFeatures : ConcurrentDictionary<string, DbFeature>
	{
		public DbFeature GetFeatureByName(FeatureNames name)
		{
			return Values.FirstOrDefault(x => x.Name.Equals(name.ToString()));
		}

        public DbFeature GetFeatureById(string id)
        {
            return Values.FirstOrDefault(x => x.Id.Equals(id));
        }

        public void Add(DbFeature feature)
        {
            if (!TryAdd(feature.Id, feature))
                Log.Error($"Failed to add feature [{feature.Name}]");
        }

        public DbFeatures GetFeatureSet(FeatureNames[] featureNames)
        {
            var featureSet = new DbFeatures();
            foreach (var featureName in featureNames)
            {
                featureSet.Add(GetFeature(featureName));
            }
            return featureSet;
        }

        public DbFeature GetFeature(FeatureNames featureName)
        {
            return Values.FirstOrDefault(x => x.Name.Equals(featureName.ToString(), StringComparison.InvariantCultureIgnoreCase));
        }

    }

    public enum FeatureNames
    {
        ExtraShield,			
        ExtraPhasers,				
        ExtraTorpedos,			
        AdvancedWarp_1,		
        SignalRange_1,			
        SignalRange_2,			
        AdditionalCargoSpace_30,
        AdditionalCargoSpace_80,
        AdditionalProbe		
    }

    public enum FeatureBonusTypes
    {
        AttackBonus,
        DefenceBonus,
        TravelSpeedBonus,
        RangeBonus,
        FreightBonus,
        AddtionalProbe
    }

    public class DbFeature : BaseDbField
	{
		[DbField]
		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

        public FeatureBonusTypes BonusType { get; set; }
        public int BonusValue { get; set; } = 0;
        public double Costs { get; set; } = 1500.0;

        public DbFeature()
		{
		}

		public DbFeature(string id, string name, string description, FeatureBonusTypes bonusType, int bonusValue, double costs)
		{
			Id = id;
			Name = name;
			Description = description;
            BonusType = bonusType;
            BonusValue = bonusValue;
            Costs = costs;
        }


		public override string ToString()
		{
			return base.ToString();
		}

        public static async Task<DbFeature> ReadRecord(SqlDataReader reader)
        {
            var model = new DbFeature();
            var props = typeof(DbFeature).GetProperties();

            foreach (var prop in props)
            {
                if (Attribute.IsDefined(prop, typeof(DbField)))
                {
                    if (!reader.IsDBNull(reader.GetOrdinal(prop.Name)))
                    {
                        var fieldType = GetDbFieldTypeOrDefault(prop, "DbFieldType");

                        switch (fieldType)
                        {
                            case "string":
                                prop.SetValue(model, reader.GetString(reader.GetOrdinal(prop.Name)));
                                break;
                            case "money":
                                prop.SetValue(model, (decimal)reader.GetSqlMoney(reader.GetOrdinal(prop.Name)));
                                break;
                            case "int32":
                                prop.SetValue(model, reader.GetInt32(reader.GetOrdinal(prop.Name)));
                                break;
                            case "int64":
                                prop.SetValue(model, reader.GetInt64(reader.GetOrdinal(prop.Name)));
                                break;
                            case "bool":
                                prop.SetValue(model, reader.GetBoolean(reader.GetOrdinal(prop.Name)));
                                break;
                            case "datetime":
                                prop.SetValue(model, reader.GetDateTime(reader.GetOrdinal(prop.Name)));
                                break;
                            case "xml":
                                prop.SetValue(model, XElement.Parse(reader.GetSqlXml(reader.GetOrdinal(prop.Name)).Value));
                                break;
                            case "guid":
                                prop.SetValue(model, reader.GetGuid(reader.GetOrdinal(prop.Name)));
                                break;
                        }
                    }
                }
            }
            return model;
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}