using System;

namespace Infrastructure.Helpers.DateTimeConverter
{
    public static class DateTimeConverter
    {
        public static long ConvertDateTimeToTicks(this DateTime dateTime)
        {
            var dateTimeOffset = new DateTimeOffset(dateTime);
            return dateTimeOffset.ToUnixTimeMilliseconds();
        }
        public static DateTime ConvertTicksToDateTime(this long ticks)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(ticks)
                           .DateTime.ToLocalTime();
        }
    }
}
