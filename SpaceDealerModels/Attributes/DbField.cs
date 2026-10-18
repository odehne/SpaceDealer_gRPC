using System;

namespace SpaceDealerModels.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class DbField : Attribute
    {
        public string DbFieldName { get; set; }
        public string DbFieldType { get; set; } = "string";


        public DbField(string dbFieldName, string dbFieldType)
        {
            DbFieldName = dbFieldName;
            DbFieldType = dbFieldType;
        }

        public DbField()
        {
        }
    }
}
