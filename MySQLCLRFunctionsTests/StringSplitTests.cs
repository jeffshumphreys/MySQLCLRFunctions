using Xunit;
using MySQLCLRFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MySQLCLRFunctions.StringSplit;

namespace MySQLCLRFunctions.Tests
{
    public class StringSplitTests
    {
        [Fact()]
        public void SplitIn2CTest()
        {
            string input = "a,b";
            string validoutput = "b";
            (string s1, string s2) = input.SplitIn2OnC(',');
            Assert.Equal(expected: validoutput, actual: s2);
        }

        [Fact()]
        public void SplitTo4ColumnsXTest()
        {
            string input = "(\"{4F2E2C19-372F-40D8-9FA7-9D2138C6997A}\") = \"Cheaters\", \"Cheaters\\Cheaters.ssmssqlproj\", \"{50C59C26-BAD0-448E-88ED-F584CFA91CB0}\"\nEndProject\n";
            string pattern = "\\(\"{(?<folderGUID>.*?)}\"\\) = \"(?<FolderName>.*?)\", \"(?<ProjectPath>.*?)\", \"{(?<ParentFolderId>.*?)}\"";
            var validoutput = "Cheaters";
            IEnumerable<Pieces4Record> pieces = (IEnumerable<Pieces4Record>)SplitTo4ColumnsX(input, pattern);
            Assert.Equal(expected:validoutput, actual:pieces.ToArray()[0].col2);
        }

        [Fact()]
        public void SplitTo8ColumnsXTest()
        {
            string input = "1,2,3,4,5,6,7,8";
            string pattern = "(.*?),(.*?),(.*?),(.*?),(.*?),(.*?),(.*?),(.*?)$";
            var validoutput = "8";
            IEnumerable<Pieces8Record> pieces = (IEnumerable<Pieces8Record>)SplitTo8ColumnsX(input, pattern);
            Assert.Equal(expected: validoutput, actual: pieces.ToArray()[0].col8);
        }
    }
}