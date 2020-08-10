using Xunit;
using MySQLCLRFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQLCLRFunctions.Tests
{
    public class StringBuildOutTests
    {
        [Fact]
        public void AppendWithSeparatorTest()
        {
            const string colhdr = "";
            const string col1 = "ProductId";
            const string col2 = "Customer";
            string validoutput2 = "ProductId, Customer";
            string sep = ",";

            var output1 = StringBuildOut.AppendWithSeparator(colhdr, col1, sep);
            var output2 = StringBuildOut.AppendWithSeparator(colhdr, col2, sep);
            Assert.Equal(expected: validoutput2, actual: output2);
        }
    }
}