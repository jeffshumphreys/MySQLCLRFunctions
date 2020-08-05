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
    public class StringTransformTests
    {
        [TestMethod()]
        public void ReplaceMatchTest()
        {
            const string input = "ThisIsIt";
            const string validoutput = "Th!s!s!t";
            var output = StringTransform.ReplaceMatchX(input, "[it]", "!");
            Assert.AreEqual(expected: validoutput, output);
        }

        [TestMethod()]
        public void ReplaceRecursiveTest()
        {
            const string input = "This is                a test  of the     emergency  ";
            const string validoutput = "This is a test of the emergency ";
            var output = StringTransform.ReplaceRecursiveS(input, "  ", " ");
            Assert.AreEqual(expected: validoutput, output);
        }
    }
}