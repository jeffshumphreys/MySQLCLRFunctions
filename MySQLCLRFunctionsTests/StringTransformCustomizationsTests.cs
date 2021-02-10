using MySQLCLRFunctions;
using Xunit;
using static MySQLCLRFunctions.StringTransformCustomizations;

namespace MySQLCLRFunctions.Tests
{
    public class StringTransformCustomizationsTests
    {
        [Theory]
        [InlineData("dbo.[TableName]", "dbo.TableName")]
        public void RemoveSQLServerNameDelimitersTest(string input, string validoutput)
        {
            var output = RemoveSQLServerNameDelimiters(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Theory]
        [InlineData("SELECT * FROM x where TABLE_NAME = ''$1''", 1, "I couldn't believe it", "SELECT * FROM x where TABLE_NAME = 'I couldn''t believe it'")]
        public void ExpandSQLParameterStringTest(string input, int indexno, string expansion, string validoutput)
        {
            var output = ExpandSQLParameterString(input, indexno, expansion);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact()]
        public void ExpandSQLParameterTest()
        {
            const string input = "input";
            const string validoutput = input + "not implemented";
            var output = "not implemented yet";
            Assert.Equal(expected: validoutput, output);
        }

        [Fact()]
        public void TrimSQLTest()
        {
            const string input = "input";
            const string validoutput = input + "not implemented";
            var output = "not implemented yet";
            Assert.Equal(expected: validoutput, output);
        }

        [Theory]
        [InlineData("TableName", "[TableName]")]
        [InlineData("Table Name", "[Table Name]")]
        [InlineData("A.B.C.D", "[A].[B].[C].[D]")]
        [InlineData("A.B.C", "[A].[B].[C]")]
        [InlineData("A.B", "[A].[B]")]
        [InlineData("A..B", "[A]..[B]")]
        [InlineData("ETLSteps.[0001_Jobs_0001_CMR_AJF]", "[ETLSteps].[0001_Jobs_0001_CMR_AJF]")]
        [InlineData("ETLSteps.[0001_Jobs_0001_CMR_AJF", "[ETLSteps].[0001_Jobs_0001_CMR_AJF]")]
        public void CleanUpSQLServerNameDelimitersTest(string input, string validoutput)
        {
            var output = CleanUpSQLServerNameDelimiters(input);
            Assert.Equal(expected: validoutput, output);
        }
    }
}