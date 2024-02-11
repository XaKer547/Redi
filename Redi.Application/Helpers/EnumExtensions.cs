using System.ComponentModel;

namespace Redi.Application.Helpers
{
    public static class EnumExtensions
    {
        public static string Description(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            if (field == null)
                return null;

            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length == 0)
                return null;

            return ((DescriptionAttribute)attributes[0]).Description;
        }
    }
}
