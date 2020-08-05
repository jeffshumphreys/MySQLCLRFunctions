using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MySQLCLRFunctions.Tests
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
            var pieces = StringPivot.PiecesX("This is input", "\b");
            Assert.IsNotNull(pieces);
        }

        [Counts]
        [TestMethod()]
        public void PiecesTestCount()
        {
            var pieces = (string[])StringPivot.PiecesX("This is input", " ");
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