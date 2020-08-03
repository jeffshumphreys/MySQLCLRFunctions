using MySQLCLRFunctions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MySQLCLRFunctions.Tests
{
    [TestClass()]
    public class StringMeasureTests
    {
        [TestMethod()]
        public void HowManySTest()
        {
            var input = "This.3.3.3";
            var validoutput = 3;
            var output = StringMeasure.HowManyS(input, ".");
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void MinTest()
        {
            var inputs = new int[] { 1, 2, 3 };
            var validoutput = 1;
            var output = StringMeasure.Min(inputs);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void MinOverTest()
        {
            var inputs = new int[] { 0, -1, 4 };
            var validoutput = 0;
            var output = StringMeasure.MinOver(0, inputs);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void MaxTest()
        {
            var inputs = new int[] { 0, -1, 4 };
            var validoutput = 4;
            var output = StringMeasure.Max(inputs);
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void HowManyXTest()
        {
            var input = "Print the %s for %d times.";
            var validoutput = 2;
            var output = StringMeasure.HowManyX(input, "(%s|%d)");
            Assert.AreEqual(expected: validoutput, output);
        }
    }
}