using Xunit;
using MySQLCLRFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQLCLRFunctions.Tests
{
    public class NetworkTestTests
    {
        [Fact]
        [PositiveTest]
        public void PingTest()
        {
            Assert.True(NetworkTest.Ping("64.68.90.1").IsTrue);
        }
    }
}