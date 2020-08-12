using Xunit;
using static MySQLCLRFunctions.StringTest;

namespace MySQLCLRFunctions.Tests
{
    public class StringTestTests
    {
        [Fact]
        public void IsNullOrWhiteSpaceOrEmptyTest()
        {
            const string input = null;
            const bool validoutput = true;
            var output = IsNullOrWhiteSpaceOrEmpty(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsNullOrWhiteSpaceOrEmptyTest2()
        {
            string input = string.Empty;
            const bool validoutput = true;
            var output = IsNullOrWhiteSpaceOrEmpty(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsNullOrWhiteSpaceOrEmptyTest3()
        {
            const string input = " ";
            const bool validoutput = true;
            var output = IsNullOrWhiteSpaceOrEmpty(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsNullOrWhiteSpaceOrEmptyTest4()
        {
            const string input = "\n";
            const bool validoutput = true;
            var output = IsNullOrWhiteSpaceOrEmpty(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsNullOrEmptyTest()
        {
            string input = string.Empty;
            const bool validoutput = true;
            var output = IsNullOrEmpty(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsNullOrEmptyTest2()
        {
            const string input = null;
            const bool validoutput = true;
            var output = IsNullOrWhiteSpaceOrEmpty(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsNullTest()
        {
            const string input = null;
            const bool validoutput = true;
            var output = IsNull(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsEmptyNegativeTest()
        {
            const string input = null;
            const bool validoutput = false;
            var output = IsEmpty(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsEmptyNegativeTest2()
        {
            const string input = "  ";
            const bool validoutput = false;
            var output = IsEmpty(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsEmptyNegativeTest3()
        {
            const string input = "\n";
            const bool validoutput = false;
            var output = IsEmpty(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsEmptyPositiveTest()
        {
            const string input = "";
            const bool validoutput = true;
            var output = IsEmpty(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsEmptyOrWhiteSpaceTest()
        {
            string input = string.Empty;
            const bool validoutput = true;
            var output = IsEmptyOrWhiteSpace(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsNullOrWhiteSpaceTest()
        {
            string input = string.Empty;
            const bool validoutput = false;
            var output = IsNullOrWhiteSpace(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsWhiteSpaceTest()
        {
            string input = string.Empty;
            const bool validoutput = false;
            var output = IsWhiteSpace(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsIP4Test()
        {
            const string input = "1.1.1.1";
            const bool validoutput = true;
            var output = IsIP4(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void IsIP6Test()
        {
            const string input = "1.1.1.1";
            const bool validoutput = false;
            var output = IsIP6(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void StartsWithTestAmbiguous()
        {
            const string input = "1.1.1.1";
            const string marker = "1.1";
            const bool validoutput = true;
            var output = StartsWithS(input, marker);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void StartsWithTest()
        {
            const string input = "1x1.1.1";
            const string marker = "1.1";
            const bool validoutput = false;
            var output = StartsWithS(input, marker);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void EndsWithTest()
        {
            const string input = "1.1.1.1";
            const string marker = "1.1";
            const bool validoutput = true;
            var output = EndsWithS(input, marker);
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
            var output = AnyOfTheseSAreAnyOfThoseS(input, markers, sep: ";");
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void AnyOfTheseSAreAnyOfThoseSTest_Negative()
        {
            const string input = "hi;there;";
            const string markers = "not;here;";
            const bool validoutput = false;
            var output = AnyOfTheseSAreAnyOfThoseS(input, markers, sep: ";");
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void LikeAnyXHitTest()
        {
            const string inputs = "Jeffrey S. Humphrey;Jeff Humprheys";
            const string patterns = "%Humphreys;Humphrey%;JSH;%Jeff%Hum%;Jeff%H;(Jeff|Jeffrey|Jeffry)";
            const bool validoutput = true;
            var output = LikeAnyX(inputs, patterns, inputsep: ";", patternsep: ";");
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void LikeAnyXMissTest()
        {
            const string inputs = "Jffrey S. Humhrey;Jeef Hmprheys";
            const string patterns = "%Humphreys;Humphrey%;JSH;%Jeff%Hum%;Jeff%H;(Jeff|Jeffrey|Jeffry)";
            const bool validoutput = false;
            var output = LikeAnyX(inputs, patterns, inputsep: ";", patternsep: ";");
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void ValidateRegexTest()
        {
            const string pattern = "[a-zA-Z\\&\\_\\#]";
            const bool validoutput = false;
            var output = ValidateRegex(pattern);
            Assert.Equal(expected: validoutput, output);
        }
    }
}