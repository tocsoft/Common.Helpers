using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Tocsoft.Common.Helpers
{
    public static class DataTableExtensions
    {

        const string ColumnNameGroupSeperator = " ~ ";

        private class RowFilter {
            public RowFilter(RowFilter parent, string col, object val): this(col, val){

                Parent = parent;
            }
            public RowFilter(string col, object val) {
                ColumnName = col;
                ColumnValue = val;
            }
            public string ColumnName { get; set; }
            public object ColumnValue { get; set; }

            public override bool Equals(object obj)
            {
                var filter = obj as RowFilter;
                if (filter == null)
                    return false;

                return filter.ColumnName == this.ColumnName
                    &&
                    this.ColumnValue.Equals(filter.ColumnValue)
                    &&
                    this.Parent == filter.Parent
                    ;
            }
            public override int GetHashCode()
            {
                return this.ColumnName.GetHashCode() + this.ColumnValue.GetHashCode() +
                    (Parent == null ? 0 : Parent.GetHashCode());
            }

            public static bool operator ==(RowFilter a, RowFilter b)
            {
                // If both are null, or both are same instance, return true.
                if (System.Object.ReferenceEquals(a, b))
                {
                    return true;
                }

                // If one is null, but not both, return false.
                if (((object)a == null) || ((object)b == null))
                {
                    return false;
                }

                // Return true if the fields match:
                return a.Equals(b);
            }

            public static bool operator !=(RowFilter a, RowFilter b)
            {
                return !(a == b);
            }
            public RowFilter Parent {get;set;}

            public string PivotedName {
                get { 

                    if (Parent != null)
                        return Parent.PivotedName + ColumnNameGroupSeperator + ColumnValue;

                    return ColumnValue.ToString();
                }
            }

            public bool IsMatch(DataRow row) {

                return row[ColumnName].Equals(ColumnValue) && (Parent == null || Parent.IsMatch(row));
            }
            public IEnumerable<object> RowValues() {
                if (Parent != null) {
                    foreach (var v in Parent.RowValues())
                    { 
                        yield return v;
                    }
                }
                yield return ColumnValue;
            }
        }
       

        public static DataTable Pivot(this DataTable source, string[] rowSources, string[] columnSources,
            Func<IEnumerable<DataRow>, object> valueCalculation)
        {
            var output = new DataTable(source.TableName, source.Namespace);

            //grouping columns
            
            var colFilters = columnSources.Skip(1)
                .Aggregate(
                    source.Rows.OfType<DataRow>()
                    .Select(x => new RowFilter(columnSources.First(), x[columnSources.First()]))
                    .Distinct()
                    .ToList()
                , 
                    (filters, src)=> filters.SelectMany(filter=>
                        source.Rows.OfType<DataRow>()
                        .Where(row => filter.IsMatch(row))
                        .Select(x => new RowFilter(filter, src, x[src]))
                        .Distinct()
                        .ToList()
                    ).Distinct().ToList()
                ).Distinct().ToList();


            var rowFilters = rowSources.Skip(1)
                .Aggregate(
                    source.Rows.OfType<DataRow>().Select(x => new RowFilter( rowSources.First(), x[ rowSources.First()]))
                    .Distinct()
                    .ToList()
                , 
                    (filters, src)=> filters.SelectMany(filter=>
                        source.Rows.OfType<DataRow>()
                        .Where(row => filter.IsMatch(row))
                        .Select(x => new RowFilter(filter, src, x[src]))
                        .Distinct()
                        .ToList()
                    ).Distinct().ToList()

                ).Distinct().ToList();


            
            //name each source in order of depth
            foreach (var rowSource in rowSources){
                output.Columns.Add(rowSource);
            }
            var ary = colFilters.ToArray();
            
            var r = ary[2] == ary[1];

            foreach (var col in colFilters)
            {
                output.Columns.Add(col.PivotedName);
            }

            foreach (var filter in rowFilters)
            {
                List<object> rowVals = new List<object>();
                
                rowVals.AddRange(filter.RowValues());
                var rows = source.Rows.OfType<DataRow>()
                    .Where(x => filter.IsMatch(x))
                    .ToList();
                //for each row we now look at each column
                foreach (var col in colFilters)
                {
                    //we now need all the row where the 

                    var matchRows = rows.Where(x => col.IsMatch(x)).ToList();
                    rowVals.Add(valueCalculation(matchRows));

                }

                output.Rows.Add(rowVals.ToArray());
            }

            return output;
        }


        public static DataTable Pivot<T>(this DataTable source, string rowSource, string columnSource, string valueSource,
            Func<IEnumerable<T>, object> valueCalculation)
        {
            var output = new DataTable(source.TableName, source.Namespace);


            return output;
        }

        public static DataTable Pivot<T>(this DataTable source, string rowSource, string columnSource, string valueSource)
        {
            return source.Pivot<T>(rowSource, columnSource, valueSource, x => x.Count());
        }

    }
}
