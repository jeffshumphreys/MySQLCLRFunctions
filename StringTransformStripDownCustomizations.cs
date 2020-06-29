using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MySQLCLRFunctions
{
    public static class StringTransformStripDownCustomizations
    {
        /***************************************************************************************************************************************************************************************************
         * 
         * StripDownCherwellDescription    Descriptions in Cherwell tickets are often full of garbage.  Strip out garbage.
         * 
         **************************************************************************************************************************************************************************************/
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static string StripDownCherwellDescription(string input)
        {
            if (input == null) return null;
            if (string.IsNullOrWhiteSpace(input)) return input;
            return input.Trim();
        }
    }
}
/*
 * 
 * using System;
using System.Text;
using System.Runtime.InteropServices;

class Class1
{
    [DllImport("kernel32", SetLastError=true)]
    static extern unsafe uint GetWindowsDirectory(byte* lpBuffer,uint uSize);

    static void Main(string[] args)
    {
        byte[] buff = new byte[512]; 
        unsafe 
        {
            fixed(byte *pbuff=buff)GetWindowsDirectory(pbuff,512);
        }
        ASCIIEncoding ae = new ASCIIEncoding();
        System.Console.WriteLine(ae.GetString(buff));
    }
}

        unsafe public struct student
        {
            public fixed char Name[126];
            public fixed char Family[126];
            public int ID;
            public float mark;
            public student* next;

        };
       public student* start;


        private void button1_Click(object sender, EventArgs e)
        {
            student m = new student();
            unsafe{
                if(start==null)
                {
                    start = &m;
                    start->Name[126] = textBox1.Text.ToCharArray(); 
                    MessageBox.Show(Convert.ToString(start->Name));


                }




            }



*/
