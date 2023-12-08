
namespace DynoTools
{
    public static class DateTimeExtensions
    {
        public static string format = "dd/MM/yyyy HH:mm:ss";
        public static string FromDateTime(this DateTime dateTime)
        {
            return dateTime.ToString(format);
        }

        public static DateTime ToDateTime(this DateTime dateTime, string dateTimeString)
        {
            return DateTime.ParseExact(dateTimeString, format, null);

        }
    }
}
