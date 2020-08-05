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
    public class StringFormatTests
    {
        [TestMethod()]
        //[DataSource(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Projects\MyBank\TestData\AccountsTest.accdb","AddIntegerHelperData")]
        [Timeout(100)]
        public void RPadTest()
        {
            const string input = "Joseph Jr.";
            var output = StringFormat.RPad(input, input.Length + 1);
            Assert.AreEqual(expected: input + " ", actual: output);
        }

        [TestMethod()]
        public void RPadCharTest()
        {
            const string input = "Joseph Jr.";
            var output = StringFormat.RPadChar(input, input.Length + 1, 'x');
            Assert.AreEqual(expected: input + "x", output);
        }

        [TestMethod()]
        public void LPadTest()
        {
            const string input = "Joseph Jr.";
            var output = StringFormat.LPad(input, input.Length + 1);
            Assert.AreEqual(expected: " " + input, output);
        }

        // Assert.ThrowsException<System.ArgumentException>(() => account.Withdraw(20.0));
        [TestMethod()]
        public void LPadCharTest()
        {
            const string input = "Joseph Jr.";
            var output = StringFormat.LPadC(input, input.Length + 1, 'x');
            Assert.AreEqual(expected: "x" + input, output);
        }

        [TestMethod()]
        public void TitleTest()
        {
            const string input = "joseph Jr.";
            var output = StringFormat.Title(input);
            Assert.AreEqual(expected: "Joseph Jr.", actual: output, ignoreCase: false);
        }
    }
}