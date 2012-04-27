using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tocsoft.Common.Dates.PatternRules
{
    public class Daily : RepeatingDatePattern
    {
        public override bool IsPatternMatch(DateTime date)
        {
            return (date.Subtract(Start).TotalDays % Offset == 0);
        }
    }
}