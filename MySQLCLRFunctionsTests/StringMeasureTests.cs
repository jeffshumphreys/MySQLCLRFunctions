using MySQLCLRFunctions;
using Xunit;

namespace MySQLCLRFunctions.Tests
{
    public class StringMeasureTests
    {
        [Fact]
        public void HowManySTest()
        {
            const string input = "This.3.3.3";
            const int validoutput = 3;
            var output = StringMeasure.HowManyS(input, ".");
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void MinTest()
        {
            var inputs = new int[] { 1, 2, 3 };
            const int validoutput = 1;
            var output = StringMeasure.Min(inputs);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void MinOverTest()
        {
            int[] inputs = new int[] { 0, -1, 4 };
            const int validoutput = 0;
            var output = StringMeasure.MinOver(0, inputs);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void MaxTest()
        {
            var inputs = new int[] { 0, -1, 4 };
            const int validoutput = 4;
            var output = StringMeasure.Max(inputs);
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        public void HowManyXTest()
        {
            const string input = "Print the %s for %d times.";
            const int validoutput = 2;
            var output = StringMeasure.HowManyX(input, "(%s|%d)");
            Assert.Equal(expected: validoutput, output);
        }
    }
}