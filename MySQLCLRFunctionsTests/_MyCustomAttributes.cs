using System;

namespace MySQLCLRFunctions.Tests
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    internal class PositiveTestAttribute : Attribute
    {
    }
}