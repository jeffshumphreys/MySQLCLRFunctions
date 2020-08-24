-- Time to execute: 14 sec
:SETVAR LibName MySQLCLRFunctions
:R TestValueSettings.localuseonly
SELECT ThisServer = @@servername, ThisDatabase = DB_NAME(), ThisUser = ORIGINAL_LOGIN()                 
GO
SELECT name, principal_id, clr_name, permission_set, is_visible, create_date, modify_date, is_user_defined FROM sys.assemblies AS a WHERE name = '$(LibName)'
select * from sys.trusted_assemblies
GO
-- If anything changes, drop and recreate
/**************************************************************************************************************************************************************************************************
 *      Event Logs
/***************************************************************************************************************************************************************************************************/*/
DROP FUNCTION IF EXISTS ReadEventLog
/**************************************************************************************************************************************************************************************************
 *      Networking
/***************************************************************************************************************************************************************************************************/*/
DROP FUNCTION IF EXISTS GetHostRealName
DROP FUNCTION IF EXISTS GetHostAliases
DROP FUNCTION IF EXISTS GetHostNames
DROP FUNCTION IF EXISTS PingGetReturnBuffer
DROP FUNCTION IF EXISTS PingGetAddress
DROP FUNCTION IF EXISTS Ping
DROP FUNCTION IF EXISTS IsIP4
DROP FUNCTION IF EXISTS GetMyIP4
DROP FUNCTION IF EXISTS SpreadHex
/**************************************************************************************************************************************************************************************************
 *      Convert
/***************************************************************************************************************************************************************************************************/*/
DROP FUNCTION IF EXISTS VarBin2Hex
DROP FUNCTION IF EXISTS ADDateTimeString2DateTime
/**************************************************************************************************************************************************************************************************
 *      Compare
/***************************************************************************************************************************************************************************************************/*/
DROP FUNCTION IF EXISTS Max2DateTimes
DROP FUNCTION IF EXISTS Max3DateTimes
DROP FUNCTION IF EXISTS Max4DateTimes
/**************************************************************************************************************************************************************************************************
 *      Interact with Environment (Doesn't work)
/***************************************************************************************************************************************************************************************************/*/
DROP FUNCTION IF EXISTS Beep
DROP FUNCTION IF EXISTS BeepStandard
/**************************************************************************************************************************************************************************************************
 *      String Formatting
/***************************************************************************************************************************************************************************************************/*/
DROP FUNCTION IF EXISTS Title
DROP FUNCTION IF EXISTS HumanizeDateTimeDiff
DROP FUNCTION IF EXISTS RPad
DROP FUNCTION IF EXISTS RPadChar
DROP FUNCTION IF EXISTS RPadC
DROP FUNCTION IF EXISTS LPad
DROP FUNCTION IF EXISTS LPadChar
DROP FUNCTION IF EXISTS LPadC
/**************************************************************************************************************************************************************************************************
 *      String Pivots
/***************************************************************************************************************************************************************************************************/*/
DROP FUNCTION IF EXISTS Pieces
DROP FUNCTION IF EXISTS PiecesX
DROP FUNCTION IF EXISTS PiecesWithContext
DROP FUNCTION IF EXISTS PiecesWithContextX
DROP FUNCTION IF EXISTS PiecesWithMatches
DROP FUNCTION IF EXISTS PiecesWithMatchesX
DROP FUNCTION IF EXISTS Matches
DROP FUNCTION IF EXISTS MatchesX
DROP FUNCTION IF EXISTS Captures
DROP FUNCTION IF EXISTS CapturesX
DROP FUNCTION IF EXISTS StringSplitOnRegEx
DROP FUNCTION IF EXISTS KeyValuePairsWithMultiValues
DROP FUNCTION IF EXISTS KeyValuePairsWithMultiValuesS
DROP FUNCTION IF EXISTS NearX
/**************************************************************************************************************************************************************************************************
 *      String Extract
/***************************************************************************************************************************************************************************************************/*/
DROP FUNCTION IF EXISTS GetFirstName
--DROP FUNCTION IF EXISTS GetLastName
DROP FUNCTION IF EXISTS LeftOf
DROP FUNCTION IF EXISTS LeftOfS
DROP FUNCTION IF EXISTS LeftOfAny
DROP FUNCTION IF EXISTS LeftOfAnyC
DROP FUNCTION IF EXISTS LeftOfNth
DROP FUNCTION IF EXISTS LeftOfNthS
DROP FUNCTION IF EXISTS LeftMOfNth
DROP FUNCTION IF EXISTS LeftMOfNthS
DROP FUNCTION IF EXISTS LeftMthOfNthS
DROP FUNCTION IF EXISTS FirstWord
DROP FUNCTION IF EXISTS FirstWordW
DROP FUNCTION IF EXISTS FirstWordBefore
DROP FUNCTION IF EXISTS FirstWordBeforeS
DROP FUNCTION IF EXISTS FirstWordBeforeAny
DROP FUNCTION IF EXISTS FirstWordBeforeAnyChar
DROP FUNCTION IF EXISTS FirstWordBeforeAnyC
DROP FUNCTION IF EXISTS PieceNumber
DROP FUNCTION IF EXISTS PieceNumberX
DROP FUNCTION IF EXISTS LastPiece
DROP FUNCTION IF EXISTS RightOf
DROP FUNCTION IF EXISTS RightOfAny
DROP FUNCTION IF EXISTS RightOfAnyC
DROP FUNCTION IF EXISTS EverythingAfter
DROP FUNCTION IF EXISTS EverythingAfterS
DROP FUNCTION IF EXISTS EverythingAfterX -- X = Regexpression
DROP FUNCTION IF EXISTS Mid
/**************************************************************************************************************************************************************************************************
 *      String Reduce
/***************************************************************************************************************************************************************************************************/*/
DROP FUNCTION IF EXISTS BlankOut
DROP FUNCTION IF EXISTS RemoveSQLServerNameDelimiters
DROP FUNCTION IF EXISTS TrimSQL
DROP FUNCTION IF EXISTS TrimBracketing
DROP FUNCTION IF EXISTS TrimBrackets
DROP FUNCTION IF EXISTS RTrimChar
DROP FUNCTION IF EXISTS RTrimAnyC
DROP FUNCTION IF EXISTS TrimEnd
DROP FUNCTION IF EXISTS RTrimN
DROP FUNCTION IF EXISTS RTrimOne
DROP FUNCTION IF EXISTS TrimIfStartsWith
DROP FUNCTION IF EXISTS TrimIfStartsWithS
DROP FUNCTION IF EXISTS LTrimIfStartsWithS
DROP FUNCTION IF EXISTS TrimOne
DROP FUNCTION IF EXISTS LTrimOne
DROP FUNCTION IF EXISTS LTrimN
/**************************************************************************************************************************************************************************************************
 *      String Reduce Customizations
/***************************************************************************************************************************************************************************************************/*/
DROP FUNCTION IF EXISTS TrimNormalizeStringInput
/**************************************************************************************************************************************************************************************************
 *      String Build Out (dumb name)
/***************************************************************************************************************************************************************************************************/*/
DROP FUNCTION IF EXISTS AppendWithSeparator
DROP FUNCTION IF EXISTS AppendWithComma
DROP FUNCTION IF EXISTS AppendWithTab
/**************************************************************************************************************************************************************************************************
 *      String Transformations
/***************************************************************************************************************************************************************************************************/*/
DROP FUNCTION IF EXISTS ReplaceRecursive
DROP FUNCTION IF EXISTS ReplaceRecursiveX
DROP FUNCTION IF EXISTS ReplaceRecursiveS
DROP FUNCTION IF EXISTS ReplaceMatch
DROP FUNCTION IF EXISTS ReplaceMatchX
/**************************************************************************************************************************************************************************************************
 *      String Decode
/***************************************************************************************************************************************************************************************************/*/
DROP FUNCTION IF EXISTS RevealNonPrintables
/**************************************************************************************************************************************************************************************************
 *      String Measure
/***************************************************************************************************************************************************************************************************/*/
DROP FUNCTION IF EXISTS HowMany
DROP FUNCTION IF EXISTS HowManyS
DROP FUNCTION IF EXISTS HowManyX
/**************************************************************************************************************************************************************************************************
 *      String Tests
/***************************************************************************************************************************************************************************************************/*/
DROP FUNCTION IF EXISTS StartsWith
DROP FUNCTION IF EXISTS StartsWithS
DROP FUNCTION IF EXISTS EndsWith
DROP FUNCTION IF EXISTS EndsWithS
DROP FUNCTION IF EXISTS LegalName
DROP FUNCTION IF EXISTS AnyOfTheseAreAnyOfThose
DROP FUNCTION IF EXISTS AnyOfTheseSAreAnyOfThoseS
/**************************************************************************************************************************************************************************************************
 *      File Name processing
/***************************************************************************************************************************************************************************************************/*/
DROP FUNCTION IF EXISTS FileNameExtension
DROP FUNCTION IF EXISTS FileNameWithoutExtension
DROP FUNCTION IF EXISTS FileNameWithExtension
DROP FUNCTION IF EXISTS FileInDirectory
DROP FUNCTION IF EXISTS FileInFolder
DROP FUNCTION IF EXISTS TempFilePath
/**************************************************************************************************************************************************************************************************
 *      SQL Caller Script Generators
/***************************************************************************************************************************************************************************************************/*/
DROP FUNCTION IF EXISTS ExpandSQLParameterString
DROP FUNCTION IF EXISTS ExpandSQLParameter
DROP FUNCTION IF EXISTS BuildRaiserrorMessage
/*
    Embedded in Table Def!!!
*/
--DROP FUNCTION IF EXISTS HowManyX

DECLARE @errmsg NVARCHAR(2048)

--DROP ASSEMBLY MySQLCLRFunctions
--Msg 10327, Level 14, State 1, Line 44
--CREATE ASSEMBLY for assembly 'MySQLCLRFunctions' failed because assembly 'MySQLCLRFunctions' is not trusted. The assembly is trusted when either of the following is true: the assembly is signed with a certificate or an asymmetric key that has a corresponding login with UNSAFE ASSEMBLY permission, or the assembly is trusted using sp_add_trusted_assembly.
-- https://nielsberglund.com/2017/07/23/sql-server-2017-sqlclr---whitelisting-assemblies/
--SELECT content FROM sys.assembly_files AS af WHERE name = '$(MySQLCLRFunctions)'
--EXEC sp_add_trusted_assembly @hash=0x8893AD6D78D14EE43DF482E2EAD44123E3A0B684A8873C3F7BF3B5E8D8F09503F3E62370CE742BBC96FE3394477214B84C7C1B0F7A04DCC788FA99C2C09DFCCC, @description='mysqlclrfunctions, version=0.0.0.0, culture=neutral, publickeytoken=null, processorarchitecture=msil' -- from sys.assemblies
--.Net SqlClient Data Provider: Msg 6285, Level 16, State 1, Line 57
--ALTER ASSEMBLY failed because the source assembly is, according to MVID, identical to an assembly that is already registered under the name "MySQLCLRFunctions".
BEGIN TRY
    CREATE ASSEMBLY MySQLCLRFunctions from 'C:\Users\humphrej2\Source\Repos\jeffshumphreys\MySQLCLRFunctions\bin\Release\MySQLCLRFunctions.dll' WITH PERMISSION_SET = UNSAFE
END TRY
BEGIN CATCH
    IF ERROR_NUMBER() = 6246 PRINT 'Already present'
    ELSE
    IF ERROR_NUMBER() = 6246 PRINT 'Already present'
    ELSE
    BEGIN
        SET @errmsg = CONCAT('Error: CANNOT ', ERROR_MESSAGE())
         PRINT @errmsg
         ;THROW 51001, @errmsg, 1
         
    END
END CATCH
BEGIN TRY
    ALTER ASSEMBLY MySQLCLRFunctions from 'C:\Users\humphrej2\Source\Repos\jeffshumphreys\MySQLCLRFunctions\bin\Release\MySQLCLRFunctions.dll' WITH PERMISSION_SET = UNSAFE, UNCHECKED DATA
END TRY
BEGIN CATCH
    IF ERROR_NUMBER() = 6285 PRINT 'MVID? Says assembly identical. Ignore.'
    ELSE IF ERROR_NUMBER() = 6270
    BEGIN
        PRINT ERROR_MESSAGE()
        ;THROW 51002, @errmsg, 1
    END
    ELSE
    BEGIN
        SET @errmsg = CONCAT('Error: CANNOT ', ERROR_MESSAGE())
         PRINT @errmsg
         ;THROW 51000, @errmsg, 1
         
    END
END CATCH
GO
/**************************************************************************************************************************************************************************************************
 *
 *       Adaptors - Technically a transformation, but also reversible in most cases.
 *
/***************************************************************************************************************************************************************************************************/*/
CREATE OR ALTER FUNCTION VarBin2Hex(@InputAsVarBin VARBINARY(MAX)) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.Adaptors].VarBin2Hex;  
GO  
CREATE OR ALTER FUNCTION ADDateTimeString2DateTime(@InputAsStringDateTime NVARCHAR(17)) RETURNS DATETIME
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.Adaptors].ADDateTimeString2DateTime;  
GO  
/**************************************************************************************************************************************************************************************************
 *
 *       Test Network - Check if a name actually points to a live server with ICMP running.
 *
/***************************************************************************************************************************************************************************************************/*/
CREATE OR ALTER FUNCTION Ping(@Machine NVARCHAR(257)) RETURNS BIT
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.NetworkTest].Ping;  
GO  
SELECT Ping = dbo.Ping('$(testsvr2)') 
GO
/**************************************************************************************************************************************************************************************************
 *
 *       Collect/Gather/Get detail from the Network about the Network
 *
/***************************************************************************************************************************************************************************************************/*/

CREATE OR ALTER FUNCTION PingGetAddress(@Machine NVARCHAR(257)) RETURNS NVARCHAR(120) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.NetworkCollect].PingGetAddress;  
GO  
SELECT PingGetAddress = dbo.PingGetAddress('$(testsvr2)')                                         -->fd85:eb7f:dad9:7777::a0a:b0e
GO
CREATE OR ALTER FUNCTION PingGetReturnBuffer(@Machine NVARCHAR(257)) RETURNS VARBINARY(200) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.NetworkCollect].PingGetReturnBuffer;  
GO  
SELECT PingGetReturnBuffer = CAST(dbo.PingGetReturnBuffer('$(testsvr2)') AS VARCHAR(400))        -->abcdefghijklmnopqrstuvwabcdefghi<--
GO
CREATE OR ALTER FUNCTION GetHostNames(@Machine NVARCHAR(257)) RETURNS NVARCHAR(600) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.NetworkCollect].GetHostNames;  
GO  
/*
A .NET Framework error occurred during execution of user-defined routine or aggregate "GetHostNames": 
System.Net.Sockets.SocketException: This is usually a temporary error during hostname resolution and means that the local server did not receive a response from an authoritative server
System.Net.Sockets.SocketException: 
   at System.Net.Dns.InternalGetHostByAddress(IPAddress address, Boolean includeIPv6)
   at System.Net.Dns.GetHostEntry(String hostNameOrAddress)
   at MySQLCLRFunctions.NetworkCollect.GetHostNames(SqlString MachineOrAlias)
  SELECT GetHostNames = CAST(dbo.GetHostNames('10.10.11.14') AS VARCHAR(400))   -- Never comes back.
  GO
*/
CREATE OR ALTER FUNCTION GetHostAliases(@Machine NVARCHAR(257)) RETURNS NVARCHAR(600) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.NetworkCollect].GetHostAliases;  
GO  
SELECT GetHostAliases = CAST(dbo.GetHostAliases('$(testsvr2)') AS VARCHAR(400))
GO
CREATE OR ALTER FUNCTION GetHostRealName(@Machine NVARCHAR(257)) RETURNS NVARCHAR(600) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.NetworkCollect].GetHostRealName;  
GO  
SELECT GetHostRealName = dbo.GetHostRealName('$(testsvr1)')
SELECT GetHostRealName = dbo.GetHostRealName('$(testsvr1a)')
GO
CREATE OR ALTER FUNCTION GetMyIP4() RETURNS NVARCHAR(20)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.NetworkCollect].GetMyIP4;  
GO
SELECT MyHost = dbo.GetMyIP4()
GO
/**************************************************************************************************************************************************************************************************
 *
 *       Comparisons - Would be lovely to be generic, but not possible.
 *
/***************************************************************************************************************************************************************************************************/*/

CREATE OR ALTER FUNCTION Max2DateTimes(@d1 DATETIME2, @d2 DATETIME2) RETURNS DATETIME
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.Compares].Max2DateTimes;
GO  
SELECT Max2DateTimes = dbo.Max2DateTimes(GETDATE(), DATEADD(DAY, 1, GETDATE()))     --> shows tomorrow
GO
/**************************************************************************************************************************************************************************************************
 *
 *       Environmental - may also be machine-specific.  Things like sound, color, videio
 *
/***************************************************************************************************************************************************************************************************/*/
CREATE OR ALTER FUNCTION Beep(@frequencyHz INT, @durationMs INT) RETURNS INT
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.Environmental].Beep;  
GO  
SELECT Beep = dbo.Beep(400, 500)
GO
CREATE OR ALTER FUNCTION BeepStandard() RETURNS INT
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.Environmental].BeepStandard;  
GO  
SELECT BeepStandard = dbo.BeepStandard()
GO
/**************************************************************************************************************************************************************************************************
 *
 *       Humanization - Make things more meaningful to human readers, especially with complex reports
 *
/***************************************************************************************************************************************************************************************************/*/
CREATE OR ALTER FUNCTION HumanizeDateTimeDiff(@from DATETIME) RETURNS NVARCHAR(500)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.Humanization].HumanizeDateTimeDiff;  
GO  
SELECT HumanizeDataTimeDiff1 = dbo.HumanizeDateTimeDiff(SYSDATETIME())                                     -- FAILED
GO
SELECT HumanizeDataTimeDiff2 = dbo.HumanizeDateTimeDiff(DATEADD(YEAR, -1, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff3 = dbo.HumanizeDateTimeDiff(DATEADD(YEAR, -10, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff4 = dbo.HumanizeDateTimeDiff(DATEADD(QUARTER, -1, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff5 = dbo.HumanizeDateTimeDiff(DATEADD(QUARTER, -2, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff6 = dbo.HumanizeDateTimeDiff(DATEADD(MONTH, -1, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff7 = dbo.HumanizeDateTimeDiff(DATEADD(MONTH, -2, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff8 = dbo.HumanizeDateTimeDiff(DATEADD(WEEK, -1, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff9 = dbo.HumanizeDateTimeDiff(DATEADD(WEEK, -2, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff10 = dbo.HumanizeDateTimeDiff(DATEADD(WEEKDAY, -1, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff11 = dbo.HumanizeDateTimeDiff(DATEADD(DAY, -1, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff12 = dbo.HumanizeDateTimeDiff(DATEADD(DAY, -2, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff13 = dbo.HumanizeDateTimeDiff(DATEADD(DAY, -3, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff14 = dbo.HumanizeDateTimeDiff(DATEADD(HOUR, -12, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff15 = dbo.HumanizeDateTimeDiff(DATEADD(HOUR, -11, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff16 = dbo.HumanizeDateTimeDiff(DATEADD(HOUR, -9, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff17 = dbo.HumanizeDateTimeDiff(DATEADD(HOUR, -5, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff18 = dbo.HumanizeDateTimeDiff(DATEADD(HOUR, -3, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff19 = dbo.HumanizeDateTimeDiff(DATEADD(HOUR, -2, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff20 = dbo.HumanizeDateTimeDiff(DATEADD(HOUR, -1, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff21 = dbo.HumanizeDateTimeDiff(DATEADD(MINUTE, -1, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff22 = dbo.HumanizeDateTimeDiff(DATEADD(MINUTE, -11, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff23 = dbo.HumanizeDateTimeDiff(DATEADD(SECOND, -1, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff24 = dbo.HumanizeDateTimeDiff(DATEADD(SECOND, -2, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff25 = dbo.HumanizeDateTimeDiff(DATEADD(SECOND, -31, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff26 = dbo.HumanizeDateTimeDiff(DATEADD(SECOND, -59, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff27 = dbo.HumanizeDateTimeDiff(DATEADD(SECOND, -(11*24+1), SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff28 = dbo.HumanizeDateTimeDiff(DATEADD(SECOND, -(11*24-1), SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff29 = dbo.HumanizeDateTimeDiff(DATEADD(SECOND, -(11*23-1), SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff30 = dbo.HumanizeDateTimeDiff(DATEADD(SECOND, -(23*23-1), SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff31 = dbo.HumanizeDateTimeDiff(DATEADD(MILLISECOND, -1, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff32 = dbo.HumanizeDateTimeDiff(DATEADD(MICROSECOND, -1, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff33 = dbo.HumanizeDateTimeDiff(DATEADD(MICROSECOND, -101, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff34 = dbo.HumanizeDateTimeDiff(DATEADD(MICROSECOND, -1011, SYSDATETIME()))          -- FAILED
GO
SELECT HumanizeDataTimeDiff35 = dbo.HumanizeDateTimeDiff(DATEADD(MICROSECOND, -10111, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff36 = dbo.HumanizeDateTimeDiff(DATEADD(MICROSECOND, -101111, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff37 = dbo.HumanizeDateTimeDiff(DATEADD(MICROSECOND, -1011111, SYSDATETIME())) 
GO
SELECT HumanizeDataTimeDiff38 = dbo.HumanizeDateTimeDiff(DATEADD(NANOSECOND, -100, SYSDATETIME()))
GO
/**************************************************************************************************************************************************************************************************
 *
 *       File Name Functions - Usually I've used FileInfo class, but that goes out to NTFS and various OS functions, very slow for massive work.
 *
/***************************************************************************************************************************************************************************************************/*/
CREATE OR ALTER FUNCTION FileNameExtension(@fullfilepath NVARCHAR(500)) RETURNS NVARCHAR(12) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.FileNameExtract].FileNameExtension;  
GO
CREATE OR ALTER FUNCTION FileNameWithoutExtension(@fullfilepath NVARCHAR(500)) RETURNS NVARCHAR(400)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.FileNameExtract].FileNameWithoutExtension;
GO
CREATE OR ALTER FUNCTION FileNameWithExtension(@fullfilepath NVARCHAR(500)) RETURNS NVARCHAR(500)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.FileNameExtract].FileNameWithExtension;  
GO
CREATE OR ALTER FUNCTION FileInDirectory(@fullfilepath NVARCHAR(500)) RETURNS NVARCHAR(500)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.FileNameExtract].FileInDirectory;  
GO
CREATE OR ALTER FUNCTION FileInFolder(@fullfilepath NVARCHAR(500)) RETURNS NVARCHAR(400)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.FileNameExtract].FileInFolder;  
GO
/**************************************************************************************************************************************************************************************************
 *
 *       File functions
 *
/***************************************************************************************************************************************************************************************************/*/
CREATE OR ALTER FUNCTION TempFilePath() RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.Files].TempFilePath;  
GO  
SELECT TempFilePath____________________________________________ = dbo.TempFilePath() --> C:\Users\~humphrej2\AppData\Local\Temp\tmpD28B.tmp
GO
/**************************************************************************************************************************************************************************************************
 *
 *       String tests for conditions, contents, meanings
 *
/***************************************************************************************************************************************************************************************************/*/
CREATE OR ALTER FUNCTION AnyOfTheseSAreAnyOfThoseS(@inputs NVARCHAR(MAX), @markers NVARCHAR(MAX), @sep NVARCHAR(MAX)) RETURNS BIT
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTest].AnyOfTheseSAreAnyOfThoseS;  
GO
SELECT AnyOfTheseSAreAnyOfThoseS___________________________ = dbo.AnyOfTheseSAreAnyOfThoseS('hi;there;', 'not;here;', ';') --> 0
SELECT AnyOfTheseSAreAnyOfThoseS____________________________ = dbo.AnyOfTheseSAreAnyOfThoseS('hi;there;', 'not;there;', ';') --> 1
GO
CREATE OR ALTER FUNCTION StartsWithS(@input NVARCHAR(MAX), @searchFor NVARCHAR(MAX)) RETURNS BIT
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTest].StartsWithS;  
GO
SELECT StartsWithS_______________________________________________________________ = dbo.StartsWithS('test', 't')
GO
CREATE OR ALTER FUNCTION EndsWithS(@input NVARCHAR(MAX), @searchFor NVARCHAR(MAX)) RETURNS BIT
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTest].EndsWithS;  
GO
SELECT EndsWithS______________  = dbo.EndsWithS('x', 'x') -- 1
GO
CREATE OR ALTER FUNCTION IsIP4(@input NVARCHAR(256)) RETURNS BIT
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTest].IsIP4;  
GO
SELECT IsIP4_______________________________________________________________ = dbo.IsIP4('10.10.10.218')
SELECT IsIP4_______________________________________________________________ = dbo.IsIP4('$(FQDN1)')
GO
CREATE OR ALTER FUNCTION LegalName(@input NVARCHAR(MAX), @rule NVARCHAR(MAX)) RETURNS BIT
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTest].LegalName;  
GO
SELECT LegalName_______________________________________________________________ = dbo.LegalName('HOSTNAMEUSUALLY\R12345678901234567', 'SQL Server Server Name')
GO

/**************************************************************************************************************************************************************************************************
 *
 *       T-SQL String Measure (which creates a new value, deterministically, but not reversibly)
 *
/***************************************************************************************************************************************************************************************************/*/
CREATE OR ALTER FUNCTION HowManyS(@input NVARCHAR(MAX), @marker NVARCHAR(MAX)) RETURNS INT
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringMeasure].HowManyS;  
GO  
SELECT HowManyS = dbo.HowManyS('hi!', '!')
SELECT HowManyS = dbo.HowManyS('Hell%%%%o', '%%')
SELECT HowManyS = dbo.HowManyS('Hell%%%o', '%%')
GO

CREATE OR ALTER FUNCTION HowManyX(@input NVARCHAR(MAX), @pattern NVARCHAR(MAX)) RETURNS INT
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringMeasure].HowManyX;  
GO  
SELECT HowManyX = dbo.HowManyX('Created for raiserror generator so that %s with %d would generate', '(%s|%d)') -- 2
GO

/**************************************************************************************************************************************************************************************************
 *
 *       T-SQL String Extractions (sort of a transformation)
 *
/***************************************************************************************************************************************************************************************************/*/
CREATE OR ALTER FUNCTION PieceNumberX(@input NVARCHAR(MAX), @pattern NVARCHAR(4000), @selectionindex INT) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringExtract].PieceNumberX;  
GO  
SELECT PieceNumberX_____________ = dbo.PieceNumberX('this is 100 times, or maybe 1503', '(\d+)', 1)      --> 2 rows match:100, matchat: 8, match:1503, matchat:28
SELECT PieceNumberX_____________ = dbo.PieceNumberX('this is 100 times, or maybe 1503', '(\d+)', 100) 
SELECT PieceNumberX_____________ = dbo.PieceNumberX('this is 100 times, or maybe 1503', '(\d+)', 2) 
GO
CREATE OR ALTER FUNCTION LastPieceX(@input NVARCHAR(MAX), @pattern NVARCHAR(4000)) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringExtract].LastPieceX;  
GO  
SELECT LastPieceX__________________ = dbo.LastPieceX('this is 100 times, or maybe 1503', '(\d+)') 
GO
CREATE OR ALTER FUNCTION GetFirstName(@FullName NVARCHAR(500)) RETURNS NVARCHAR(500) 
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringExtract].GetFirstName;  
GO  
SELECT GetFirstName = dbo.GetFirstName('Humphreys, Jeff')
GO
CREATE OR ALTER FUNCTION LeftOfS(@input NVARCHAR(MAX), @marker NVARCHAR(MAX)) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringExtract].LeftOfS;  
GO
SELECT LeftOfS_______________________________________________________________ = dbo.LeftOfS('Test\x', '\')
GO
CREATE OR ALTER FUNCTION LeftOfAnyC(@input NVARCHAR(MAX), @marker NVARCHAR(MAX)) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringExtract].LeftOfAnyC;  
GO
SELECT LeftOfAnyC_______________________________________________________________ = dbo.LeftOfAnyC('Test\x', '\')
GO
CREATE OR ALTER FUNCTION LeftOfNthS(@input NVARCHAR(MAX), @marker NVARCHAR(MAX), @n INT) RETURNS NVARCHAR(MAX) 
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringExtract].LeftOfNthS;  
GO
SELECT LeftOfNthS_______________________________________________________________ = dbo.LeftOfNthS('Test\x\', '\', 2)
GO
CREATE OR ALTER FUNCTION LeftMthOfNthS(@input NVARCHAR(MAX), @marker NVARCHAR(MAX), @n INT, @howmanyback INT) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringExtract].LeftMthOfNthS;  
GO
SELECT LeftMOfNth_______________________________________________________________ = dbo.LeftMthOfNthS('Test\x\', '\', 2, 2)
GO
CREATE OR ALTER FUNCTION RightOfAnyC(@input NVARCHAR(MAX), @marker NVARCHAR(MAX)) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringExtract].RightOfAnyC;  
GO
SELECT RightOfAnyC_______________________________________________________________ = dbo.RightOfAnyC('Test\x', '\')  -->x<--
GO
CREATE OR ALTER FUNCTION RightOfS(@input NVARCHAR(MAX), @marker NVARCHAR(MAX)) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringExtract].RightOfS;  
GO
SELECT RightOfS_______________________________________________________________ = dbo.RightOfS('Test\hx', '\h')  -->x<--
GO
CREATE OR ALTER FUNCTION FirstWordW(@input NVARCHAR(MAX)) RETURNS NVARCHAR(MAX)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringExtract].FirstWordW;  
GO
SELECT FirstWordW_______________________________________________________________ = dbo.FirstWordW('126.334.333 Hi!')
GO
CREATE OR ALTER FUNCTION FirstWordBeforeS(@input NVARCHAR(MAX), @marker NVARCHAR(MAX)) RETURNS NVARCHAR(MAX)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringExtract].FirstWordBeforeS;  
GO
SELECT FirstWordBeforeS_______________________________________________________________ = dbo.FirstWordBeforeS('$(FQDN2)', '.')
GO
CREATE OR ALTER FUNCTION FirstWordBeforeAnyC(@input NVARCHAR(MAX), @marker NVARCHAR(MAX)) RETURNS NVARCHAR(MAX)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringExtract].FirstWordBeforeAnyC;  
GO
SELECT FirstWordBeforeAnyC___________________________________________________________ = dbo.FirstWordBeforeAnyC('$(FQDN3)', '.\')
GO
CREATE OR ALTER FUNCTION EverythingAfterX(@input NVARCHAR(MAX), @marker NVARCHAR(MAX)) RETURNS NVARCHAR(MAX)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringExtract].EverythingAfterX;  
GO
SELECT EverythingAfterX_______________________________________________________________ = dbo.EverythingAfterX('$(FQDN2)', '.')
GO
/*
.Net SqlClient Data Provider: Msg 6572, Level 16, State 1, Procedure Mid, Line 557
More than one method, property or field was found with name 'Mid' in class 'MySQLCLRFunctions.StringExtract' in assembly 'MySQLCLRFunctions'. Overloaded methods, properties or fields are not supported.
.Net SqlClient Data Provider: Msg 4121, Level 16, State 1, Line 561
Cannot find either column "dbo" or the user-defined function or aggregate "dbo.Mid", or the name is ambiguous.
*/
CREATE OR ALTER FUNCTION Mid(@input NVARCHAR(MAX), @from INT, @to INT) RETURNS NVARCHAR(MAX)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringExtract].Mid;  
GO
SELECT Mid_______________________________________________________________ = dbo.Mid('this is my face', 2, 6)
GO
/**************************************************************************************************************************************************************************************************
 *
 *       Pivot substrings into rows and metadata.
 *
/***************************************************************************************************************************************************************************************************/*/
CREATE OR ALTER FUNCTION PiecesX(@input NVARCHAR(MAX), @pattern NVARCHAR(4000)) 
RETURNS TABLE (piece NVARCHAR(MAX), pieceorderno INT) 
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringPivot].PiecesX;  
GO  
SELECT ssore.piece, ssore.pieceorderno FROM PiecesX('this is 100 times, or maybe 1503', '(\d+)') AS ssore
GO
CREATE OR ALTER FUNCTION PiecesWithContextX(@input NVARCHAR(MAX), @regexsplitterpattern NVARCHAR(4000)) RETURNS TABLE (pieceorderno INT, previousPiece NVARCHAR(MAX), piece NVARCHAR(MAX), nextPiece NVARCHAR(MAX)) 
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringPivot].PiecesWithContextX;  
GO  
SELECT ssore.piece, ssore.pieceorderno FROM PiecesWithContextX('this is 100 times, or maybe 1503', '(\d+)') AS ssore
GO
CREATE OR ALTER FUNCTION PiecesWithMatchesX(@stringtoextractmatchesintorows NVARCHAR(MAX), @pattern NVARCHAR(4000)) RETURNS TABLE(pieceorderNo INT, previousPiece NVARCHAR(MAX)
, matchAtStartOfPiece NVARCHAR(MAX), piece NVARCHAR(MAX), matchAtEndOfPiece NVARCHAR(MAX), nextPiece NVARCHAR(MAX)) 
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringPivot].PiecesWithMatchesX;  
GO  
SELECT * FROM PiecesWithMatchesX('this is 100 times, or maybe 1503', '(\d+)') AS ssore
GO
CREATE OR ALTER FUNCTION MatchesX(@input NVARCHAR(MAX), @pattern NVARCHAR(4000)) RETURNS TABLE(matchorderno INT, capturedMatch NVARCHAR(MAX), capturedmatchstartsat INT) 
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringPivot].MatchesX;  
GO  
SELECT matchorderno, ssore.capturedMatch, ssore.capturedmatchstartsat FROM MatchesX('this is 100 times, or maybe 1503', '(\d+)') AS ssore
SELECT * FROM MatchesX('this is 100 times, or maybe 1503', '(\d+)') AS ssore
GO
CREATE OR ALTER FUNCTION NearX(@input NVARCHAR(MAX), @pattern NVARCHAR(4000)) RETURNS TABLE(matchorderno INT, [match] NVARCHAR(MAX), matchstartsat INT, matchcontextstartsat INT, matchcontextendsat INT, matchcontext NVARCHAR(MAX)) 
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringPivot].NearX;  
GO  
SELECT * FROM NearX('this is 100 times, or maybe emailed you 1503

Don''t forget to mailout', 'mail') AS ssore
GO
CREATE OR ALTER FUNCTION CapturesX(@input NVARCHAR(MAX), @pattern NVARCHAR(4000)) RETURNS TABLE(matchorderno INT, capturedMatch NVARCHAR(MAX), capturedmatchstartsat INT) 
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringPivot].CapturesX;  
GO  
SELECT * FROM CapturesX('div class="browsename"><span class="listname"><a href="/name/abbey">ABBEY</a></span> <span class="listgender">', '\<span class\="listname"\>\<a href=\"\/name\/(\w+)"') AS ssore
GO
CREATE OR ALTER FUNCTION KeyValuePairsWithMultiValuesS(@input NVARCHAR(MAX), @betweeneachkeyvaluepair NVARCHAR(4000), @betweenkeyandvalue NVARCHAR(4000), @betweensubvalues NVARCHAR(4000)) 
RETURNS TABLE(pieceorderNo INT, keystring NVARCHAR(MAX), valuestring NVARCHAR(MAX)) 
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringPivot].KeyValuePairsWithMultiValuesS;  
GO  
SELECT * FROM KeyValuePairsWithMultiValuesS('Edward => Ned, Ed, Eddy, Eddie
Henry => Harry, Hal
Jacob => Jake
', NULL, NULL, NULL) AS ssore
GO
/**************************************************************************************************************************************************************************************************
 *
 *       String decoding.
 *
/***************************************************************************************************************************************************************************************************/*/
CREATE OR ALTER FUNCTION RevealNonPrintables(@input NVARCHAR(MAX)) RETURNS NVARCHAR(MAX)                -->                                                                                                test<--
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringDecode].RevealNonPrintables;  
GO
SELECT RevealNonprintables_______________________________________________________________ = dbo.RevealNonPrintables('test'+ CHAR(10) + CHAR(13))     -->test1013                                                                                               <--
GO
/**************************************************************************************************************************************************************************************************
 *
 *       String reductions, actions that will always result in the same or shorter string.
 *
/***************************************************************************************************************************************************************************************************/*/
CREATE OR ALTER FUNCTION BlankOut(@input NVARCHAR(MAX), @blankanyofthese NVARCHAR(MAX), @sep NVARCHAR(MAX)) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringReduce].BlankOut;  
GO
CREATE OR ALTER FUNCTION RTrimAnyC(@input NVARCHAR(MAX), @trimAllOccFromRight NVARCHAR(MAX)) RETURNS NVARCHAR(MAX)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringReduce].RTrimAnyC;  
GO  
SELECT RTrimAnyC = CAST(dbo.RTrimAnyC('100.20000000', '0') AS DECIMAL(10,2))     --> 100.20
GO
CREATE OR ALTER FUNCTION TrimBrackets(@input NVARCHAR(MAX)) RETURNS NVARCHAR(MAX)                -->                                                                                                test<--
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringReduce].TrimBrackets;  
GO
SELECT TrimBrackets_______________________________________________________________ = dbo.TrimBrackets('[test]')-->test<--               dbo.TrimIfStartsWith(PingableAddress, '\032')
GO
CREATE OR ALTER FUNCTION LTrimIfStartsWithS(@input NVARCHAR(MAX), @marker NVARCHAR(MAX)) RETURNS NVARCHAR(MAX)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringReduce].LTrimIfStartsWithS; 
GO
SELECT LTrimIfStartsWithS_______________________________________________________________ = dbo.LTrimIfStartsWithS('$(FQDN4)', '\032')
GO
CREATE OR ALTER FUNCTION TrimEnd(@input NVARCHAR(MAX), @howmanycharactersofftheend INT) RETURNS NVARCHAR(MAX)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringReduce].TrimEnd; 
GO
SELECT TrimEnd_______________________________________________________________ = dbo.TrimEnd('$(FQDN5)', 1)
GO
/**************************************************************************************************************************************************************************************************
 *
 *       String custom reductions for more special use cases.
 *
/***************************************************************************************************************************************************************************************************/*/
CREATE OR ALTER FUNCTION TrimNormalizeStringInput(@input NVARCHAR(MAX)) RETURNS NVARCHAR(MAX)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringReduceCustomizations].TrimNormalizeStringInput; 
GO
SELECT TrimNormalizeStringInput_______________________________________________________________ = dbo.TrimNormalizeStringInput('   Hi   There! ')
GO
/**************************************************************************************************************************************************************************************************
 *
 *       String build outs, which always increase the size of a string.
 *
/***************************************************************************************************************************************************************************************************/*/
CREATE OR ALTER FUNCTION AppendWithSeparator(@input NVARCHAR(MAX), @newfield NVARCHAR(MAX), @sep NVARCHAR(MAX)) RETURNS NVARCHAR(MAX)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringBuildOut].AppendWithSeparator; 
GO
DECLARE @LineHeader VARCHAR(300) = dbo.AppendWithSeparator('', 'FirstField', ', ')
SELECT AppendWithSeparator_______________________________________________________________ = dbo.AppendWithSeparator('', 'FirstField', ', ')
SELECT AppendWithSeparator_______________________________________________________________ = dbo.AppendWithSeparator(@LineHeader, 'SecondField', ', ')
GO
CREATE OR ALTER FUNCTION AppendWithComma(@input NVARCHAR(MAX), @newfield NVARCHAR(MAX)) RETURNS NVARCHAR(MAX)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringBuildOut].AppendWithComma; 
GO
DECLARE @LineHeader VARCHAR(300) = dbo.AppendWithComma('', 'FirstField')
SELECT AppendWithComma_______________________________________________________________ = dbo.AppendWithComma('', 'FirstField')
SELECT AppendWithComma_______________________________________________________________ = dbo.AppendWithComma(@LineHeader, 'SecondField')
GO
CREATE OR ALTER FUNCTION AppendWithTab(@input NVARCHAR(MAX), @newfield NVARCHAR(MAX)) RETURNS NVARCHAR(MAX)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringBuildOut].AppendWithTab; 
GO
DECLARE @LineHeader VARCHAR(300) = dbo.AppendWithTab('', 'FirstField')
SELECT AppendWithTab_______________________________________________________________ = dbo.AppendWithTab('', 'FirstField')
SELECT AppendWithTab_______________________________________________________________ = dbo.AppendWithTab(@LineHeader, 'SecondField')
GO

/**************************************************************************************************************************************************************************************************
 *
 *       String formatting (which probably should include date humanization)
 *
/***************************************************************************************************************************************************************************************************/*/
CREATE OR ALTER FUNCTION LPad(@input NVARCHAR(MAX), @padToLen INT) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringFormat].LPad;  
GO
SELECT LPad_______________________________________________________________ = dbo.LPad('test', 100)
GO
CREATE OR ALTER FUNCTION RPad(@input NVARCHAR(MAX), @padToLen INT) RETURNS NVARCHAR(MAX)                -->                                                                                                test<--
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringFormat].RPad;  
GO
SELECT RPad_______________________________________________________________ = dbo.RPad('test', 100)     -->test                                                                                                <--
GO
CREATE OR ALTER FUNCTION Title(@input NVARCHAR(MAX)) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringFormat].Title; 
GO
SELECT [Input] = 'test', Title_______________________________________________________________ = dbo.Title('test')   
SELECT Title_______________________________________________________________ = dbo.Title('Abigail')    
GO
/**************************************************************************************************************************************************************************************************
 *
 *      General transformations
 *
/***************************************************************************************************************************************************************************************************/*/
CREATE OR ALTER FUNCTION ReplaceRecursiveS(@input NVARCHAR(MAX), @find NVARCHAR(MAX), @replacement NVARCHAR(MAX)) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransform].ReplaceRecursiveS; 
GO
SELECT ReplaceRecursiveS_____________________ = dbo.ReplaceRecursiveS('This is                a test  of the     emergency  ', '  ', '')    -->This is a test of the emergency <--
GO
CREATE OR ALTER FUNCTION ReplaceMatchX(@input NVARCHAR(MAX), @find NVARCHAR(MAX), @replacement NVARCHAR(MAX)) RETURNS NVARCHAR(MAX)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransform].ReplaceMatchX; 
GO
SELECT ReplaceMatchX________________ = dbo.ReplaceMatchX('SELECT * FROM $1.$10', '\$1~[0-9]', 'Merry')    -->  ????
GO
/**************************************************************************************************************************************************************************************************
 *
 *       T-SQL (Microsoft) Specific transformations
 *
/***************************************************************************************************************************************************************************************************/*/
CREATE OR ALTER FUNCTION RemoveSQLServerNameDelimiters(@input NVARCHAR(MAX)) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransformCustomizations].RemoveSQLServerNameDelimiters;  
GO
SELECT [Input] = '[Dave]', RemoveSQLServerNameDelimiters_____________ = dbo.RemoveSQLServerNameDelimiters('[Dave]')
GO
CREATE OR ALTER FUNCTION ExpandSQLParameterString(@sqlwithparametersembedded NVARCHAR(MAX), @paramno INT, @newvalue NVARCHAR(MAX)) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransformCustomizations].ExpandSQLParameterString;  
GO
CREATE OR ALTER FUNCTION ExpandSQLParameter(@sqlwithparametersembedded NVARCHAR(MAX), @paramno INT, @newvalue NVARCHAR(MAX)) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransformCustomizations].ExpandSQLParameter;  
GO
CREATE OR ALTER FUNCTION TrimSQL(@input NVARCHAR(MAX), @toSingleLine BIT, @dropFullLineComments BIT) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransformCustomizations].TrimSQL; 
GO

/*
    Buggy still.
*/
--CREATE OR ALTER FUNCTION BuildRaiserrorMessage(@input NVARCHAR(MAX), @Param1 VARCHAR(MAX) = NULL, @Param2 VARCHAR(MAX) = NULL, @Param3 VARCHAR(MAX) = NULL, @Param4 VARCHAR(MAX) = NULL, @Param5 VARCHAR(MAX) = NULL, @Param6 VARCHAR(MAX) = NULL, @Param7 VARCHAR(MAX) = NULL, @Param8 VARCHAR(MAX) = NULL, @Param9 VARCHAR(MAX) = NULL, @Param10 VARCHAR(MAX) = NULL) RETURNS NVARCHAR(MAX) 
--WITH RETURNS NULL ON NULL INPUT
--AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransformCustomizations].BuildRaiserrorMessage; 
--GO

SELECT name, principal_id, clr_name, permission_set, is_visible, create_date, modify_date, is_user_defined FROM sys.assemblies AS a WHERE name = '$(LibName)'
SELECT name, DATALENGTH(content) FROM sys.assembly_files AS af WHERE name = '$(LibName)'
SELECT object_id
     , am.assembly_id
     , assembly_class
     , assembly_method
     , null_on_null_input
     , execute_as_principal_id
     FROM sys.assembly_modules AS am JOIN sys.assemblies AS a ON a.assembly_id = am.assembly_id WHERE name = '$(LibName)'
SELECT * FROM sys.dm_clr_properties
-- version	v4.0.30319
-- SQL Server 2012 and newer, even though they are on CLR version 4.
-- .NET Framework 4.8 was released on 18 April 2019.