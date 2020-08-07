using Xunit;
using MySQLCLRFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQLCLRFunctions.Tests
{
    public class StringReduceTests
    {
        [Fact]
        public void TrimBracketsTest()
        {
            const string input = "[test]";
            const string validoutput = "test";
            var output = StringReduce.TrimBrackets(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void TrimIfStartsWithSTest()
        {
            const string input = "-This is a test";
            const string marker = "-";
            const string validoutput = "This is a test";
            var output = StringReduce.LTrimIfStartsWithS(input, marker);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void TrimEndTest()
        {
            Assert.False(true);
        }

        [Fact]
        public void TrimLeftNTest()
        {
            Assert.False(true);
        }

        [Fact]
        public void BlankOutTest()
        {
            Assert.False(true);
        }

        [Fact]
        public void RTrimCTest()
        {
            const string input = "1.2.1.1";
            const string marker = "1xy.";
            const string validoutput = "1.2";
            var output = StringReduce.RTrimAnyC(input, marker); // So it's recursive.
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void RTrimNTest()
        {
            const string input = "1.1.1.1";
            const int howmany = 2;
            const string validoutput = "1.1.1";
            var output = StringReduce.RTrimN(input, howmany);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void RTrimOneTest()
        {
            const string input = "1.1.1.1";
            const string validoutput = "1.1.1.";
            var output = StringReduce.RTrimOne(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void LTrimOneTest()
        {
            const string input = "1.1.1.1";
            const string validoutput = ".1.1.1";
            var output = StringReduce.LTrimOne(input);
            Assert.Equal(expected: validoutput, output);
        }
    }
}