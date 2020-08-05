using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQLCLRFunctions
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Readability", "RCS1018:Add accessibility modifiers.", Justification = "<Pending>")]
    class _VERIFY_CSHARP8
    {
        // https://stackoverflow.com/questions/54701377/how-can-i-use-c-sharp-8-with-visual-studio-2017/58190585#58190585
        // https://stackoverflow.com/questions/56651472/does-c-sharp-8-support-the-net-framework/57020770#57020770
        public string NullableString { get; } = "Test";
        static int Test2() => 5;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "<Pending>")]
        static int WriteLinesToFile(IEnumerable<string> lines)
        {
            Debug.Write(Test2());
            var words = new string[]
            {
                            // index from start    index from end
                "The",      // 0                   ^9
                "quick",    // 1                   ^8
                "brown",    // 2                   ^7
                "fox",      // 3                   ^6
                "jumped",   // 4                   ^5
                "over",     // 5                   ^4
                "the",      // 6                   ^3
                "lazy",     // 7                   ^2
                "dog"       // 8                   ^1
            };

            //Console.WriteLine($"The last word is {words[^1]}");
            //var lazyDog = words[^2..^0];
            //var quickBrownFox = words[1..4];
            //var allWords = words[..]; // contains "The" through "dog".
            //var firstPhrase = words[..4]; // contains "The" through "fox"
            //var lastPhrase = words[6..]; // contains "the", "lazy" and "dog"
            //Range phrase = 1..4;
            //var text = words[phrase];

            List<int> numbers = null;
            int? i = null;

            numbers ??= new List<int>();
            numbers.Add(i ??= 17);
            numbers.Add(i ??= 20);

            Console.WriteLine(string.Join(" ", numbers));  // output: 17 17
            Console.WriteLine(i);  // output: 17
            using var file = new System.IO.StreamWriter("WriteLines2.txt");
            // Notice how we declare skippedLines after the using statement.
            int skippedLines = 0;
            foreach (string line in lines)
            {
                if (!line.Contains("Second"))
                {
                    file.WriteLine(line);
                }
                else
                {
                    skippedLines++;
                }
            }
            // Notice how skippedLines is in scope here.

            //Span<Coords<int>> coordinates = stackalloc[]
            //{
            //    new Coords<int> { X = 0, Y = 0 },
            //    new Coords<int> { X = 0, Y = 3 },
            //    new Coords<int> { X = 4, Y = 0 }
            //};

            //Span<int> numbers = stackalloc[] { 1, 2, 3, 4, 5, 6 };
            //var ind = numbers.IndexOfAny(stackalloc[] { 2, 4, 6, 8 });
            //Console.WriteLine(ind);  // output: 1

            // Coords<int> strx = default;
            //strx.X = 1;
            //strx.Y = 2;
            return skippedLines;
            // file is disposed here
        }

        // https://github.com/dotnet/extensions
        // https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#asynchronous-streams
        // https://stu.dev/csharp8-doing-unsupported-things/

        //public static async System.Collections.Generic.IAsyncEnumerable<int> GenerateSequence()
        //{
        //    for (int i = 0; i < 20; i++)
        //    {
        //        await Task.Delay(100);
        //        yield return i;
        //    }
        //}

        //public struct Coords<T>
        //{
        //    private T x;
        //    public T Y;

        //    public T X { get => x; set => x = value; }

        //    public override bool Equals(object obj)
        //    {
        //        return obj is Coords<T> coords &&
        //               EqualityComparer<T>.Default.Equals(Y, coords.Y);
        //    }

        //    public override int GetHashCode()
        //    {
        //        return 613529524 + EqualityComparer<T>.Default.GetHashCode(Y);
        //    }
        //}
    }
}
