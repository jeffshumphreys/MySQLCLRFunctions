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
    public class StringTransformTests
    {
        [TestMethod()]
        public void ReplaceMatchTest()
        {
            var input = "ThisIsIt";
            var validoutput = "Th!s!s!t";
            var output = StringTransform.ReplaceMatch(input, "[it]", "!");
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void ReplaceRecursiveTest()
        {
            var input = "This is                a test  of the     emergency  ";
            var validoutput = "This is a test of the emergency ";
            var output = StringTransform.ReplaceRecursive(input, "  ", " ");
            Assert.AreEqual(expected: validoutput, output);
        }
    }
}