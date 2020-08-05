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
            const string input = "GetTHEMIDDLEofthis";
            const string validoutput = "THEMIDDLE";
            var output = StringExtract.Mid(input, 4, 12);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void MidTest_OverloadBySign()
        {
            const string input = "GetTHEMIDDLEofthis";
            const string validoutput = "th";
            var output = StringExtract.Mid(input, -4, -2);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void GetFirstNameTest()
        {
            const string input = "Jeff Humphreys";
            const string validoutput = "Jeff";
            var output = StringExtract.GetFirstName(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void GetFirstNameTest2()
        {
            const string input = "Humphreys, Jeff";
            const string validoutput = "Jeff";
            var output = StringExtract.GetFirstName(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void GetFirstNameTest3()
        {
            const string input = "Humphreys, Jeff S.";
            const string validoutput = "Jeff";
            var output = StringExtract.GetFirstName(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void LeftTest()
        {
            const string input = "Humphreys, Jeff S.";
            const string validoutput = "Hum";
#pragma warning disable RCS1196 // Call extension method as instance method.
            var output = StringExtract.Left(input, 3);
#pragma warning restore RCS1196 // Call extension method as instance method.
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void FirstWordTest()
        {
            const string input = "Humphreys, Jeff S.";
            const string validoutput = "Humphreys";
            var output = StringExtract.FirstWordW(input);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void PieceNumberTest()
        {
            const string input = "Humphreys, Jeff S.";
            const string validoutput = "Humphreys";
            var output = StringExtract.PieceNumberX(input, ",", 1);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void LastPieceTest()
        {
            const string input = "Humphreys, Jeff S.";
            const string validoutput = " Jeff S.";
            var output = StringExtract.LastPieceX(input, ",");
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void FirstWordBeforeSTest()
        {
            const string input = "Humphreys, Jeff S.";
            const string validoutput = "Humphreys, ";
            var output = StringExtract.FirstWordBeforeS(input, "Je");
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void FirstWordBeforeAnyCTest()
        {
            const string input = "Humphreys, Jeff S.";
            const string validoutput = "Humphr";
            var output = StringExtract.FirstWordBeforeAnyC(input, "e,.");
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void EverythingAfterXTest()
        {
            const string input = "Humphreys, Jeff S.";
            const string validoutput = " Jeff S.";
            var output = StringExtract.EverythingAfterX(input, ",");
            Assert.AreEqual(expected: validoutput, output);
        }
    }
}