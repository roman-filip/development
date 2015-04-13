using System;

namespace SolarWindsOnlineTest
{
    public class Task2MyOnlineSolution
    {
        private enum DayOfWeek
        {
            Monday = 0,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
            Sunday
        }

        private enum Month
        {
            January = 0,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December
        }

        public int solution(int Y, string A, string B, string W)
        {
            // write your code in C# 5.0 with .NET 4.5 (Mono)        
            var startWeekIndex = 0;
            var endWeekIndex = 0;
            var weekIndex = 1;
            int dayInMonth = 1;
            Month month = Month.January;
            DayOfWeek dayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), W);
            Month startMonth = (Month)Enum.Parse(typeof(Month), A);
            Month endMonth = (Month)Enum.Parse(typeof(Month), B);

            for (int i = 0; i < 366; i++)
            {
                if (dayOfWeek < DayOfWeek.Sunday)
                {
                    dayOfWeek++;
                }
                else
                {
                    dayOfWeek = DayOfWeek.Monday;
                    weekIndex++;
                }

                if (dayInMonth < DaysInMonth(Y, month))
                {
                    dayInMonth++;
                }
                else
                {
                    dayInMonth = 0;
                    month++;
                }

                if (month == startMonth && dayOfWeek == DayOfWeek.Monday && startWeekIndex == 0)
                {
                    startWeekIndex = weekIndex;
                }

                if (month == endMonth && dayOfWeek == DayOfWeek.Sunday)
                {
                    endWeekIndex = weekIndex;
                }
            }

            return endWeekIndex - startWeekIndex;
        }

        private int DaysInMonth(int year, Month month)
        {
            switch (month)
            {
                case Month.February:
                    return IsLeapYear(year) ? 29 : 28;
                    break;
                case Month.April:
                case Month.June:
                case Month.September:
                case Month.November:
                    return 30;
                default:
                    return 31;
            }
        }

        private bool IsLeapYear(int year)
        {
            return year % 4 == 0;
        }
    }
}
