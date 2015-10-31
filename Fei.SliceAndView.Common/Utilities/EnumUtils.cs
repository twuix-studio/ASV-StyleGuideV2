using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using FEI.Applications.SaintCore.SharedDataTypes;

namespace Fei.SliceAndView.Common.Utilities
{
    public static class EnumUtils
    {
        public static string GetDescription(Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

            // Default description if there is no DescriptionAttribute on the value.
            string description = value.ToString();

            if (fieldInfo != null)
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Length > 0)
                {
                    description = attributes[0].Description;
                }
            }
            return (description);
        }

        public static T GetAttributeOfType<T>(this Enum value) where T : Attribute
        {
            Type type = value.GetType();
            var memInfo = type.GetMember(value.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }

        public static bool IsInternalFlagSet(this Enum value)
        {
            return (value.GetAttributeOfType<InternalValueAttribute>() != null);
        }

        public static IEnumerable<TEnum> GetValues<TEnum>(bool excludeInternal = false)
        {
            List<TEnum> values = new List<TEnum>();

            foreach (TEnum item in typeof(TEnum).GetEnumValues().OfType<TEnum>())
            {
                if (excludeInternal && IsInternalFlagSet((Enum)(object)item))
                {
                    continue;
                }

                values.Add(item);
            }

            return values;
        }

    }
}
