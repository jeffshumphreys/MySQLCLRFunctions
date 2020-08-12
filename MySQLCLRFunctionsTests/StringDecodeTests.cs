using Xunit;
using static MySQLCLRFunctions.StringDecode;

namespace MySQLCLRFunctions.Tests
{
    public class StringDecodeTests
    {
        [Fact]
        public void RevealNonPrintablesTest()
        {
            Assert.False(true);
        }
    }
}