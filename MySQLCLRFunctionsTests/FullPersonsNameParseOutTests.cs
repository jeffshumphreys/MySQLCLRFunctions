using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MySQLCLRFunctions.StringCastIntoStruct;

namespace MySQLCLRFunctions.Tests
{
    public class FullPersonsNameParseOutTests : IDisposable
    {
        FullPersonsNameParseOut parser;


        public FullPersonsNameParseOutTests()
        {
            parser = FullPersonsNameParseOut.Parse("Jeff H");
        }

        [Fact]
        public void FirstNameMatches()
        {
             Assert.Equal(actual: parser.GivenName, expected: "Jeff");
        }

        public void Dispose()
        {
            
        }
    }
}
