using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySQLCLRFunctions;

namespace MySQLCLRFunctionsTests
{
    [TestClass()]
    public class StringExtractTests
    {
        [TestMethod()]
        public void LeftOfTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LeftOfNthTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LeftMOfNthTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LeftOfAnyTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RightOfTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RightOfAnyTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void MidTest()
        {
            var input = "GetTHEMIDDLEofthis";
            var validoutput = "THEMIDDLE";
            var output = StringExtract.Mid(input, 4, 12);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void MidTest_OverloadBySign()
        {
            var input = "GetTHEMIDDLEofthis";
            var validoutput = "th";
            var output = StringExtract.Mid(input, -4, -2);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void GetFirstNameTest()
        {
            var input = "Jeff Humphreys";
            var validoutput = "Jeff";
            var output = StringExtract.GetFirstName(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void GetFirstNameTest2()
        {
            var input = "Humphreys, Jeff";
            var validoutput = "Jeff";
            var output = StringExtract.GetFirstName(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void GetFirstNameTest3()
        {
            var input = "Humphreys, Jeff S.";
            var validoutput = "Jeff";
            var output = StringExtract.GetFirstName(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void LeftTest()
        {
            var input = "Humphreys, Jeff S.";
            var validoutput = "Hum";
            var output = StringExtract.Left(input, 3);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void FirstWordTest()
        {
            var input = "Humphreys, Jeff S.";
            var validoutput = "Humphreys";
            var output = StringExtract.FirstWord(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void PieceNumberTest()
        {
            var input = "Humphreys, Jeff S.";
            var validoutput = "Humphreys";
            var output = StringExtract.PieceNumber(input, ",", 1);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void LastPieceTest()
        {
            var input = "Humphreys, Jeff S.";
            var validoutput = " Jeff S.";
            var output = StringExtract.LastPiece(input, ",");
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void FirstWordBeforeSTest()
        {
            var input = "Humphreys, Jeff S.";
            var validoutput = "Humphreys, ";
            var output = StringExtract.FirstWordBeforeS(input, @"Je");
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void FirstWordBeforeAnyCTest()
        {
            var input = "Humphreys, Jeff S.";
            var validoutput = "Humphr";
            var output = StringExtract.FirstWordBeforeAnyC(input, @"e,.");
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void EverythingAfterXTest()
        {
            var input = "Humphreys, Jeff S.";
            var validoutput = " Jeff S.";
            var output = StringExtract.EverythingAfterX(input, @",");
            Assert.AreEqual(expected: validoutput, output);
        }
    }
}