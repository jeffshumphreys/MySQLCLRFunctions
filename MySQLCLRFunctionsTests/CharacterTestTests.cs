using Xunit;
using static MySQLCLRFunctions.CharacterTest;

namespace MySQLCLRFunctions.Tests
{
    public class CharacterTestTests
    {
        [Fact()]
        public void NotInXTest()
        {
            const char input = 'a';
            const string pattern = "[Adb]";
            bool? output = NotInX(input, pattern);
            Assert.True(output);
        }
    }
}