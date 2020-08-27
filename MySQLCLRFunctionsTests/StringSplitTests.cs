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
        public void SplitIn2OnCTest()
        {
            Assert.True(false, "This test needs an implementation");
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
    }
}