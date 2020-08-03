using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySQLCLRFunctions;
using System;

/************************************************************************************************************************************************
 * 
 * WARNING: These are impossible to test from inside. They must first be deployed to SQL Server and then tested in SSMS.
 * Possibly a generation script of tests could be pushed out into SQL_Register?
 * 
 ************************************************************************************************************************************************/
namespace MySQLCLRFunctionsTests
{
    [TestClass()]
    public class StringPivotTests
    {
        [TestMethod()]
        public void MatchesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PiecesWithContextTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PiecesTest()
        {
            var pieces = StringPivot.Pieces("This is input", "\b");
            Assert.IsNotNull(pieces);
        }

        [Counts]
        [TestMethod()]
        public void PiecesTestCount()
        {
            var pieces = (string[])StringPivot.Pieces("This is input", " ");
            Assert.AreEqual(expected: 3, pieces.Length);
        }

        [TestMethod()]
        public void PiecesWithMatchesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetWordsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SplitTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void KeyValuePairsWithMultiValuesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CapturesTest()
        {
            Assert.Fail();
        }
    }

}