using Xunit;
using static MySQLCLRFunctions.StringFormat;

namespace MySQLCLRFunctions.Tests
{
    public class StringFormatTests
    {
        [Fact]
        public void RPadTest()
        {
            const string input = "Joseph Jr.";
            var output = RPad(input, input.Length + 1);
            Assert.Equal(expected: input + " ", actual: output);
        }

        [Fact]
        public void RPadCTest()
        {
            const string input = "Joseph Jr.";
            var output = RPadC(input, input.Length + 1, 'x');
            Assert.Equal(expected: input + "x", output);
        }

        [Fact]
        public void LPadTest()
        {
            const string input = "Joseph Jr.";
            var output = LPad(input, input.Length + 1);
            Assert.Equal(expected: " " + input, output);
        }

        // Assert.ThrowsException<System.ArgumentException>(() => account.Withdraw(20.0));
        [Fact]
        public void LPadCharTest()
        {
            const string input = "Joseph Jr.";
            var output = LPadC(input, input.Length + 1, 'x');
            Assert.Equal(expected: "x" + input, output);
        }

        [Fact]
        public void TitleTest()
        {
            const string input = "joseph Jr.";
            var output = Title(input);
            Assert.Equal(expected: "Joseph Jr.", actual: output, ignoreCase: false);
        }
    }
}