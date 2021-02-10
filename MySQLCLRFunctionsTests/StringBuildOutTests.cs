using Xunit;
using static MySQLCLRFunctions.StringBuildOut;

namespace MySQLCLRFunctions.Tests
{
    public class StringBuildOutTests
    {
        [Fact]
        public void AppendWithSeparatorTest()
        {
            string colhdr = "";
            const string col1 = "ProductId";
            const string col2 = "Customer";
            string validoutput = "ProductId, Customer";
            string sep = ", ";

            colhdr = AppendWithSeparator(colhdr, col1, sep);
            colhdr = AppendWithSeparator(colhdr, col2, sep);
            Assert.Equal(expected: validoutput, actual: colhdr);
        }
    }
}