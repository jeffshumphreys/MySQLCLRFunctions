using System.Collections.Generic;
using System.Linq;
using Xunit;
using static MySQLCLRFunctions.StringPivot;

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
        public void PiecesXTest()
        {
            var pieces = PiecesX("This is input", "\b");
            Assert.NotNull(pieces);
        }

        [CountTest]
        [Fact]
        public void PiecesXTestCount()
        {
            var pieces = (string[])PiecesX("This is input", " ");
            Assert.Equal(expected: 3, pieces.Length);
        }

        [PositiveTest]
        [Fact]
        public void PiecesXTestFirst()
        {
            var pieces = (string[])PiecesX("This is input", " ");
            Assert.Equal(expected: "This", pieces[0]);
        }

        [Fact]
        public void PiecesWithMatchesXTest()
        {
            var input = "Joan Joyce Julia June Karen Katherine (Kathy) Laura Louise Marilyn Mary Moira Molly Monica Nancy Natalie Norma Pamela (Pam) Patricia (Pat) Paula Ruth Sally Sarah Sophia Susan (Sue) Teresa (Terry) Valerie Veronica Vivian Vickie Wanda Wilma Yvonne";
            IEnumerable<PiecesWithMatchesRecord> pieces = (IEnumerable<PiecesWithMatchesRecord>)PiecesWithMatchesX(input, @"(\([^)]+\)|\w+)");

             foreach (PiecesWithMatchesRecord piece in pieces.Take(1))
                Assert.Equal(expected: "Joan", actual: piece.piece);
        }

        [Fact]
        public void PiecesWithMatchesXTest2()
        {
            var input = "Joan Joyce Julia June Karen Katherine (Kathy) Laura Louise Marilyn Mary Moira Molly Monica Nancy Natalie Norma Pamela (Pam) Patricia (Pat) Paula Ruth Sally Sarah Sophia Susan (Sue) Teresa (Terry) Valerie Veronica Vivian Vickie Wanda Wilma Yvonne";
            IEnumerable<PiecesWithMatchesRecord> pieces = (IEnumerable<PiecesWithMatchesRecord>)PiecesWithMatchesX(input, @"(\([^)]+\)|\w+)");

            foreach (PiecesWithMatchesRecord piece in pieces.Skip(1).Take(1))
                Assert.Equal(expected: "Joyce", actual: piece.piece);
        }

        [Fact]
        public void PiecesWithMatchesXTest3()
        {
            var input = "Joan Joyce Julia June Karen Katherine (Kathy) Laura Louise Marilyn Mary Moira Molly Monica Nancy Natalie Norma Pamela (Pam) Patricia (Pat) Paula Ruth Sally Sarah Sophia Susan (Sue) Teresa (Terry) Valerie Veronica Vivian Vickie Wanda Wilma Yvonne";
            IEnumerable<PiecesWithMatchesRecord> pieces = (IEnumerable<PiecesWithMatchesRecord>)PiecesWithMatchesX(input, @"(\([^)]+\)|\w+)");

            foreach (PiecesWithMatchesRecord piece in pieces.Skip(6).Take(1))
                Assert.Equal(expected: "(Kathy)", actual: piece.piece);
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