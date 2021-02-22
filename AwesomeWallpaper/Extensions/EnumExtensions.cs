using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace AwesomeWallpaper.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            var attribute = GetSingleAttributeOrNull<DisplayAttribute>(value);
            var name = attribute == null ? String.Empty : attribute.Name;
            return name;
        }

        public static IEnumerable<TEnum> AsEnumerable<TEnum>() where TEnum : IComparable, IConvertible, IFormattable
        {
            var result = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
            return result;
        }

        internal static TAttribute GetSingleAttributeOrNull<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            var attribute = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(TAttribute), false).SingleOrDefault() as TAttribute;
            return attribute;
        }
    }
}
