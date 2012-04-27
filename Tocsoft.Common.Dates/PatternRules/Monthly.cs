using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tocsoft.Common.Dates.PatternRules
{
    public class MonthlyDayOfTheMonth : RepeatingDatePattern
    {
        public override bool IsPatternMatch(DateTime date)
        {
            var month = ((date.Year - Start.Year) * 12) + (date.Month - Start.Month);

            if (month % Offset == 0)
            {
                return date.Day == Start.Day;
            }

            return false;
        }
    }

    public class MonthlyDayOfTheWeek : RepeatingDatePattern
    {
        public int Week(DateTime dt)
        {
            var startOfWeekOfDt = dt.AddDays(-(int)dt.DayOfWeek);
            var startOfWeekOfStart = Start.AddDays(-(int)Start.DayOfWeek);
            int weeks = (int)Math.Floor(startOfWeekOfStart.Subtract(startOfWeekOfDt).TotalDays / 7);
            return weeks;
        }

        public DateTime GetDate(int year, int month)
        {
            var date =  new DateTime(year, month, 1);

            date =  date.AddDays(-(int)date.DayOfWeek);//shift this to the start of the week containing the first of the month (week 1)

            date = date.AddDays((int)Start.DayOfWeek);//shift it to the correct day of the week needed;

            date = date.AddDays(7 * ((int)Math.Ceiling(Start.Day / 7m) -1));//add the correct number of weeks

            if (date < new DateTime(year, month, 1))
            {
                date = date.AddDays(7);
            }
            else if (date >= new DateTime(year, month, 1).AddMonths(1))
            {
                date = date.AddDays(-7);
            }

            return date;
        }

        public override bool IsPatternMatch(DateTime date)
        {
            var month = ((date.Year - Start.Year) * 12) + (date.Month - Start.Month);

            if (month % Offset == 0)
            {
                return date == GetDate(date.Year, date.Month);
            }
            return false;
        }
    }
}