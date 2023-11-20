using System.Collections.Generic;
using System.Linq;

namespace InstallationsMonitor.Logic.Utilities
{
    public class TablesCreatorHelper
    {
        private readonly IList<string> columnNames;
        private readonly IList<IList<string>> allRows;

        public TablesCreatorHelper(
            IEnumerable<string> columnNames, IEnumerable<IEnumerable<string>> rows)
        {
            this.columnNames = columnNames.ToList();

            IEnumerable<IEnumerable<string>> columnNamesRow = new IEnumerable<string>[]
            {
                columnNames,
            };

            this.allRows = columnNamesRow.Concat(rows).Cast<IList<string>>().ToList();

            this.ColumnMaxValues = this.GetColumnMaxValues();
            this.MaxRowLength = this.GetMaxRowLength();
        }

        public IEnumerable<int> ColumnMaxValues;

        public int MaxRowLength;

        private IEnumerable<int> GetColumnMaxValues()
        {
            IList<int> columnMaxValues = new List<int>();

            for (int i = 0; i < this.columnNames.Count; i++)
            {
                columnMaxValues.Add(
                    this.allRows.Select(row => row.ElementAt(i)).Max(value => value.Length));
            }

            return columnMaxValues;
        }

        private int GetMaxRowLength()
        {
            IList<int> columnMaxValues = new List<int>();

            for (int i = 0; i < this.columnNames.Count; i++)
            {
                columnMaxValues.Add(
                    this.allRows.Select(row => row.ElementAt(i)).Max(value => value.Length));
            }

            return 1 + columnMaxValues.Sum() + 3 * this.columnNames.Count;
        }
    }
}