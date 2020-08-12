using System.Linq;
using Xunit;
using static MySQLCLRFunctions.NetworkTest;

namespace MySQLCLRFunctions.Tests
{
    public class NetworkTestTests 
    {
        [Fact]
        [PositiveTest]
        public void PingPositiveTestList()
        {
            var testvalues = _MyTestValuesLoader.Instance.PingableValidAddresses as TestSet[];
            
            // Verify all good addresses ping good.  But then we won't know which failed??
            Assert.True(testvalues.Where(v => NetworkTest.Ping(v.input).IsFalse).Count() == 0);
        }
        [Fact]
        [NegativeTest]
        public void PingNegativeTestList()
        {
            var testvalues = _MyTestValuesLoader.Instance.UnpingableAddresses as TestSet[];

            // Verify all bad addresses fail.
            Assert.True(testvalues.Where(v => NetworkTest.Ping(v.input).IsTrue).Count() == 0);
        }
    }
}