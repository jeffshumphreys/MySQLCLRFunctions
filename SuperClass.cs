using System;
using System.Collections;
using Microsoft.SqlServer.Server;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using System.Text;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.IO;
using System.Xml.Schema;

namespace MySQLCLRFunctions
{
    public static class TheFunctions
    {
    }
}

/*
 * 
 * TO ADD:
 * - ReplaceRecurse
 * - Pull out parenthesed strings
 * - Any extended latin characters
 * - Any unicode characters, regardless of if the string is NVARCHAR
 */

// Name in dotted parts. 2nd from end (previous extension)
// CenterString
// IsFirstPartNumeric
// InitCap
// LastIndexOf
// NthIndexOf
// IndexOfWhenBefore
// IndexOfWhenAfter
// FoundAfterButBefore
// WordWrap
// WordUnwrap
// SplitKeyValuePairs     'x=1;d=Joe;'
// Cut
// ExtractNthSplit
// StripParens  (Table)
// UnNest (Table)
// SplitByFixedWidth
// SplitByFixedWidths
// StringToMoney         "$30,3920.32" to money type
// StringToDecimal
// StripDiacritics
// UnescapeHTML
// EscapeStandard      /0/a/b/f/n/r/t

// ScrapWebPage
// GenerateDateTimeRangeBetween
// Convert a binary field to image file (Cherwell)
// IsHoliday
// GetFileType from magic byte
// Is file text?
// Is file sql?
// is it language with stop words?
// is it csv, fixed width?
// FileTouch
// Describe multiple result sets?
// return sp_who as table?
// SaxonTransform
// BisonParse
// SessionRunningGlobalVar

/*
 * 
 * \v = Vertical Tab ( CHAR(11) )
    o \? = Question Mark ( CHAR(63) )
    o \\ = Reverse Solidus (i.e. “Backslash” CHAR(92) )
    o \' = Apostrophe (i.e. “Single Quote” CHAR(39) )
    o \" = Quotation Mark (i.e. “Double Quote” CHAR(34) )
    o \0[?][?] = Octal notation (1 - 3 digits in the form of: \[0-7] or \[0-7][0-7] or \[0-3][0-7][0-7])
    o \xH[?][?][?] = variable-length UCS-2 Code Point (1 - 4 hex digits: 0-9, A-F)
    o \uHHHH = 4 hex digit UCS-2 Code Point / UTF-16 Code Unit (same as UTF-16 BE; range is
    \u0000 - \uFFFF)
    o \U00HHHHHH = 8 hex digit Unicode Code Point (first two digits are always 00; same as
    UTF-32 BE; range is \U00000000 - \U0010FFFF)

 * EXEC SQL#.String_SplitIntoFields 'SELECT * FROM #SplitTest', '[ ]+', 'Name,Title
    , Alias '
    /*
    Name Title Alias Field4 Field5
    -------- -------- -------- -------- --------
    Value1 Value2 Value3 Value4
    NewValue1 NewValue2 NewValue3 Value4
    Another1 Another2 Another3
    a b c d e
 */

/*
[SqlFunction(FillRowMethodName = "FillRow")]
public static IEnumerable InitMethod(String logname)
{
    return new EventLog(logname).Entries;
}

public static void FillRow(Object obj, out SqlDateTime timeWritten, out SqlChars message, out SqlChars category, out long instanceId)
{
    EventLogEntry eventLogEntry = (EventLogEntry)obj;
    timeWritten = new SqlDateTime(eventLogEntry.TimeWritten);
    message = new SqlChars(eventLogEntry.Message);
    category = new SqlChars(eventLogEntry.Category);
    instanceId = eventLogEntry.InstanceId;
}
[Microsoft.SqlServer.Server.SqlProcedure]
public static void HelloWorld(out string text)
{
    SqlContext.Pipe.Send("Hello worldcccccccccccccccccccccccccccccccc!" + Environment.NewLine);
    text = "Hello world!";
}

[Microsoft.SqlServer.Server.SqlProcedure]
public static void WindowsIDTestProc()
{
    WindowsIdentity clientId = null;
    WindowsImpersonationContext impersonatedUser = null;

    // Get the client ID.  
    clientId = SqlContext.WindowsIdentity;

    // This outer try block is used to thwart exception filter   
    // attacks which would prevent the inner finally   
    // block from executing and resetting the impersonation.  
    try
    {
        try
        {
            impersonatedUser = clientId.Impersonate();
            if (impersonatedUser != null)
            {
                // Perform some action using impersonation.  
            }
        }
        finally
        {
            // Undo impersonation.  
            if (impersonatedUser != null)
                impersonatedUser.Undo();
        }
    }
    catch
    {
        throw;
    }
}

[Microsoft.SqlServer.Server.SqlProcedure]
public static void CreateNewRecordProc()
{
    // Variables.         
    SqlDataRecord record;

    // Create a new record with the column metadata.  The constructor   
    // is able to accept a variable number of parameters.  
    record = new SqlDataRecord(new SqlMetaData("EmployeeID", SqlDbType.Int),
                               new SqlMetaData("Surname", SqlDbType.NVarChar, 20),
                               new SqlMetaData("GivenName", SqlDbType.NVarChar, 20),
                               new SqlMetaData("StartDate", SqlDbType.DateTime));

    // Set the record fields.  
    record.SetInt32(0, 0042);
    record.SetString(1, "Funk");
    record.SetString(2, "Don");
    record.SetDateTime(3, new DateTime(2005, 7, 17));

    // Send the record to the calling program.  
    SqlContext.Pipe.Send(record);
}

[SqlTrigger(Name = @"EmailAudit", Target = "[dbo].[Users]", Event = "FOR INSERT, UPDATE, DELETE")]
public static void EmailAudit()
{
    string userName;
    string realName;
    SqlCommand command;
    SqlTriggerContext triggContext = SqlContext.TriggerContext;
    SqlPipe pipe = SqlContext.Pipe;
    SqlDataReader reader;

    switch (triggContext.TriggerAction)
    {
        case TriggerAction.Insert:
            // Retrieve the connection that the trigger is using  
            using (SqlConnection connection
               = new SqlConnection(@"context connection=true"))
            {
                connection.Open();
                command = new SqlCommand(@"SELECT * FROM INSERTED;",
                   connection);
                reader = command.ExecuteReader();
                reader.Read();
                userName = (string)reader[0];
                realName = (string)reader[1];
                reader.Close();

                if (IsValidEMailAddress(userName))
                {
                    command = new SqlCommand(
                       @"INSERT [dbo].[UserNameAudit] VALUES ('"
                       + userName + @"', '" + realName + @"');",
                       connection);
                    pipe.Send(command.CommandText);
                    command.ExecuteNonQuery();
                    pipe.Send("You inserted: " + userName);
                }
            }

            break;

        case TriggerAction.Update:
            // Retrieve the connection that the trigger is using  
            using (SqlConnection connection
               = new SqlConnection(@"context connection=true"))
            {
                connection.Open();
                command = new SqlCommand(@"SELECT * FROM INSERTED;",
                   connection);
                reader = command.ExecuteReader();
                reader.Read();

                userName = (string)reader[0];
                realName = (string)reader[1];

                pipe.Send(@"You updated: '" + userName + @"' - '"
                   + realName + @"'");

                for (int columnNumber = 0; columnNumber < triggContext.ColumnCount; columnNumber++)
                {
                    pipe.Send("Updated column "
                       + reader.GetName(columnNumber) + "? "
                       + triggContext.IsUpdatedColumn(columnNumber).ToString());
                }

                reader.Close();
            }

            break;

        case TriggerAction.Delete:
            using (SqlConnection connection
               = new SqlConnection(@"context connection=true"))
            {
                connection.Open();
                command = new SqlCommand(@"SELECT * FROM DELETED;",
                   connection);
                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    pipe.Send(@"You deleted the following rows:");
                    while (reader.Read())
                    {
                        pipe.Send(@"'" + reader.GetString(0)
                        + @"', '" + reader.GetString(1) + @"'");
                    }

                    reader.Close();

                    //alternately, to just send a tabular resultset back:  
                    //pipe.ExecuteAndSend(command);  
                }
                else
                {
                    pipe.Send("No rows affected.");
                }
            }

            break;
    }
}

public static bool IsValidEMailAddress(string email)
{
    return Regex.IsMatch(email, @"^([\w-]+\.)*?[\w-]+@[\w-]+\.([\w-]+\.)*?[\w]+$");
}

// Enter existing table or view for the target and uncomment the attribute line  
// [Microsoft.SqlServer.Server.SqlTrigger (Name="trig_InsertValidator", Target="Table1", Event="FOR INSERT")]  
public static void trig_InsertValidator()
{
    using (SqlConnection connection = new SqlConnection(@"context connection=true"))
    {
        SqlCommand command;
        SqlDataReader reader;
        int value;

        // Open the connection.  
        connection.Open();

        // Get the inserted value.  
        command = new SqlCommand(@"SELECT * FROM INSERTED", connection);
        reader = command.ExecuteReader();
        reader.Read();
        value = (int)reader[0];
        reader.Close();

        // Rollback the transaction if a value of 1 was inserted.  
        if (1 == value)
        {
            try
            {
                // Get the current transaction and roll it back.  
                Transaction trans = Transaction.Current;
                trans.Rollback();
            }
            catch (SqlException ex)
            {
                // Catch the expected exception.                      
            }
        }
        else
        {
            // Perform other actions here.  
        }

        // Close the connection.  
        connection.Close();
    }
}

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.Native,
 IsByteOrdered = true, ValidationMethodName = "ValidatePoint")]
public struct Point : INullable
{
    private bool is_Null;
    private Int32 _x;
    private Int32 _y;

    public bool IsNull
    {
        get
        {
            return (is_Null);
        }
    }

    public static Point Null
    {
        get
        {
            Point pt = new Point();
            pt.is_Null = true;
            return pt;
        }
    }

    // Use StringBuilder to provide string representation of UDT.  
    public override string ToString()
    {
        // Since InvokeIfReceiverIsNull defaults to 'true'  
        // this test is unnecessary if Point is only being called  
        // from SQL.  
        if (this.IsNull)
            return "NULL";
        else
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(_x);
            builder.Append(",");
            builder.Append(_y);
            return builder.ToString();
        }
    }

    [SqlMethod(OnNullCall = false)]
    public static Point Parse(SqlString s)
    {
        // With OnNullCall=false, this check is unnecessary if   
        // Point only called from SQL.  
        if (s.IsNull)
            return Null;

        // Parse input string to separate out points.  
        Point pt = new Point();
        string[] xy = s.Value.Split(",".ToCharArray());
        pt.X = Int32.Parse(xy[0]);
        pt.Y = Int32.Parse(xy[1]);

        // Call ValidatePoint to enforce validation  
        // for string conversions.  
        if (!pt.ValidatePoint())
            throw new ArgumentException("Invalid XY coordinate values.");
        return pt;
    }

    // X and Y coordinates exposed as properties.  
    public Int32 X
    {
        get
        {
            return this._x;
        }
        // Call ValidatePoint to ensure valid range of Point values.  
        set
        {
            Int32 temp = _x;
            _x = value;
            if (!ValidatePoint())
            {
                _x = temp;
                throw new ArgumentException("Invalid X coordinate value.");
            }
        }
    }

    public Int32 Y
    {
        get
        {
            return this._y;
        }
        set
        {
            Int32 temp = _y;
            _y = value;
            if (!ValidatePoint())
            {
                _y = temp;
                throw new ArgumentException("Invalid Y coordinate value.");
            }
        }
    }

    // Validation method to enforce valid X and Y values.  
    private bool ValidatePoint()
    {
        // Allow only zero or positive integers for X and Y coordinates.  
        if ((_x >= 0) && (_y >= 0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Distance from 0 to Point method.  
    [SqlMethod(OnNullCall = false)]
    public Double Distance()
    {
        return DistanceFromXY(0, 0);
    }

    // Distance from Point to the specified point method.  
    [SqlMethod(OnNullCall = false)]
    public Double DistanceFrom(Point pFrom)
    {
        return DistanceFromXY(pFrom.X, pFrom.Y);
    }

    // Distance from Point to the specified x and y values method.  
    [SqlMethod(OnNullCall = false)]
    public Double DistanceFromXY(Int32 iX, Int32 iY)
    {
        return Math.Sqrt(Math.Pow(iX - _x, 2.0) + Math.Pow(iY - _y, 2.0));
    }
}
*/
//    }
//}
/*
 * 
 * sp_configure 'clr enabled', 1;
GO
reconfigure
GO 
ALTER DATABASE CURRENT SET TRUSTWORTHY ON 

 * CREATE ASSEMBLY helloworld from 'c:\helloworld.dll' WITH PERMISSION_SET = SAFE
 * Next, we need to set valid permissions. As we are using static "Encrypt" and "Decrypt" methods, we need to set the permissions to "Unrestricted" mode as shown below:
 * 
 * CREATE FUNCTION [dbo].Encrypt(@Input nvarchar(max)) RETURNS nvarchar(max)
EXTERNAL NAME  EDCLR.EDCLR.Encrypt
Go 
CREATE FUNCTION [dbo].Decrypt(@Input nvarchar(max)) RETURNS nvarchar(max)
EXTERNAL NAME EDCLR.EDCLR.Decrypt; 

CREATE TABLE Users  
(  
    UserName nvarchar(200) NOT NULL,  
    RealName nvarchar(200) NOT NULL  
);  
GO CREATE TABLE UserNameAudit  
(  
    UserName nvarchar(200) NOT NULL,  
    RealName nvarchar(200) NOT NULL  
)  

CREATE TRIGGER EmailAudit  
ON Users  
FOR INSERT, UPDATE, DELETE  
AS  
EXTERNAL NAME SQLCLRTest.CLRTriggers.EmailAudit  

CREATE TABLE Table1(c1 int);  
go  

CREATE TRIGGER trig_InsertValidator  
ON Table1  
FOR INSERT  
AS EXTERNAL NAME ValidationTriggers.Triggers.trig_InsertValidator;  
go  

-- Use a Try/Catch block to catch the expected exception  
BEGIN TRY  
   INSERT INTO Table1 VALUES(42)  
   INSERT INTO Table1 VALUES(1)  
END TRY  
BEGIN CATCH  
  SELECT ERROR_NUMBER() AS ErrorNum, ERROR_MESSAGE() AS ErrorMessage  
END CATCH;  

CREATE TYPE dbo.Point  EXTERNAL NAME Point.[Point]

DROP FUNCTION IF EXISTS ReadEventLog
CREATE FUNCTION ReadEventLog(@logname nvarchar(100))  
RETURNS TABLE   
(logTime datetime,Message nvarchar(MAX),Category nvarchar(4000),InstanceId bigint)  
AS   
EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.TheFunctions].InitMethod;  
GO  


    SELECT TOP 100 * FROM dbo.ReadEventLog(N'Security') as T;  
*/

