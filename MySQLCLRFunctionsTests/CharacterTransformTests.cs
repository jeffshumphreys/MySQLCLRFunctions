using Xunit;
using MySQLCLRFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQLCLRFunctions.Tests
{
    public class CharacterTransformTests
    {
        [Fact]
        [PositiveTest]
        public void ReplaceFirstCTest()
        {
            const string input = "Jeff Humphreys";
            const string validoutput = "Heff Humphreys";
            var output = CharacterTransform.ReplaceFirstC(input, 'H');
            Assert.Equal(expected: validoutput, output);
        }

        [Fact]
        [PositiveTest]
        public void ReplaceLastCTest()
        {
            const string input = "Jeff Humphreys";
            const string validoutput = "Jeff Humphreyz";
            var output = CharacterTransform.ReplaceLastC(input, 'z');
            Assert.Equal(expected: validoutput, output);
        }
    }
}