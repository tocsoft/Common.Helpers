using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tocsoft.Common.Dates.PatternRules
{
    public class Weekly : RepeatingDatePattern
    {
        public ICollection<DayOfWeek> On { get; set; }

        public override bool IsPatternMatch(DateTime date)
        {
            var week = Math.Floor(date.Subtract(Start).TotalDays / 7);

            if (week % Offset == 0) {
                return On.Contains(date.DayOfWeek);
            }

            return false;
        }
    }
}