using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstallationsMonitor.Utilities
{
    internal class TablesCreator
    {
        private const char ColumnSeparator = '|';
        private const char ColumnNamesRowSeparator = '-';
        private static readonly string NewLine = Environment.NewLine;

        private readonly IList<string> columnNames;
        private readonly IList<IList<string>> rows;

        internal TablesCreator(IEnumerable<string> columnNames)
        {
            this.columnNames = columnNames.ToList();
            this.rows = new List<IList<string>>();
        }

        internal void AddRow(IEnumerable<string> values)
        {
            IList<string> valuesList = values.ToList();

            if (valuesList.Count != this.columnNames.Count)
            {
                throw new InvalidOperationException(
                    $"The {nameof(values)} list have to have the same length as the " +
                    $"{nameof(this.columnNames)} list.");
            }

            this.rows.Add(valuesList);
        }

        internal string Create()
        {
            if (!this.rows.Any())
            {
                throw new InvalidOperationException("Add some rows before creating the table.");
            }

            TablesCreatorHelper tablesCreatorHelper = new TablesCreatorHelper(this.columnNames, this.rows);

            StringBuilder stringBuilder = new StringBuilder();

            AddRowWithValues(stringBuilder, this.columnNames, tablesCreatorHelper.ColumnMaxValues);

            AddColumnNamesSeparatorRow(stringBuilder, tablesCreatorHelper.MaxRowLength);

            foreach (IEnumerable<string> row in this.rows)
            {
                AddRowWithValues(stringBuilder, row, tablesCreatorHelper.ColumnMaxValues);
            }

            string result = stringBuilder.ToString();

            return result.Remove(result.Length - NewLine.Length);
        }

        private static void AddRowWithValues(
            StringBuilder stringBuilder, IEnumerable<string> values, IEnumerable<int> columnMaxValues)
        {
            IList<string> valuesList = values.ToList();
            IList<int> columnMaxValuesList = columnMaxValues.ToList();

            stringBuilder.Append(ColumnSeparator);

            for (int i = 0; i < valuesList.Count; i++)
            {
                stringBuilder.Append(' ');

                for (int j = 0; j < columnMaxValuesList[i] - valuesList[i].Length; j++)
                {
                    stringBuilder.Append(' ');
                }

                stringBuilder.Append(valuesList[i]);

                stringBuilder.Append(' ');
                stringBuilder.Append(ColumnSeparator);
            }

            stringBuilder.Append(NewLine);
        }

        private static void AddColumnNamesSeparatorRow(
            StringBuilder stringBuilder, int maxRowLength)
        {
            for (int i = 0; i < maxRowLength; i++)
            {
                stringBuilder.Append(ColumnNamesRowSeparator);
            }

            stringBuilder.Append(NewLine);
        }
    }
}