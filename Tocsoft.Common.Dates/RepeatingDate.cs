using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tocsoft.Common.Dates
{
    public class RepeatingDate
    {
        public RepeatingDate()
        {
            Include = new List<RepeatingDatePattern>();
            Exclude = new List<RepeatingDatePattern>();
        }

        public ICollection<RepeatingDatePattern> Include { get; set; }

        public ICollection<RepeatingDatePattern> Exclude { get; set; }

        /// <summary>
        /// matches all of the include dates and non of the exclude dates
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool IsMatchAll(DateTime date)
        {
            return
                !Include.Where(x => !x.IsMatch(date)).Any()
                &&
                !Exclude.Where(x => x.IsMatch(date)).Any();
        }

        /// <summary>
        /// mathces any on the inclue dates an no of the exclude dates
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool IsMatchAny(DateTime date)
        {
            return
                Include.Where(x => x.IsMatch(date)).Any()
                &&
                !Exclude.Where(x => x.IsMatch(date)).Any();
        }

        /// <summary>
        /// returns all dates that match these rules between 2 dates
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IEnumerable<DateTime> DatesAny(DateTime start, DateTime end)
        {
            var days_in_period = (int)Math.Round(end.Subtract(start).TotalDays);
            return Enumerable.Range(0, days_in_period)
                .Select(x => start.AddDays(x))
                .Where(x => IsMatchAny(x));
        }

        /// <summary>
        /// matches all of the include dates and non of the exclude dates
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public IEnumerable<DateTime> DatesAll(DateTime start, DateTime end)
        {
            var days_in_period = (int)Math.Round(end.Subtract(start).TotalDays);
            return Enumerable.Range(0, days_in_period)
                .Select(x => start.AddDays(x))
                .Where(x => IsMatchAll(x));
        }
    }
}