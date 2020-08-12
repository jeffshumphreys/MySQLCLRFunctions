using Xunit;
using static MySQLCLRFunctions.CharacterTransform;

namespace MySQLCLRFunctions.Tests
{
    public class CharacterTransformTests
    {
        [Fact]
        [PositiveTest]
        public void ReplaceFirstCTest()
        {
            const string input = "Jeff Humphreys";
            const string validoutput = "Heff Humphreys";
            var output = ReplaceFirstC(input, 'H');
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        [PositiveTest]
        public void ReplaceLastCTest()
        {
            const string input = "Jeff Humphreys";
            const string validoutput = "Jeff Humphreyz";
            var output = ReplaceLastC(input, 'z');
            Assert.Equal(expected: validoutput, output);
        }
    }
}