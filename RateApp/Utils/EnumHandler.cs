using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace RateApp.Utils
{
    public static class EnumHandler
    {
        public static string GetDescriptionFromEnumValue(Enum value)
        {
            DisplayAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .SingleOrDefault() as DisplayAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
