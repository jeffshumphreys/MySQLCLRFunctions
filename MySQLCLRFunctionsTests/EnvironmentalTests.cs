using Xunit;
using static MySQLCLRFunctions.Environmental;

namespace MySQLCLRFunctions.Tests
{
    public class EnvironmentalTests
    {
        [Fact()]
        public void BeepStandardTest()
        {
            const string input = "input";
            const string validoutput = input + "not implemented";
            var output = "not implemented yet";
            Assert.Equal(expected: validoutput, output);
        }

        [Fact()]
        public void BeepTest()
        {
            const string input = "input";
            const string validoutput = input + "not implemented";
            var output = "not implemented yet";
            Assert.Equal(expected: validoutput, output);
        }
    }
}