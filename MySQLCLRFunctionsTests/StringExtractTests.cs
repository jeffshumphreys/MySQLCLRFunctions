using Xunit;
using static MySQLCLRFunctions.StringExtract;

namespace MySQLCLRFunctions.Tests
{
    public class StringExtractTests
    {
        [Fact]
        public void LeftOfTest()
        {
            Assert.False(true);
        }

        [Fact]
        public void LeftOfNthTest()
        {
            Assert.False(true);
        }

        [Fact]
        public void LeftMOfNthTest()
        {
            Assert.False(true);
        }
        [Fact]
        public void LeftOfAnyTest()
        {
            Assert.False(true);
        }

        [Fact]
        public void RightOfTest()
        {
            Assert.False(true);
        }

        [Fact]
        public void RightOfAnyTest()
        {
            Assert.False(true);
        }

        [Fact]
        public void MidTest()
        {
            const string input = "GetTHEMIDDLEofthis";
            const string validoutput = "THEMIDDLE";
            var output = Mid(input, 4, 12);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void MidTest_OverloadBySign()
        {
            const string input = "GetTHEMIDDLEofthis";
            const string validoutput = "th";
            var output = Mid(input, -4, -2);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void MidTest_OverloadBySignButOnlyOnEnd()
        {
            const string input = "[test]";
            const string validoutput = "test";
            var output = Mid(input, 1, -1);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void MIDTest()
        {
            const string input = "GetTHEMIDDLEofthis";
            const string validoutput = "THEMIDDLE";
            var output = input.MID(4, 12);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void GetFirstNameTest()
        {
            const string input = "Jeff Humphreys";
            const string validoutput = "Jeff";
            var output = GetFirstName(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void GetFirstNameTest2()
        {
            const string input = "Humphreys, Jeff";
            const string validoutput = "Jeff";
            var output = GetFirstName(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void GetFirstNameTest3()
        {
            const string input = "Humphreys, Jeff S.";
            const string validoutput = "Jeff";
            var output = GetFirstName(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void LeftTest()
        {
            const string input = "Humphreys, Jeff S.";
            const string validoutput = "Hum";
#pragma warning disable RCS1196 // Call extension method as instance method.
            var output = Left(input, 3);
#pragma warning restore RCS1196 // Call extension method as instance method.
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void FirstWordTest()
        {
            const string input = "Humphreys, Jeff S.";
            const string validoutput = "Humphreys";
            var output = FirstWordW(input);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void PieceNumberTest()
        {
            const string input = "Humphreys, Jeff S.";
            const string validoutput = "Humphreys";
            var output = PieceNumberX(input, ",", 1);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void LastPieceTest()
        {
            const string input = "Humphreys, Jeff S.";
            const string validoutput = " Jeff S.";
            var output = LastPieceX(input, ",");
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void FirstWordBeforeSTest()
        {
            const string input = "Humphreys, Jeff S.";
            const string validoutput = "Humphreys, ";
            var output = FirstWordBeforeS(input, "Je");
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void FirstWordBeforeAnyCTest()
        {
            const string input = "Humphreys, Jeff S.";
            const string validoutput = "Humphr";
            var output = FirstWordBeforeAnyC(input, "e,.");
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void EverythingAfterXTest()
        {
            const string input = "Humphreys, Jeff S.";
            const string validoutput = " Jeff S.";
            var output = EverythingAfterX(input, ",");
            Assert.Equal(expected: validoutput, output);
        }
    }
}