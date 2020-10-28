using MySQLCLRFunctions;
using Xunit;
using static MySQLCLRFunctions.StringTransformCustomizations;

namespace MySQLCLRFunctions.Tests
{
    public class StringTransformCustomizationsTests
    {
        [Fact()]
        public void RemoveSQLServerNameDelimitersTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void ExpandSQLParameterStringTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void ExpandSQLParameterTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void TrimSQLTest()
        {
            Assert.True(false, "This test needs an implementation");
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