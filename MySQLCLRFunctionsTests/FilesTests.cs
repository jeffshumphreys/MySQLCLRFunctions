using Xunit;
using static MySQLCLRFunctions.Files;

namespace MySQLCLRFunctions.Tests
{
    public class FilesTests
    {
        [Fact()]
        public void TempFilePathTest()
        {
            const string input = "input";
            const string validoutput = input + "not implemented";
            var output = "not implemented yet";
            Assert.Equal(expected: validoutput, output);
        }
    }
}