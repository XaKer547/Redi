using System.ComponentModel.DataAnnotations;

namespace Redi.Application.Helpers
{
    public static class EnumExtensions
    {
        public static string Description(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            if (field == null)
                return null;

            var attributes = field.GetCustomAttributes(typeof(DisplayAttribute), false);

            if (attributes.Length == 0)
                return null;

            return ((DisplayAttribute)attributes[0]).Name;
        }

        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());

            int j = Array.IndexOf(Arr, src) + 1;

            return (Arr.Length == j) ? Arr[0] : Arr[j];
        }
    }
}
