using System;

namespace TaskManager
{
    public static class CDateTimeExtension
    {
        public static string GetDate(this DateTime date)
        {
            return $"{ GetDateValue(date.Day) }.{ GetDateValue(date.Month) }.{ GetDateValue(date.Year) } { GetDateValue(date.Hour) }:{ GetDateValue(date.Minute) }:{ GetDateValue(date.Second) }";
        }

        public static string GetShortDate(this DateTime date)
        {
            return $"{ GetDateValue(date.Day) }.{ GetDateValue(date.Month) }.{ GetDateValue(date.Year) }";
        }

        private static string GetDateValue(int value)
        {
            return (value < 10) ? $"0{ value }" : value.ToString();
        }

        public static bool IsTheSameDay(this DateTime date, DateTime dateTime)
        {
            return date.Year == dateTime.Year && date.Month == dateTime.Month && date.Day == dateTime.Day;
        }
    }
}
