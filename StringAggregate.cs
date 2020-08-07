using Microsoft.SqlServer.Server; // Added for Format.UserDefined
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQLCLRFunctions
{
    public static class StringAggregate
    {
        // Product
        // Median
        // with "AND"  so "X, Y, and Z"  or "OR"

    }

    // Stolen from the desk of https://github.com/enriquecatala/SQLCLRUtils/blob/master/SQLCLRUtils/clr/CONCAT_AGG.cs
    // Using as a baseline for new ideas since 2019 has STRING_AGG already, with multi-column sorting. A distinct aggregate, for example.
    [Microsoft.SqlServer.Server.SqlUserDefinedAggregate(Format.UserDefined,
    IsInvariantToNulls = true,
    IsInvariantToOrder = false,
    IsInvariantToDuplicates = false,
    IsNullIfEmpty = true,
    MaxByteSize = -1,
    Name = nameof(StringTogether))]
    public struct StringTogether : IBinarySerialize
    {
        private StringBuilder result;
        public void Init()
        {
            // Put your code here
            result = new StringBuilder();
        }

        public void Accumulate(SqlString Value)
        {
            if (Value.IsNull)
                return;

            result.Append(Value.Value).Append(';');
        }

        public void Merge(StringTogether Group)
        {
            result.Append(Group.result);
        }

        public SqlString Terminate()
        {
            String trimmedResult = null;

            if (result?.Length > 0)
                trimmedResult = result.ToString(0, result.Length - 1);
            return new SqlString(trimmedResult);
        }

        public void Read(System.IO.BinaryReader r)
        {
            result = new StringBuilder(r.ReadString());
        }

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(result.ToString());
        }
    }
}
