using SpaceDealerModels.Attributes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SpaceDealerModels.Units
{
	public class DbFeatures : List<DbFeature>
	{
		public DbFeature GetFeatureByName(string name)
		{
			return this.FirstOrDefault(x => x.Name.Equals(name));
		}
	}

    public class DbFeature : BaseDbField
	{
		[DbField]
		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int AttackBonus { get; set; }
		public int DefenceBonus{ get; set; }
		public int RangeBonus { get; set; }
		public int SpeedBonus { get; set; }

		public DbFeature()
		{
		}

		public DbFeature(string name, string description, int attackBonus, int defenceBonus, int rangeBonus, int speedBonus)
		{
			Id = Guid.NewGuid().ToString();
			Name = name;
			Description = description;
			AttackBonus= attackBonus;
			RangeBonus = rangeBonus;
			DefenceBonus = defenceBonus;
			SpeedBonus = speedBonus;
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