using Xunit;
using static MySQLCLRFunctions.CharacterExtract;

namespace MySQLCLRFunctions.Tests
{
    public class CharacterExtractTests
    {
        [Fact()]
        [PositiveTest]
        public void FirstCTest()
        {
            const string input = "~NBK";
            char? validoutput = '~';
            char? output = FirstC(input);
            Assert.Equal(expected: validoutput, actual: output);
        }
    }
}