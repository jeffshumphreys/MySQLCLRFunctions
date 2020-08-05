using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySQLCLRFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQLCLRFunctions.Tests
{
    [TestClass()]
    public class StringReduceTests
    {
        [TestMethod()]
        public void TrimBracketsTest()
        {
            const string input = "[test]";
            const string validoutput = "test";
            var output = StringReduce.TrimBrackets(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void TrimIfStartsWithSTest()
        {
            const string input = "-This is a test";
            const string marker = "-";
            const string validoutput = "This is a test";
            var output = StringReduce.LTrimIfStartsWithS(input, marker);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void TrimEndTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void TrimLeftNTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void BlankOutTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RTrimCTest()
        {
            const string input = "1.2.1.1";
            const string marker = "1xy.";
            const string validoutput = "1.2";
            var output = StringReduce.RTrimAnyC(input, marker); // So it's recursive.
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void RTrimNTest()
        {
            const string input = "1.1.1.1";
            const int howmany = 2;
            const string validoutput = "1.1.1";
            var output = StringReduce.RTrimN(input, howmany);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void RTrimOneTest()
        {
            const string input = "1.1.1.1";
            const string validoutput = "1.1.1.";
            var output = StringReduce.RTrimOne(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void LTrimOneTest()
        {
            const string input = "1.1.1.1";
            const string validoutput = ".1.1.1";
            var output = StringReduce.LTrimOne(input);
            Assert.AreEqual(expected: validoutput, output);
        }
    }
}