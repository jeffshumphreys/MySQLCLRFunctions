using Xunit;
using MySQLCLRFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQLCLRFunctions.Tests
{
    public class StringTestTests
    {
        [Fact]
        public void IsNullOrWhiteSpaceOrEmptyTest()
        {
            const string input = null;
            const bool validoutput = true;
            var output = StringTest.IsNullOrWhiteSpaceOrEmpty(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsNullOrWhiteSpaceOrEmptyTest2()
        {
            string input = string.Empty;
            const bool validoutput = true;
            var output = StringTest.IsNullOrWhiteSpaceOrEmpty(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsNullOrWhiteSpaceOrEmptyTest3()
        {
            const string input = " ";
            const bool validoutput = true;
            var output = StringTest.IsNullOrWhiteSpaceOrEmpty(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsNullOrWhiteSpaceOrEmptyTest4()
        {
            const string input = "\n";
            const bool validoutput = true;
            var output = StringTest.IsNullOrWhiteSpaceOrEmpty(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsNullOrEmptyTest()
        {
            string input = string.Empty;
            const bool validoutput = true;
            var output = StringTest.IsNullOrEmpty(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsNullOrEmptyTest2()
        {
            const string input = null;
            const bool validoutput = true;
            var output = StringTest.IsNullOrWhiteSpaceOrEmpty(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsNullTest()
        {
            const string input = null;
            const bool validoutput = true;
            var output = StringTest.IsNull(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsEmptyNegativeTest()
        {
            const string input = null;
            const bool validoutput = false;
            var output = StringTest.IsEmpty(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsEmptyNegativeTest2()
        {
            const string input = "  ";
            const bool validoutput = false;
            var output = StringTest.IsEmpty(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsEmptyNegativeTest3()
        {
            const string input = "\n";
            const bool validoutput = false;
            var output = StringTest.IsEmpty(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsEmptyPositiveTest()
        {
            const string input = "";
            const bool validoutput = true;
            var output = StringTest.IsEmpty(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsEmptyOrWhiteSpaceTest()
        {
            string input = string.Empty;
            const bool validoutput = true;
            var output = StringTest.IsEmptyOrWhiteSpace(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsNullOrWhiteSpaceTest()
        {
            string input = string.Empty;
            const bool validoutput = false;
            var output = StringTest.IsNullOrWhiteSpace(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsWhiteSpaceTest()
        {
            string input = string.Empty;
            const bool validoutput = false;
            var output = StringTest.IsWhiteSpace(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsIP4Test()
        {
            const string input = "1.1.1.1";
            const bool validoutput = true;
            var output = StringTest.IsIP4(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsIP6Test()
        {
            const string input = "1.1.1.1";
            const bool validoutput = false;
            var output = StringTest.IsIP6(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void StartsWithTestAmbiguous()
        {
            const string input = "1.1.1.1";
            const string marker = "1.1";
            const bool validoutput = true;
            var output = StringTest.StartsWithS(input, marker);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void StartsWithTest()
        {
            const string input = "1x1.1.1";
            const string marker = "1.1";
            const bool validoutput = false;
            var output = StringTest.StartsWithS(input, marker);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void EndsWithTest()
        {
            const string input = "1.1.1.1";
            const string marker = "1.1";
            const bool validoutput = true;
            var output = StringTest.EndsWithS(input, marker);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void LegalNameTest()
        {
            //Assert.<NotImplementedException>(() => StringTest.LegalName("H", "x"));
        }

        [Fact]
        public void AnyOfTheseSAreAnyOfThoseSTest()
        {
            const string input = "hi;there;";
            const string markers = "not;there;";
            const bool validoutput = true;
            var output = StringTest.AnyOfTheseSAreAnyOfThoseS(input, markers, sep: ";");
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void AnyOfTheseSAreAnyOfThoseSTest_Negative()
        {
            const string input = "hi;there;";
            const string markers = "not;here;";
            const bool validoutput = false;
            var output = StringTest.AnyOfTheseSAreAnyOfThoseS(input, markers, sep: ";");
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void LikeAnyXHitTest()
        {
            const string inputs = "Jeffrey S. Humphrey;Jeff Humprheys";
            const string patterns = "%Humphreys;Humphrey%;JSH;%Jeff%Hum%;Jeff%H;(Jeff|Jeffrey|Jeffry)";
            const bool validoutput = true;
            var output = StringTest.LikeAnyX(inputs, patterns, inputsep: ";", patternsep: ";");
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void LikeAnyXMissTest()
        {
            const string inputs = "Jffrey S. Humhrey;Jeef Hmprheys";
            const string patterns = "%Humphreys;Humphrey%;JSH;%Jeff%Hum%;Jeff%H;(Jeff|Jeffrey|Jeffry)";
            const bool validoutput = false;
            var output = StringTest.LikeAnyX(inputs, patterns, inputsep: ";", patternsep: ";");
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void ValidateRegexTest()
        {
            const string pattern = "[a-zA-Z\\&\\_\\#]";
            const bool validoutput = false;
            var output = StringTest.ValidateRegex(pattern);
            Assert.Equal(expected: validoutput, output);
        }
    }
}