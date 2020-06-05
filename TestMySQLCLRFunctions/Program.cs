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
            string output; string input; string marker; string markerchars; string markers; int markerno; int howmanyback;

            input = "EDWPROD.UserData.x.y"; marker = "."; markerno = 2; howmanyback = 1;
            output = MySQLCLRFunctions.StringExtract.LeftOfNth(input, marker, markerno); if (output == null) output = "{null}";
            Debug.Print($"MySQLCLRFunctions.StringExtract.LeftOfNth(\"{input}\", \"{marker}\", {markerno});=>{output}<=");
            
            markerno = 3;
            output = MySQLCLRFunctions.StringExtract.LeftOfNth(input, marker, markerno); if (output == null) output = "{null}";
            Debug.Print($"MySQLCLRFunctions.StringExtract.LeftOfNth(\"{input}\", \"{marker}\", {markerno});=>{output}<=");
            
            markerno = 5;
            output = MySQLCLRFunctions.StringExtract.LeftOfNth(input, marker, markerno); if (output == null) output = "{null}";
            Debug.Print($"MySQLCLRFunctions.StringExtract.LeftOfNth(\"{input}\", \"{marker}\", {markerno});=>{output}<=");

            markerno = 1;
            output = MySQLCLRFunctions.StringExtract.LeftMOfNth(input, marker, markerno, howmanyback); if (output == null) output = "{null}";
            Debug.Print($"MySQLCLRFunctions.StringExtract.LeftMOfNth(\"{input}\", \"{marker}\", {markerno}, {howmanyback});=>{output}<=");
            
            markerno = 2; howmanyback = 2;
            output = MySQLCLRFunctions.StringExtract.LeftMOfNth(input, marker, markerno, howmanyback); if (output == null) output = "{null}";
            Debug.Print($"MySQLCLRFunctions.StringExtract.LeftMOfNth(\"{input}\", \"{marker}\", {markerno}, {howmanyback});=>{output}<=");
 
            markerno = 1; howmanyback = 2;
            output = MySQLCLRFunctions.StringExtract.LeftMOfNth(input, marker, markerno, howmanyback); if (output == null) output = "{null}";
            Debug.Print($"MySQLCLRFunctions.StringExtract.LeftMOfNth(\"{input}\", \"{marker}\", {markerno}, {howmanyback});=>{output}<=");

            markerno = 2; howmanyback = 4;
            output = MySQLCLRFunctions.StringExtract.LeftMOfNth(input, marker, markerno, howmanyback); if (output == null) output = "{null}";
            Debug.Print($"MySQLCLRFunctions.StringExtract.LeftMOfNth(\"{input}\", \"{marker}\", {markerno}, {howmanyback});=>{output}<=");

            markerno = 1; howmanyback = 1; input = ".."; marker = ".";
            output = MySQLCLRFunctions.StringExtract.LeftMOfNth(input, marker, markerno, howmanyback); if (output == null) output = "{null}";
            Debug.Print($"MySQLCLRFunctions.StringExtract.LeftMOfNth(\"{input}\", \"{marker}\", {markerno}, {howmanyback});=>{output}<=");

            markerno = 1; howmanyback = 1; input = ".."; marker = "..";
            output = MySQLCLRFunctions.StringExtract.LeftMOfNth(input, marker, markerno, howmanyback); if (output == null) output = "{null}";
            Debug.Print($"MySQLCLRFunctions.StringExtract.LeftMOfNth(\"{input}\", \"{marker}\", {markerno}, {howmanyback});=>{output}<=");

            markerno = 2; howmanyback = 1; input = ".."; marker = ".";
            output = MySQLCLRFunctions.StringExtract.LeftMOfNth(input, marker, markerno, howmanyback); if (output == null) output = "{null}";
            Debug.Print($"MySQLCLRFunctions.StringExtract.LeftMOfNth(\"{input}\", \"{marker}\", {markerno}, {howmanyback});=>{output}<=");

            markerno = 2; howmanyback = 1; input = ".."; marker = ".";
            output = MySQLCLRFunctions.StringExtract.LeftMOfNth(input, marker, markerno, howmanyback); if (output == null) output = "{null}";
            Debug.Print($"MySQLCLRFunctions.StringExtract.LeftMOfNth(\"{input}\", \"{marker}\", {markerno}, {howmanyback});=>{output}<=");

            input = "He.was;therefore:Not."; markerchars = ".,;:";
            output = MySQLCLRFunctions.StringExtract.LeftOfAny(input, markerchars); if (output == null) output = "{null}";
            Debug.Print($"MySQLCLRFunctions.StringExtract.LeftOfAny(\"{input}\", \"{markerchars}\";=>{output}<=");

            input = "He.was;therefore:Not."; markerchars = ";:.,";
            output = MySQLCLRFunctions.StringExtract.LeftOfAny(input, markerchars); if (output == null) output = "{null}";
            Debug.Print($"MySQLCLRFunctions.StringExtract.LeftOfAny(\"{input}\", \"{markerchars}\";=>{output}<=");

            input = "[xxx]]y]";
            output = MySQLCLRFunctions.StringTransform.RemoveSQLServerNameDelimiters($"{input}"); if (output == null) output = "{null}";
            Debug.Print($"MySQLCLRFunctions.StringTransform.RemoveSQLServerNameDelimiters(\"{input}\");=>{output}<=");
        }
    }
}
