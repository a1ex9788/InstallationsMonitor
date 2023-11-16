using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace InstallationsMonitor.Utilities
{
    [TestClass]
    public class TablesCreatorTests
    {
        [TestMethod]
        public void AddRow_RowWithIncorrectLength_ThrowsException()
        {
            // Arrange.
            IEnumerable<string> columnNames = new string[] { "A" };

            TablesCreator tablesCreator = new TablesCreator(columnNames);

            // Act.
            Action action = () => tablesCreator.AddRow(new string[] { "1", "2" });

            // Assert.
            action.Should().Throw<InvalidOperationException>()
                .WithMessage("The values list have to have the same length as the columnNames list.");
        }

        [TestMethod]
        public void Create_RowWithIncorrectLength_ThrowsException()
        {
            // Arrange.
            IEnumerable<string> columnNames = new string[] { "A" };

            TablesCreator tablesCreator = new TablesCreator(columnNames);

            // Act.
            Action action = () => tablesCreator.Create();

            // Assert.
            action.Should().Throw<InvalidOperationException>()
                .WithMessage("Add some rows before creating the table.");
        }

        [TestMethod]
        public void Create_TableWithOneRow_ReturnsExpectedString()
        {
            // Arrange.
            IEnumerable<string> columnNames = new string[] { "A", "B", "C" };

            TablesCreator tablesCreator = new TablesCreator(columnNames);

            tablesCreator.AddRow(new string[] { "1", "2", "3" });

            // Act.
            string table = tablesCreator.Create();

            // Assert.
            string expectedOutput =
                $"| A | B | C |{Environment.NewLine}" +
                $"-------------{Environment.NewLine}" +
                $"| 1 | 2 | 3 |";

            table.ToString().Should().Be(expectedOutput);
        }

        [TestMethod]
        public void Create_TableWithMoreThanOneRow_ReturnsExpectedString()
        {
            // Arrange.
            IEnumerable<string> columnNames = new string[] { "A", "B", "C" };

            TablesCreator tablesCreator = new TablesCreator(columnNames);

            tablesCreator.AddRow(new string[] { "1", "2", "3" });
            tablesCreator.AddRow(new string[] { "4", "5", "6" });
            tablesCreator.AddRow(new string[] { "7", "8", "9" });

            // Act.
            string table = tablesCreator.Create();

            // Assert.
            string expectedOutput =
                $"| A | B | C |{Environment.NewLine}" +
                $"-------------{Environment.NewLine}" +
                $"| 1 | 2 | 3 |{Environment.NewLine}" +
                $"| 4 | 5 | 6 |{Environment.NewLine}" +
                $"| 7 | 8 | 9 |";

            table.Should().Be(expectedOutput);
        }

        [TestMethod]
        public void Create_TableWithValuesWithDifferentLengths_ReturnsExpectedString()
        {
            // Arrange.
            IEnumerable<string> columnNames = new string[] { "A", "BB", "CCC", "D", "E" };

            TablesCreator tablesCreator = new TablesCreator(columnNames);

            tablesCreator.AddRow(new string[] { "1", "2", "3", "d", "eeee" });
            tablesCreator.AddRow(new string[] { "4", "5", "6", "ddddd", "e" });
            tablesCreator.AddRow(new string[] { "7", "8", "9", "ddd", "ee" });

            // Act.
            string table = tablesCreator.Create();

            // Assert.
            string expectedOutput =
                $"| A | BB | CCC |     D |    E |{Environment.NewLine}" +
                $"-------------------------------{Environment.NewLine}" +
                $"| 1 |  2 |   3 |     d | eeee |{Environment.NewLine}" +
                $"| 4 |  5 |   6 | ddddd |    e |{Environment.NewLine}" +
                $"| 7 |  8 |   9 |   ddd |   ee |";

            table.Should().Be(expectedOutput);
        }
    }
}