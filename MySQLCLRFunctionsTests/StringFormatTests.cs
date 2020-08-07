using Xunit;
using MySQLCLRFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQLCLRFunctions.Tests
{
    public class StringFormatTests
    {
        [Fact]
        public void RPadTest()
        {
            const string input = "Joseph Jr.";
            var output = StringFormat.RPad(input, input.Length + 1);
            Assert.Equal(expected: input + " ", actual: output);
        }

        [Fact]
        public void RPadCTest()
        {
            const string input = "Joseph Jr.";
            var output = StringFormat.RPadC(input, input.Length + 1, 'x');
            Assert.Equal(expected: input + "x", output);
        }

        [Fact]
        public void LPadTest()
        {
            const string input = "Joseph Jr.";
            var output = StringFormat.LPad(input, input.Length + 1);
            Assert.Equal(expected: " " + input, output);
        }

        // Assert.ThrowsException<System.ArgumentException>(() => account.Withdraw(20.0));
        [Fact]
        public void LPadCharTest()
        {
            const string input = "Joseph Jr.";
            var output = StringFormat.LPadC(input, input.Length + 1, 'x');
            Assert.Equal(expected: "x" + input, output);
        }

        [Fact]
        public void TitleTest()
        {
            const string input = "joseph Jr.";
            var output = StringFormat.Title(input);
            Assert.Equal(expected: "Joseph Jr.", actual: output, ignoreCase: false);
        }
    }
}