using Xunit;
using static MySQLCLRFunctions.AssemblyTools;

namespace MySQLCLRFunctions.Tests
{
    public class AssemblyToolsTests
    {
        [Fact()]
        public void SaveAssemblyTest()
        {
            const string input = "input";
            const string validoutput = input + "not implemented";
            var output = "not implemented yet";
            Assert.Equal(expected: validoutput, output);
        }
    }
}