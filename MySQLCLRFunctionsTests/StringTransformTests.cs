using Xunit;
using static MySQLCLRFunctions.StringTransform;

namespace MySQLCLRFunctions.Tests
{
    public class StringTransformTests
    {
        [Fact]
        public void ReplaceMatchXTest()
        {
            const string input = "ThisIsIt";
            const string validoutput = "Th!sIsI!";
            var output = ReplaceMatchX(input, "[it]", "!");
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void ReplaceRecursiveSTest()
        {
            const string input = "This is                a test  of the     emergency  ";
            const string validoutput = "This is a test of the emergency ";
            var output = ReplaceRecursiveS(input, "  ", " ");
            Assert.Equal(expected: validoutput, output);
        }
    }
}