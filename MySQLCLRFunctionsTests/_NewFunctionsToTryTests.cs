using Xunit;
using static MySQLCLRFunctions._NewFunctionsToTry;

namespace MySQLCLRFunctions.Tests
{
    public class _NewFunctionsToTryTests
    {
        [Fact()]
        public void fn_clr_LongRunningAdderTest()
        {
            const string input = "input";
            const string validoutput = input + "not implemented";
            var output = "not implemented yet";
            Assert.Equal(expected: validoutput, output);
        }

        [Fact()]
        public void TestSqlCharTest()
        {
            const string input = "input";
            const string validoutput = input + "not implemented";
            var output = "not implemented yet";
            Assert.Equal(expected: validoutput, output);
        }
    }
}