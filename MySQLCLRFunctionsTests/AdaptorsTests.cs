using System;
using Xunit;
using static MySQLCLRFunctions.Adaptors;

namespace MySQLCLRFunctions.Tests
{
    public class AdaptorsTests
    {
        [Fact()]
        public void VarBin2HexTest()
        {
            const string input = "input";
            const string validoutput = input + "not implemented";
            var output = "not implemented yet";
            Assert.Equal(expected: validoutput, output);
        }

        [Fact()]
        public void ADDateTimeString2DateTimeTest()
        {
            const string input = "210207";
            DateTime validoutput = new DateTime(2021, 2, 7);
            var output = ADDateTimeString2DateTime(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact()]
        public void ToDateTest()
        {
            const string input = "210207";
            DateTime validoutput = new DateTime(2021, 2, 7);
            var output = ToDate(input);
            Assert.Equal(expected: validoutput, output);
        }
    }
}