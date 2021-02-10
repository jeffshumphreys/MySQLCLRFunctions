using System.Collections.Generic;
using System.Linq;
using Xunit;
using static MySQLCLRFunctions.StringPivot;

namespace MySQLCLRFunctions.Tests
{
    public class StringPivotTests
    {
        [Fact]
        public void MatchesXTest()
        {
            const string pattern = @"\((.*?)\)";
            const string input = "203-393-4949 (cell)";
            string validoutput = "cell";
            var output = ((IEnumerable<MatchesRecord>)MatchesX(input, pattern)).ToArray()[0].capturedMatch;
            
            Assert.Equal(expected: validoutput, actual: output);
        }

        [Fact]
        public void PiecesWithContextXTest()
        {

            const string pattern = "[ ]";
            const string input = "input this";
            const string validoutput = "input";
            var unformed_output = PiecesWithContextX(input, pattern);
            Assert.IsNotType<string>(unformed_output);
            var staged_output = ((IEnumerable<PiecesWithContextRecord>)unformed_output);
            Assert.NotNull(staged_output);
            var arrayed_output = staged_output.ToArray();
            Assert.Equal(expected: 2, actual: arrayed_output.Count());
            var output = arrayed_output[0].piece;
            Assert.Equal(expected: validoutput, actual: output);
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
            const string input = "input";
            const string validoutput = input + "not implemented";
            var output = "not implemented yet";
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void SplitTest()
        {
            const string input = "input";
            const string validoutput = input + "not implemented";
            var output = "not implemented yet";
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void KeyValuePairsWithMultiValuesTest()
        {
            const string input = "input";
            const string validoutput = input + "not implemented";
            var output = "not implemented yet";
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void CapturesTest()
        {
            const string input = "input";
            const string validoutput = input + "not implemented";
            var output = "not implemented yet";
            Assert.Equal(expected: validoutput, output);
        }
    }
}