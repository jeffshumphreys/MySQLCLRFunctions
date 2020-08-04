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
            string input = null;
            var validoutput = true;
            var output = StringTest.IsNullOrWhiteSpaceOrEmpty(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void IsNullOrWhiteSpaceOrEmptyTest2()
        {
            string input = string.Empty;
            var validoutput = true;
            var output = StringTest.IsNullOrWhiteSpaceOrEmpty(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void IsNullOrWhiteSpaceOrEmptyTest3()
        {
            string input = " ";
            var validoutput = true;
            var output = StringTest.IsNullOrWhiteSpaceOrEmpty(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void IsNullOrWhiteSpaceOrEmptyTest4()
        {
            string input = "\n";
            var validoutput = true;
            var output = StringTest.IsNullOrWhiteSpaceOrEmpty(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void IsNullOrEmptyTest()
        {
            string input = string.Empty;
            var validoutput = true;
            var output = StringTest.IsNullOrEmpty(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void IsNullOrEmptyTest2()
        {
            string input = null;
            var validoutput = true;
            var output = StringTest.IsNullOrWhiteSpaceOrEmpty(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void IsNullTest()
        {
            string input = null;
            var validoutput = true;
            var output = StringTest.IsNull(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void IsEmptyTest()
        {
            string input = null;
            var validoutput = true;
            var output = StringTest.IsEmpty(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void IsEmptyOrWhiteSpaceTest()
        {
            string input = string.Empty;
            var validoutput = true;
            var output = StringTest.IsEmptyOrWhiteSpace(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void IsNullOrWhiteSpaceTest()
        {
            string input = string.Empty;
            var validoutput = false;
            var output = StringTest.IsNullOrWhiteSpace(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void IsWhiteSpaceTest()
        {
            string input = string.Empty;
            var validoutput = false;
            var output = StringTest.IsWhiteSpace(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void IsIP4Test()
        {
            string input = "1.1.1.1";
            var validoutput = true;
            var output = StringTest.IsIP4(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void IsIP6Test()
        {
            string input = "1.1.1.1";
            var validoutput = false;
            var output = StringTest.IsIP6(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void StartsWithTestAmbiguous()
        {
            string input = "1.1.1.1";
            string marker = "1.1";
            var validoutput = true;
            var output = StringTest.StartsWithS(input, marker);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void StartsWithTest()
        {
            string input = "1x1.1.1";
            string marker = "1.1";
            var validoutput = false;
            var output = StringTest.StartsWithS(input, marker);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void EndsWithTest()
        {
            string input = "1.1.1.1";
            string marker = "1.1";
            var validoutput = true;
            var output = StringTest.EndsWith(input, marker);
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
            var input = "hi;there;";
            var markers = "not;there;";
            var validoutput = true;
            var output = StringTest.AnyOfTheseSAreAnyOfThoseS(input, markers, sep: ";");
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void AnyOfTheseSAreAnyOfThoseSTest_Negative()
        {
            var input = "hi;there;";
            var markers = "not;here;";
            var validoutput = false;
            var output = StringTest.AnyOfTheseSAreAnyOfThoseS(input, markers, sep: ";");
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void LikeAnyTest()
        {
            Assert.Fail();
        }
    }
}