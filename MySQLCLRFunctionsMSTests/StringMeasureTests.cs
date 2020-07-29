using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySQLCLRFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQLCLRFunctionsTests
{
    [TestClass()]
    public class StringMeasureTests
    {
        [TestMethod()]
        public void HowManyTest()
        {
            var input = "This.3.3.3";
            var validoutput = 3;
            var output = StringMeasure.HowMany(input, ".");
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
    }
}