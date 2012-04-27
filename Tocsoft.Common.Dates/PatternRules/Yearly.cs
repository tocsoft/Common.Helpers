using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tocsoft.Common.Dates.PatternRules
{
    public class Yearly : RepeatingDatePattern
    {
        public override bool IsPatternMatch(DateTime date)
        {
            var years =  (date.Year - Start.Year);
            if (years % Offset == 0)
            {
                return (date.Month == Start.Month) && (date.Day == Start.Day);
            }

            return false;
        }
    }
}