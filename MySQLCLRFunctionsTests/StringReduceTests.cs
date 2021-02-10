using Xunit;
using static MySQLCLRFunctions.StringReduce;

namespace MySQLCLRFunctions.Tests
{
    public class StringReduceTests
    {
        [Fact]
        public void TrimBracketsTest()
        {
            const string input = "[test]";
            const string validoutput = "test";
            var output = TrimBrackets(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void TrimIfStartsWithSTest()
        {
            const string input = "-This is a test";
            const string marker = "-";
            const string validoutput = "This is a test";
            var output = LTrimIfStartsWithS(input, marker);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void TrimEndTest()
        {
            const string input = "input";
            const string validoutput = input + "not implemented";
            var output = "not implemented yet";
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void TrimLeftNTest()
        {
            const string input = "input";
            const string validoutput = input + "not implemented";
            var output = "not implemented yet";
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void BlankOutTest()
        {
            const string input = "input";
            const string validoutput = input + "not implemented";
            var output = "not implemented yet";
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void RTrimCTest()
        {
            const string input = "1.2.1.1";
            const string marker = "1xy.";
            const string validoutput = "1.2";
            var output = RTrimAnyC(input, marker); // So it's recursive.
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void RTrimNTest()
        {
            const string input = "1.1.1.1";
            const int howmany = 2;
            const string validoutput = "1.1.1";
            var output = RTrimN(input, howmany);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void RTrimOneTest()
        {
            const string input = "1.1.1.1";
            const string validoutput = "1.1.1.";
            var output = RTrimOne(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void LTrimOneTest()
        {
            const string input = "1.1.1.1";
            const string validoutput = ".1.1.1";
            var output = LTrimOne(input);
            Assert.Equal(expected: validoutput, output);
        }
    }
}