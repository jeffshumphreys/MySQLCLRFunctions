using Xunit;
using static MySQLCLRFunctions.StringDecode;

namespace MySQLCLRFunctions.Tests
{
    public class StringDecodeTests
    {
        [Fact]
        public void RevealNonPrintablesTest()
        {
            const string input = "input";
            const string validoutput = input + "not implemented";
            var output = "not implemented yet";
            Assert.Equal(expected: validoutput, output);
        }
    }
}