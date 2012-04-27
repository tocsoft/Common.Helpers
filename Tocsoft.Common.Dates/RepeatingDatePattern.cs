using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tocsoft.Common.Dates
{
    public abstract class RepeatingDatePattern
    {
        public RepeatingDatePattern() {
            Offset = 1;
            Start = DateTime.Today;
        }

        public DateTime Start { get; set; }

        public DateTime? End { get; set; }

        public int Offset { get; set; }

        public virtual bool IsMatch(DateTime Date)
        {
            if (Date >= Start)
            {
                if (!End.HasValue || End.Value >= Date)
                {
                    return IsPatternMatch(Date);
                }
            }
            return false;
        }

        public virtual bool IsPatternMatch(DateTime Date)
        {
            return false;
        }
    }
}