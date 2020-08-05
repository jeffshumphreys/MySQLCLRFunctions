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
    public class StringTestTests
    {
        [TestMethod()]
        public void IsNullOrWhiteSpaceOrEmptyTest()
        {
            const string input = null;
            const bool validoutput = true;
            var output = StringTest.IsNullOrWhiteSpaceOrEmpty(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void IsNullOrWhiteSpaceOrEmptyTest2()
        {
            string input = string.Empty;
            const bool validoutput = true;
            var output = StringTest.IsNullOrWhiteSpaceOrEmpty(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void IsNullOrWhiteSpaceOrEmptyTest3()
        {
            const string input = " ";
            const bool validoutput = true;
            var output = StringTest.IsNullOrWhiteSpaceOrEmpty(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void IsNullOrWhiteSpaceOrEmptyTest4()
        {
            const string input = "\n";
            const bool validoutput = true;
            var output = StringTest.IsNullOrWhiteSpaceOrEmpty(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void IsNullOrEmptyTest()
        {
            string input = string.Empty;
            const bool validoutput = true;
            var output = StringTest.IsNullOrEmpty(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void IsNullOrEmptyTest2()
        {
            const string input = null;
            const bool validoutput = true;
            var output = StringTest.IsNullOrWhiteSpaceOrEmpty(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void IsNullTest()
        {
            const string input = null;
            const bool validoutput = true;
            var output = StringTest.IsNull(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void IsEmptyTest()
        {
            const string input = null;
            const bool validoutput = true;
            var output = StringTest.IsEmpty(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void IsEmptyOrWhiteSpaceTest()
        {
            string input = string.Empty;
            const bool validoutput = true;
            var output = StringTest.IsEmptyOrWhiteSpace(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void IsNullOrWhiteSpaceTest()
        {
            string input = string.Empty;
            const bool validoutput = false;
            var output = StringTest.IsNullOrWhiteSpace(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void IsWhiteSpaceTest()
        {
            string input = string.Empty;
            const bool validoutput = false;
            var output = StringTest.IsWhiteSpace(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void IsIP4Test()
        {
            const string input = "1.1.1.1";
            const bool validoutput = true;
            var output = StringTest.IsIP4(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void IsIP6Test()
        {
            const string input = "1.1.1.1";
            const bool validoutput = false;
            var output = StringTest.IsIP6(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void StartsWithTestAmbiguous()
        {
            const string input = "1.1.1.1";
            const string marker = "1.1";
            const bool validoutput = true;
            var output = StringTest.StartsWithS(input, marker);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void StartsWithTest()
        {
            const string input = "1x1.1.1";
            const string marker = "1.1";
            const bool validoutput = false;
            var output = StringTest.StartsWithS(input, marker);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void EndsWithTest()
        {
            const string input = "1.1.1.1";
            const string marker = "1.1";
            const bool validoutput = true;
            var output = StringTest.EndsWithS(input, marker);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void LegalNameTest()
        {
            //Assert.<NotImplementedException>(() => StringTest.LegalName("H", "x"));
        }

        [TestMethod()]
        public void AnyOfTheseSAreAnyOfThoseSTest()
        {
            const string input = "hi;there;";
            const string markers = "not;there;";
            const bool validoutput = true;
            var output = StringTest.AnyOfTheseSAreAnyOfThoseS(input, markers, sep: ";");
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void AnyOfTheseSAreAnyOfThoseSTest_Negative()
        {
            const string input = "hi;there;";
            const string markers = "not;here;";
            const bool validoutput = false;
            var output = StringTest.AnyOfTheseSAreAnyOfThoseS(input, markers, sep: ";");
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void LikeAnyXTest()
        {
            const string inputs = "Jeffrey S. Humphrey;Jeff Humprheys";
            const string patterns = "%Humphreys;Humphrey%;JSH;%Jeff%Hum%;Jeff%H;(Jeff|Jeffrey|Jeffry)";
            const bool validoutput = false;
            var output = StringTest.LikeAnyX(inputs, patterns, inputsep: ";", patternsep: ";");
            Assert.AreEqual(expected: validoutput, output);
        }
    }
}