using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Tocsoft.Common.Dates;
using Tocsoft.Common.Dates.PatternRules;
using Tocsoft.Common.Helpers;

namespace DataTimeDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var rptDate = new RepeatingDate();

            //rptDate.Include.Add(new Yearly()
            //{
            //    Start = new DateTime(2012, 1, 5)
            //});
            //rptDate.Include.Add(new Yearly()
            //{
            //    Start = new DateTime(2012, 1, 5)
            //});

            //var dates = rptDate.DatesAny(new DateTime(2012, 1, 1), new DateTime(2016, 1, 1));
            //foreach (var d in dates)
            //    Console.WriteLine("{0:dd MMM yyyy, dddd}",d);
            //Console.WriteLine("----------");
            //Console.WriteLine("Total : {0}", dates.Count());
            //Console.ReadKey();


            DataTable dt = new DataTable();

            dt.Columns.Add("EmployeeID", Type.GetType("System.String"));
            dt.Columns.Add("OrderID", Type.GetType("System.Int32"));
            dt.Columns.Add("Amount", Type.GetType("System.Decimal"));
            dt.Columns.Add("Cost", Type.GetType("System.Decimal"));
            dt.Columns.Add("Date", Type.GetType("System.String"));
            dt.Rows.Add(new object[] { "Sam", 1, 25.00, 13.00, "01/10/2007" });
            dt.Rows.Add(new object[] { "Sam", 2, 512.00, 1.00, "02/10/2007" });
            dt.Rows.Add(new object[] { "Sam", 3, 512.00, 1.00, "03/10/2007" });
            dt.Rows.Add(new object[] { "Tom", 4, 50.00, 1.00, "04/10/2007" });
            dt.Rows.Add(new object[] { "Tom", 5, 3.00, 7.00, "03/10/2007" });
            dt.Rows.Add(new object[] { "Tom", 6, 78.75, 12.00, "02/10/2007" });
            dt.Rows.Add(new object[] { "Sue", 7, 11.00, 7.00, "01/10/2007" });
            dt.Rows.Add(new object[] { "Sue", 8, 2.50, 66.20, "02/10/2007" });
            dt.Rows.Add(new object[] { "Sue", 9, 2.50, 22.00, "03/10/2007" });
            dt.Rows.Add(new object[] { "Jack", 10, 6.00, 23.00, "02/10/2007" });
            dt.Rows.Add(new object[] { "Jack", 11, 117.00, 199.00, "04/10/2007" });
            dt.Rows.Add(new object[] { "Jack", 12, 13.00, 2.60, "01/10/2007" });
            dt.Rows.Add(new object[] { "Jack", 13, 11.40, 99.80, "03/10/2007" });
            dt.Rows.Add(new object[] { "Phill", 14, 37.00, 2.10, "02/10/2007" });
            dt.Rows.Add(new object[] { "Phill", 15, 65.20, 99.30, "04/10/2007" });
            dt.Rows.Add(new object[] { "Phill", 16, 34.10, 27.00, "02/10/2007" });
            dt.Rows.Add(new object[] { "Phill", 17, 17.00, 959.00, "04/10/2007" });



            var output = dt.Pivot(new[]{"Date"}, new[] {"EmployeeID", "Cost"}, d => d.Select(r => r.Field<decimal>("Amount")).Sum());


            //render data table to screen
            PrintTable(dt);
            Console.WriteLine();

            Console.WriteLine();

            PrintTable(output);

            Console.ReadKey();
        }




        static int tableWidth = 77;
        static void PrintTable(DataTable table) {
            PrintLine();
            PrintRow(table.Columns.OfType<DataColumn>().Select(x => x.ColumnName).ToArray());

            PrintLine();
            foreach (var row in table.Rows.OfType<DataRow>()) {

                PrintRow(row.ItemArray);
                PrintLine();
            }
        }
        static void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }

        static void PrintRow(params object[] columns)
        {
            if (columns.Length > 0)
            {
                int width = (tableWidth - columns.Length) / columns.Length;
                string row = "|";

                foreach (var column in columns)
                {
                    row += AlignCentre((column ?? "").ToString(), width) + "|";
                }

                Console.WriteLine(row);
            }
        }

        static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}