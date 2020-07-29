using System.ComponentModel;

namespace MySQLCLRFunctions
{
    internal static class _SharedConstants
    {
        internal const int NOT_FOUND = -1;
        internal const int BACKSET_FOR_ZEROBASED = -1;
        internal const int UPSET_FOR_ZEROBASED = 1;
        internal const int UPSET_TO_ONEBASED_FROM_ZEROBASED = 1;
        internal const int ADJUST_POINTER_TO_INCLUDE = 1;  // So if the index of function returns a zero-based pointer, then to grab the value pointed at, we need to add one to include it.
    }
}
