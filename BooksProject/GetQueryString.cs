using System.Collections.Specialized;
using System.Web;

namespace BooksProject
{
    public static class GetQueryString
    {
        public static string ToQueryString(object obj)
        {
            var properties = from property in obj.GetType().GetProperties()
                             let value = property.GetValue(obj)
                             where value != null
                             select $"{property.Name}={GetValueString(value)}";

            return string.Join("&", properties);
        }

        private static string? GetValueString(object value)
        {
            if (value is Enum enumValue)
            {
                return Convert.ToInt32(enumValue).ToString();
            }
            else
            {
                return HttpUtility.UrlEncode(value.ToString());
            }
        }
    }
}
