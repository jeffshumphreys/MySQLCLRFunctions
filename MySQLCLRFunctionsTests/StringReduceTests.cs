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
        public void TrimBracketingTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void TrimIfStartsWithTest()
        {
            Assert.Fail();
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
        public void RTrimCharTest()
        {
            const string input = "1.1.1.1";
            const string marker = "1xy.";
            const string validoutput = "1.1.1.";
            var output = StringReduce.RTrimAnyC(input, marker);
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