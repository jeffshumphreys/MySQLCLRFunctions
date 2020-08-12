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
            Assert.False(true);
            //Assert.True(NetworkTest.Ping(NetworkTestPing).IsTrue);
        }
    }
}