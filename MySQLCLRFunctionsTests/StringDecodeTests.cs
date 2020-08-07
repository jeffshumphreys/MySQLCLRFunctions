using Xunit;
using MySQLCLRFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQLCLRFunctions.Tests
{
    public class StringDecodeTests
    {
        [Fact]
        public void RevealNonPrintablesTest()
        {
            Assert.False(true);
        }
    }
}