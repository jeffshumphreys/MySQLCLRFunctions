using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySQLCLRFunctions;

namespace TestMySQLCLRFunctions
{
    class Program
    {
        static void Main(string[] args)
        {
            string output; string input; string marker; int markerno; int howmanyback;
            //            var i = MySQLCLRFunctions.StringExtract.LeftOf("High.There", ".");
            //            Debug.Print(i);
            //            i = MySQLCLRFunctions.StringExtract.LeftOfNth("High.There", ".", 2);
            //            Debug.Print(i);
            input = "EDWPROD.UserData.x.y";
            marker = ".";
            markerno = 2;
            howmanyback = 1;
            output = MySQLCLRFunctions.StringExtract.LeftOfNth(input, marker, markerno);
            Debug.Print($"MySQLCLRFunctions.StringExtract.LeftOfNth(\"{input}\", \"{marker}\", {markerno});=> {output}");
            markerno = 3;
            output = MySQLCLRFunctions.StringExtract.LeftOfNth(input, marker, markerno);
            Debug.Print($"MySQLCLRFunctions.StringExtract.LeftOfNth(\"{input}\", \"{marker}\", {markerno});=> {output}");
            markerno = 5;
            output = MySQLCLRFunctions.StringExtract.LeftOfNth(input, marker, markerno);
            Debug.Print($"MySQLCLRFunctions.StringExtract.LeftOfNth(\"{input}\", \"{marker}\", {markerno});=> {output}");

            markerno = 1;
            output = MySQLCLRFunctions.StringExtract.LeftMOfNth(input, marker, markerno, howmanyback);
            Debug.Print($"MySQLCLRFunctions.StringExtract.LeftMOfNth(\"{input}\", \"{marker}\", {markerno}, {howmanyback});=> {output}");
            markerno = 2; howmanyback = 2;
            output = MySQLCLRFunctions.StringExtract.LeftMOfNth(input, marker, markerno, howmanyback);
            Debug.Print($"MySQLCLRFunctions.StringExtract.LeftMOfNth(\"{input}\", \"{marker}\", {markerno}, {howmanyback});=> {output}");
            markerno = 1; howmanyback = 2;
            output = MySQLCLRFunctions.StringExtract.LeftMOfNth(input, marker, markerno, howmanyback);
            Debug.Print($"MySQLCLRFunctions.StringExtract.LeftMOfNth(\"{input}\", \"{marker}\", {markerno}, {howmanyback});=> {output}");
            markerno = 2; howmanyback = 4;
            output = MySQLCLRFunctions.StringExtract.LeftMOfNth(input, marker, markerno, howmanyback);
            Debug.Print($"MySQLCLRFunctions.StringExtract.LeftMOfNth(\"{input}\", \"{marker}\", {markerno}, {howmanyback});=> {output}");
            input = "[xxx]]y]";
            output = MySQLCLRFunctions.StringTransform.RemoveSQLServerNameDelimiters($"{input}");
            Debug.Print($"MySQLCLRFunctions.StringTransform.RemoveSQLServerNameDelimiters(\"{input}\");=> {output}");
        }
    }
}
