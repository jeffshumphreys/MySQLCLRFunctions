using Xunit;

namespace MySQLCLRFunctions.Tests
{
    public class StringPivotTests
    {
        [Fact]
        public void MatchesTest()
        {
            Assert.False(true);
        }

        [Fact]
        public void PiecesWithContextTest()
        {
            Assert.False(true);
        }

        [Fact]
        public void PiecesTest()
        {
            var pieces = StringPivot.PiecesX("This is input", "\b");
            Assert.NotNull(pieces);
        }

        [Counts]
        [Fact]
        public void PiecesTestCount()
        {
            var pieces = (string[])StringPivot.PiecesX("This is input", " ");
            Assert.Equal(expected: 3, pieces.Length);
        }

        [Fact]
        public void PiecesWithMatchesTest()
        {
            Assert.False(true);
        }

        [Fact]
        public void GetWordsTest()
        {
            Assert.False(true);
        }

        [Fact]
        public void SplitTest()
        {
            Assert.False(true);
        }

        [Fact]
        public void KeyValuePairsWithMultiValuesTest()
        {
            Assert.False(true);
        }

        [Fact]
        public void CapturesTest()
        {
            Assert.False(true);
        }
    }
}