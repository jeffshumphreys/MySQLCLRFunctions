using Microsoft.SqlServer.Server;
using System;
using System.Linq;

namespace MySQLCLRFunctions
{
    public static class Compares
    {
        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = false, IsPrecise = true)]
        public static DateTime? Max2DateTimes(DateTime? d1, DateTime? d2)
        {
            DateTime?[] itemlist = new[] { d1, d2 };
            return itemlist.Max();
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = false, IsPrecise = true)]
        public static DateTime? Max3DateTimes(DateTime? d1, DateTime? d2, DateTime? d3)
        {
            DateTime?[] itemlist = new[] { d1, d2, d3 };
            return itemlist.Max();
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = false, IsPrecise = true)]
        public static DateTime? Max4DateTimes(DateTime? d1, DateTime? d2, DateTime? d3, DateTime? d4)
        {
            DateTime?[] itemlist = new[] { d1, d2, d3, d4 };
            return itemlist.Max();
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static int? Max2Integers(int? d1, int? d2)
        {
            int?[] itemlist = new[] { d1, d2 };
            return itemlist.Max();
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static int? Max3Integers(int? d1, int? d2, int? d3)
        {
            int?[] itemlist = new[] { d1, d2, d3 };
            return itemlist.Max();
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static int? Max4Integers(int? d1, int? d2, int? d3, int? d4)
        {
            int?[] itemlist = new[] { d1, d2, d3, d4 };
            return itemlist.Max();
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static Int64? Max2BigInts(Int64? d1, Int64? d2)
        {
            Int64?[] itemlist = new[] { d1, d2 };
            return itemlist.Max();
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static Int64? Max3BigInts(Int64? d1, Int64? d2, Int64? d3)
        {
            Int64?[] itemlist = new[] { d1, d2, d3 };
            return itemlist.Max();
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static Int64? Max4BigInts(Int64? d1, Int64? d2, Int64? d3, Int64? d4)
        {
            Int64?[] itemlist = new[] { d1, d2, d3, d4 };
            return itemlist.Max();
        }

        //**********************************************************************************************************************
        //         Mins
        //**********************************************************************************************************************

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = false, IsPrecise = true)]
        public static DateTime? Min2DateTimes(DateTime? d1, DateTime? d2)
        {
            DateTime?[] itemlist = new[] { d1, d2 };
            return itemlist.Min();
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = false, IsPrecise = true)]
        public static DateTime? Min3DateTimes(DateTime? d1, DateTime? d2, DateTime? d3)
        {
            DateTime?[] itemlist = new[] { d1, d2, d3 };
            return itemlist.Min();
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = false, IsPrecise = true)]
        public static DateTime? Min4DateTimes(DateTime? d1, DateTime? d2, DateTime? d3, DateTime? d4)
        {
            DateTime?[] itemlist = new[] { d1, d2, d3, d4 };
            return itemlist.Min();
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static int? Min2Integers(int? d1, int? d2)
        {
            int?[] itemlist = new[] { d1, d2 };
            return itemlist.Min();
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static int? Min3Integers(int? d1, int? d2, int? d3)
        {
            int?[] itemlist = new[] { d1, d2, d3 };
            return itemlist.Min();
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static int? Min4Integers(int? d1, int? d2, int? d3, int? d4)
        {
            int?[] itemlist = new[] { d1, d2, d3, d4 };
            return itemlist.Min();
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static Int64? Min2BigInts(Int64? d1, Int64? d2)
        {
            Int64?[] itemlist = new[] { d1, d2 };
            return itemlist.Min();
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static Int64? Min3BigInts(Int64? d1, Int64? d2, Int64? d3)
        {
            Int64?[] itemlist = new[] { d1, d2, d3 };
            return itemlist.Min();
        }

        [SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true)]
        public static Int64? Min4BigInts(Int64? d1, Int64? d2, Int64? d3, Int64? d4)
        {
            Int64?[] itemlist = new[] { d1, d2, d3, d4 };
            return itemlist.Min();
        }
    }
}
