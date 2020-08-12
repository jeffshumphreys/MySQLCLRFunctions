using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;

// https://csharpindepth.com/articles/Singleton
namespace MySQLCLRFunctions.Tests
{
    public class TestSet
    {
        public string TestArea;
        public string TestName;
        public string input;
        public string expected;
        public bool TruePositiveFalseNegative;
    }

    public sealed class _MyTestValuesLoader
    {
        private static readonly Lazy<_MyTestValuesLoader>
            lazy =
            new Lazy<_MyTestValuesLoader>
                (() => new _MyTestValuesLoader());

        public static _MyTestValuesLoader Instance { get { return lazy.Value; } }

        dynamic testSets;

        private _MyTestValuesLoader()
        {
            var inputFile = new FileInfo(@"C:\Users\humphrej2\source\repos\jeffshumphreys\MySQLCLRFunctions\MySQLCLRFunctionsTests\_MyTestValues.json");
            if (!inputFile.Exists)
            {
                inputFile = new FileInfo(@"C:\Users\humphrej2\source\repos\jeffshumphreys\MySQLCLRFunctions\MySQLCLRFunctionsTests\_GenericTestValues.json");
            }

            ;
            testSets = JArray.Parse(File.ReadAllText(inputFile.FullName));
        }

        /* Payload */

        public TestSet[] PingableValidAddresses
        {
            get
            {
                IList<TestSet> testsets = testSets[0].Ping.ValidAddresses.ToObject<IList<TestSet>>();
                TestSet[] testsetsAsArray = testsets.ToArray<TestSet>();
                return testsetsAsArray;
                
            }
        }

        public TestSet[] UnpingableAddresses
        {
            get
            {
                IList<TestSet> testsets = testSets[0].Ping.BadAddresses.ToObject<IList<TestSet>>();
                TestSet[] testsetsAsArray = testsets.ToArray<TestSet>();
                return testsetsAsArray;

            }
        }
    }
}
