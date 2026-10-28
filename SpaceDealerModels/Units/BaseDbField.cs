using System.Reflection;

namespace SpaceDealerModels.Units
{
    public abstract class BaseDbField
    {
        public abstract bool IsValid();
        public static string GetDbFieldTypeOrDefault(PropertyInfo prop, string attributeName, string defaultFieldType = "string")
        {
            foreach (CustomAttributeData attribData in prop.GetCustomAttributesData())
            {
                foreach (var attr in attribData.NamedArguments)
                {
                    if (attr.MemberName.Equals(attributeName))
                        return attr.TypedValue.ToString().Trim('\"');

                }

            }
            return defaultFieldType;
        }

    }
}