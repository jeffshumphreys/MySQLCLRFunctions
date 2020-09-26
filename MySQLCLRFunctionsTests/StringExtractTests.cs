using Xunit;
using static MySQLCLRFunctions.StringExtract;

namespace MySQLCLRFunctions.Tests
{
    public class StringExtractTests
    {
        [Fact]
        public void LeftOfSTest()
        {
            const string input = "GetTHEMIDDLEofthis";
            const string validoutput = "GetTHEMIDDLE";
            var output = LeftOfS(input, "of");
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void LeftOfNthSTest()
        {
            const string input = "I,JJJJ,K";
            const string validoutput = "I,JJJJ";
            var output = LeftOfNthS(input, ",", 2);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void LeftMOfNthSTest()
        {
            const string input = "EDWPROD.UserData.x.y";
            const string validoutput = "EDWPROD.UserData";
            var output = LeftMOfNthS(input, ".", 2, 2); 
            Assert.Equal(expected: validoutput, output);
        }
        [Fact]
        public void LeftOfAnyCTest()
        {
            const string input = "This is a,space";
            const string validoutput = "This";
            var output = LeftOfAnyC(input, " ,");
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void RightOfSTest()
        {
            const string input = "This is a,space";
            const string validoutput = "space";
            var output = RightOfS(input, ",");
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void RightOfAnyCTest()
        {
            const string input = "So what?";
            const string validoutput = "";
            var output = RightOfAnyC(input, ",:#?");
            Assert.Equal(expected: validoutput, output);
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