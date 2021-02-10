using Xunit;
using static MySQLCLRFunctions.StringReduceCustomizations;

namespace MySQLCLRFunctions.Tests
{
    public class StringReduceCustomizationsTests
    {
        [Fact]
        public void StripDownCherwellDescriptionTest()
        {
            const string input = "input";
            const string validoutput = input + "not implemented";
            var output = "not implemented yet";
            Assert.Equal(expected: validoutput, output);
        }
    }
}