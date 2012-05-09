using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tocsoft.Common.Dates;
using Tocsoft.Common.Dates.PatternRules;

namespace DataTimeDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var rptDate = new RepeatingDate();

            rptDate.Include.Add(new Yearly()
            {
                Start = new DateTime(2012, 1, 5)
            });
            rptDate.Include.Add(new Yearly()
            {
                Start = new DateTime(2012, 1, 5)
            });

            var dates = rptDate.DatesAny(new DateTime(2012, 1, 1), new DateTime(2016, 1, 1));
            foreach (var d in dates)
                Console.WriteLine("{0:dd MMM yyyy, dddd}",d);
            Console.WriteLine("----------");
            Console.WriteLine("Total : {0}", dates.Count());
            Console.ReadKey();
        }
    }
}