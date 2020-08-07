using Xunit;
using static MySQLCLRFunctions.Tests._MyAssertFunctions;
using MySQLCLRFunctions;
using System;

// TODO: Convert to Xunit: Assert.Throws<InvalidOperationException>(() => operation()); // VS 2008
namespace MySQLCLRFunctions.Tests
{
    public class HumanizationTests
    {
        [Fact]
        public void HumanizeDateTimeDiffTest()
        {
            DateTime input = DateTime.Now;
            var output = Humanization.HumanizeDateTimeDiff(input);
            Assert.Matches(expectedRegexPattern: "[45] milliseconds ago", actualString: output);
        }
    }
}