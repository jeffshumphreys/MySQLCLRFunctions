-- Time to execute: 5 sec
:SETVAR svr "xxxx"
:SETVAR db "yyyy"
USE $(db)
:ON ERROR EXIT
SELECT ThisServer = @@servername, ThisDatabase = DB_NAME(), ThisUser = ORIGINAL_LOGIN()                 
IF DB_NAME() <> '$(db)' THROW 50000, 'WRONG DATABASE', 0
IF @@servername <> '$(svr)' THROW 50001, 'WRONG SERVER', 0
GO
-- If anything changes, drop and recreate
:ON ERROR IGNORE
DROP FUNCTION IF EXISTS GetHostRealName
DROP FUNCTION IF EXISTS GetHostAliases
DROP FUNCTION IF EXISTS GetHostNames
DROP FUNCTION IF EXISTS PingGetReturnBuffer
DROP FUNCTION IF EXISTS PingGetAddress
DROP FUNCTION IF EXISTS Ping
DROP FUNCTION IF EXISTS SpreadHex
DROP FUNCTION IF EXISTS VarBin2Hex
DROP FUNCTION IF EXISTS ADDateTimeString2DateTime
DROP FUNCTION IF EXISTS ReadEventLog
DROP FUNCTION IF EXISTS Pieces
DROP FUNCTION IF EXISTS PiecesWithContext
DROP FUNCTION IF EXISTS PieceNumber
DROP FUNCTION IF EXISTS LastPiece
DROP FUNCTION IF EXISTS Matches
DROP FUNCTION IF EXISTS StringSplitOnRegEx
DROP FUNCTION IF EXISTS GetFirstName
DROP FUNCTION IF EXISTS HumanizeDateTimeDiff
DROP FUNCTION IF EXISTS Max2DateTimes
DROP FUNCTION IF EXISTS Max3DateTimes
DROP FUNCTION IF EXISTS Max4DateTimes
DROP FUNCTION IF EXISTS Beep
DROP FUNCTION IF EXISTS BeepStandard
DROP FUNCTION IF EXISTS HowMany
DROP FUNCTION IF EXISTS RTrimChar
DROP FUNCTION IF EXISTS RPad
DROP FUNCTION IF EXISTS RPadChar
DROP FUNCTION IF EXISTS LPad
DROP FUNCTION IF EXISTS LPadChar
DROP FUNCTION IF EXISTS StartsWith
DROP FUNCTION IF EXISTS EndsWith
DROP FUNCTION IF EXISTS LegalName
DROP FUNCTION IF EXISTS LeftOf
DROP FUNCTION IF EXISTS LeftOfAny
DROP FUNCTION IF EXISTS LeftOfNth
DROP FUNCTION IF EXISTS LeftMOfNth
DROP FUNCTION IF EXISTS RightOf
DROP FUNCTION IF EXISTS RightOfAny
DROP FUNCTION IF EXISTS BlankOut
DROP FUNCTION IF EXISTS RemoveSQLServerNameDelimiters
DROP FUNCTION IF EXISTS FileNameExtension
DROP FUNCTION IF EXISTS FileNameWithoutExtension
DROP FUNCTION IF EXISTS FileNameWithExtension
DROP FUNCTION IF EXISTS FileInDirectory
DROP FUNCTION IF EXISTS FileInFolder
DROP FUNCTION IF EXISTS AnyOfTheseAreAnyOfThose
DROP FUNCTION IF EXISTS TempFilePath
DROP FUNCTION IF EXISTS StripDownSQLModule
DROP FUNCTION IF EXISTS ReplaceRecursive
DROP FUNCTION IF EXISTS ReplaceMatch
DROP FUNCTION IF EXISTS ExpandSQLParameterString
DROP FUNCTION IF EXISTS ExpandSQLParameter
DROP FUNCTION IF EXISTS BuildRaiserrorMessage
DROP FUNCTION IF EXISTS RevealNonPrintables
DROP FUNCTION IF EXISTS StripBracketing
DROP FUNCTION IF EXISTS FirstWord
/*
    Embedded in Table Def!!!
*/
--DROP FUNCTION IF EXISTS HowMany

--DROP ASSEMBLY IF EXISTS MySQLCLRFunctions
--Msg 10327, Level 14, State 1, Line 44
--CREATE ASSEMBLY for assembly 'MySQLCLRFunctions' failed because assembly 'MySQLCLRFunctions' is not trusted. The assembly is trusted when either of the following is true: the assembly is signed with a certificate or an asymmetric key that has a corresponding login with UNSAFE ASSEMBLY permission, or the assembly is trusted using sp_add_trusted_assembly.

--.Net SqlClient Data Provider: Msg 6285, Level 16, State 1, Line 57
--ALTER ASSEMBLY failed because the source assembly is, according to MVID, identical to an assembly that is already registered under the name "MySQLCLRFunctions".

BEGIN TRY
    ALTER ASSEMBLY MySQLCLRFunctions from 'C:\Users\humphrej2\Source\Repos\jeffshumphreys\MySQLCLRFunctions\bin\Release\MySQLCLRFunctions.dll' WITH PERMISSION_SET = UNSAFE, UNCHECKED DATA
END TRY
BEGIN CATCH
    IF ERROR_NUMBER() = 6285 PRINT 'MVID? Says assembly identical'
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
SELECT Ping = dbo.Ping('EDWPROD') 
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
SELECT PingGetAddress = dbo.PingGetAddress('EDWPROD')                                         -->fd85:eb7f:dad9:7777::a0a:b0e
GO
CREATE OR ALTER FUNCTION PingGetReturnBuffer(@Machine NVARCHAR(257)) RETURNS VARBINARY(200) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.NetworkCollect].PingGetReturnBuffer;  
GO  
SELECT PingGetReturnBuffer = CAST(dbo.PingGetReturnBuffer('EDWPROD') AS VARCHAR(400))        -->abcdefghijklmnopqrstuvwabcdefghi<--
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
SELECT GetHostAliases = CAST(dbo.GetHostAliases('EDWPROD') AS VARCHAR(400))
GO
CREATE OR ALTER FUNCTION GetHostRealName(@Machine NVARCHAR(257)) RETURNS NVARCHAR(600) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.NetworkCollect].GetHostRealName;  
GO  
SELECT GetHostRealName = dbo.GetHostRealName('STANDARDCOSTPRD\CA_PROD')
SELECT GetHostRealName = dbo.GetHostRealName('STANDARDCOSTPRD')
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
CREATE OR ALTER FUNCTION HumanizeDateTimeDiff(@from DATETIME2) RETURNS NVARCHAR(500)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.Humanization].HumanizeDateTimeDiff;  
GO  
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(SYSDATETIME()) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(YEAR, -1, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(YEAR, -10, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(QUARTER, -1, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(QUARTER, -2, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(MONTH, -1, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(MONTH, -2, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(WEEK, -1, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(WEEK, -2, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(WEEKDAY, -1, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(DAY, -1, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(DAY, -2, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(DAY, -3, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(HOUR, -12, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(HOUR, -11, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(HOUR, -9, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(HOUR, -5, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(HOUR, -3, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(HOUR, -2, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(HOUR, -1, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(MINUTE, -1, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(MINUTE, -11, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(SECOND, -1, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(SECOND, -2, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(SECOND, -31, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(SECOND, -59, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(SECOND, -(11*24+1), SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(SECOND, -(11*24-1), SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(SECOND, -(11*23-1), SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(SECOND, -(23*23-1), SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(MILLISECOND, -1, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(MICROSECOND, -1, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(MICROSECOND, -101, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(MICROSECOND, -1011, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(MICROSECOND, -10111, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(MICROSECOND, -101111, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(MICROSECOND, -1011111, SYSDATETIME())) UNION ALL
SELECT HumanizeDataTimeDiff = dbo.HumanizeDateTimeDiff(DATEADD(NANOSECOND, -100, SYSDATETIME()))
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
SELECT dbo.TempFilePath() --> C:\Users\~humphrej2\AppData\Local\Temp\tmpD28B.tmp
GO
/**************************************************************************************************************************************************************************************************
 *
 *       String tests for conditions, contents, meanings
 *
/***************************************************************************************************************************************************************************************************/*/
CREATE OR ALTER FUNCTION AnyOfTheseAreAnyOfThose(@inputstrings NVARCHAR(MAX), @stringsitmightcontain NVARCHAR(MAX), @sep NVARCHAR(MAX)) RETURNS BIT
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTest].AnyOfTheseAreAnyOfThose;  
GO
SELECT dbo.AnyOfTheseAreAnyOfThose('hi;there;', 'not;here;', ';') --> 0
SELECT dbo.AnyOfTheseAreAnyOfThose('hi;there;', 'not;there;', ';') --> 1
GO
CREATE OR ALTER FUNCTION StartsWith(@input NVARCHAR(MAX), @searchFor NVARCHAR(MAX)) RETURNS BIT
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTest].StartsWith;  
GO
SELECT StartsWith_______________________________________________________________ = dbo.StartsWith('test', 't')
GO
CREATE OR ALTER FUNCTION EndsWith(@input NVARCHAR(MAX), @searchFor NVARCHAR(MAX)) RETURNS BIT
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTest].EndsWith;  
GO
SELECT dbo.EndsWith('x', 'x') -- 1
GO
CREATE OR ALTER FUNCTION LegalName(@input NVARCHAR(MAX), @searchFor NVARCHAR(MAX)) RETURNS BIT
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTest].LegalName;  
GO
SELECT LegalName_______________________________________________________________ = dbo.LegalName('HOSTNAMEUSUALLY\R12345678901234567', 'SQL Server Server Name')
GO
CREATE OR ALTER FUNCTION FirstWord(@input NVARCHAR(MAX)) RETURNS NVARCHAR(MAX)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTest].FirstWord;  
GO
SELECT FirstWord_______________________________________________________________ = dbo.FirstWord('126.334.333 Hi!')
GO

/**************************************************************************************************************************************************************************************************
 *
 *       T-SQL String Measure (which creates a new value, deterministically, but not reversibly)
 *
/***************************************************************************************************************************************************************************************************/*/
CREATE OR ALTER FUNCTION HowMany(@input NVARCHAR(MAX), @howManyOfThese NVARCHAR(MAX)) RETURNS INT
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringMeasure].HowMany;  
GO  
SELECT HowMany = dbo.HowMany('hi!', '!')
SELECT HowMany = dbo.HowMany('Hell%%%%o', '%%')
SELECT HowMany = dbo.HowMany('Hell%%%o', '%%')
GO

/**************************************************************************************************************************************************************************************************
 *
 *       T-SQL String Extractions (sort of a transformation)
 *
/***************************************************************************************************************************************************************************************************/*/
CREATE OR ALTER FUNCTION GetFirstName(@FullName NVARCHAR(500)) RETURNS NVARCHAR(500) 
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringExtract].GetFirstName;  
GO  
SELECT GetFirstName = dbo.GetFirstName('Humphreys, Jeff')
GO
CREATE OR ALTER FUNCTION LeftOf(@input NVARCHAR(MAX), @marker NVARCHAR(MAX)) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringExtract].LeftOf;  
GO
SELECT LeftOf_______________________________________________________________ = dbo.LeftOf('Test\x', '\')
GO
CREATE OR ALTER FUNCTION LeftOfNth(@input NVARCHAR(MAX), @marker NVARCHAR(MAX), @n INT) RETURNS NVARCHAR(MAX) 
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringExtract].LeftOfNth;  
GO
SELECT LeftOfNth_______________________________________________________________ = dbo.LeftOfNth('Test\x\', '\', 2)
GO
CREATE OR ALTER FUNCTION LeftMOfNth(@input NVARCHAR(MAX), @marker NVARCHAR(MAX), @n INT, @howmanyback INT) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringExtract].LeftMOfNth;  
GO
SELECT LeftMOfNth_______________________________________________________________ = dbo.LeftMOfNth('Test\x\', '\', 2, 2)
GO
CREATE OR ALTER FUNCTION RightOf(@input NVARCHAR(MAX), @marker NVARCHAR(MAX)) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringExtract].RightOf;  
GO
SELECT RightOf_______________________________________________________________ = dbo.RightOf('Test\x', '\')  -->x<--
GO
/**************************************************************************************************************************************************************************************************
 *
 *       T-SQL String transformations
 *
/***************************************************************************************************************************************************************************************************/*/
CREATE OR ALTER FUNCTION Pieces(@stringtosplitintopieces NVARCHAR(MAX), @regexsplitterpattern NVARCHAR(4000)) RETURNS TABLE (piece NVARCHAR(MAX), pieceorderno INT) 
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransform].Pieces;  
GO  
SELECT ssore.piece, ssore.pieceorderno FROM Pieces('this is 100 times, or maybe 1503', '(\d+)') AS ssore
GO
CREATE OR ALTER FUNCTION PiecesWithContext(@stringtosplitintopieces NVARCHAR(MAX), @regexsplitterpattern NVARCHAR(4000)) RETURNS TABLE (pieceorderno INT, previousPiece NVARCHAR(MAX), piece NVARCHAR(MAX), nextPiece NVARCHAR(MAX)) 
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransform].PiecesWithContext;  
GO  
SELECT ssore.piece, ssore.pieceorderno FROM PiecesWithContext('this is 100 times, or maybe 1503', '(\d+)') AS ssore
GO
CREATE OR ALTER FUNCTION PieceNumber(@stringtosplitintopieces NVARCHAR(MAX), @regexsplitterpattern NVARCHAR(4000), @piecenumbertoreturn INT) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransform].PieceNumber;  
GO  
SELECT dbo.PieceNumber('this is 100 times, or maybe 1503', '(\d+)', 1) AS PieceNumber1     --> 2 rows match:100, matchat: 8, match:1503, matchat:28
SELECT dbo.PieceNumber('this is 100 times, or maybe 1503', '(\d+)', 100) AS PieceNumber1
SELECT dbo.PieceNumber('this is 100 times, or maybe 1503', '(\d+)', 2) AS PieceNumber1
GO
CREATE OR ALTER FUNCTION LastPiece(@stringtosplitintopieces NVARCHAR(MAX), @regexsplitterpattern NVARCHAR(4000)) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransform].LastPiece;  
GO  
SELECT dbo.LastPiece('this is 100 times, or maybe 1503', '(\d+)') AS LastPiecex
GO
CREATE OR ALTER FUNCTION Matches(@stringtoextractmatchesintorows NVARCHAR(MAX), @regexcapturepattern NVARCHAR(4000)) RETURNS TABLE(match NVARCHAR(MAX), matchat INT) 
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransform].Matches;  
GO  
SELECT ssore.match, ssore.matchat FROM Matches('this is 100 times, or maybe 1503', '(\d+)') AS ssore
GO
CREATE OR ALTER FUNCTION RTrimChar(@input NVARCHAR(MAX), @trimAllOccFromRight NVARCHAR(MAX)) RETURNS NVARCHAR(MAX)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransform].RTrimChar;  
GO  
SELECT RTrimChar = CAST(dbo.RTrimChar('100.20000000', '0') AS DECIMAL(10,2))     --> 100.20
GO
CREATE OR ALTER FUNCTION BlankOut(@input NVARCHAR(MAX), @blankanyofthese NVARCHAR(MAX), @sep NVARCHAR(MAX)) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransform].BlankOut;  
GO
CREATE OR ALTER FUNCTION ReplaceRecursive(@input NVARCHAR(MAX), @find NVARCHAR(MAX), @replacement NVARCHAR(MAX)) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransform].ReplaceRecursive; 
GO
SELECT dbo.ReplaceRecursive('This is                a test  of the     emergency  ', '  ', ' ')    -->This is a test of the emergency <--
GO
CREATE OR ALTER FUNCTION ReplaceMatch(@input NVARCHAR(MAX), @find NVARCHAR(MAX), @replacement NVARCHAR(MAX)) RETURNS NVARCHAR(MAX)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransform].ReplaceMatch; 
GO
SELECT dbo.ReplaceMatch('SELECT * FROM $1.$10', '\$1~[0-9]', 'Merry')    -->  ????
GO
CREATE OR ALTER FUNCTION LPad(@input NVARCHAR(MAX), @padToLen INT) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransform].LPad;  
GO
SELECT LPad_______________________________________________________________ = dbo.LPad('test', 100)
GO
CREATE OR ALTER FUNCTION RPad(@input NVARCHAR(MAX), @padToLen INT) RETURNS NVARCHAR(MAX)                -->                                                                                                test<--
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransform].RPad;  
GO
SELECT RPad_______________________________________________________________ = dbo.RPad('test', 100)     -->test                                                                                                <--
GO
CREATE OR ALTER FUNCTION RevealNonPrintables(@input NVARCHAR(MAX)) RETURNS NVARCHAR(MAX)                -->                                                                                                test<--
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransform].RevealNonPrintables;  
GO
SELECT RevealNonprintables_______________________________________________________________ = dbo.RevealNonPrintables('test'+ CHAR(10) + CHAR(13))     -->test1013                                                                                               <--
GO
CREATE OR ALTER FUNCTION StripBracketing(@input NVARCHAR(MAX)) RETURNS NVARCHAR(MAX)                -->                                                                                                test<--
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransform].StripBracketing;  
GO
SELECT StripBracketing_______________________________________________________________ = dbo.StripBracketing('[test]')-->test<--
GO
/**************************************************************************************************************************************************************************************************
 *
 *       T-SQL (Microsoft) Specific transformations
 *
/***************************************************************************************************************************************************************************************************/*/
CREATE OR ALTER FUNCTION RemoveSQLServerNameDelimiters(@input NVARCHAR(MAX)) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransformTSQLSpecific].RemoveSQLServerNameDelimiters;  
GO
CREATE OR ALTER FUNCTION ExpandSQLParameterString(@sqlwithparametersembedded NVARCHAR(MAX), @paramno INT, @newvalue NVARCHAR(MAX)) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransformTSQLSpecific].ExpandSQLParameterString;  
GO
CREATE OR ALTER FUNCTION ExpandSQLParameter(@sqlwithparametersembedded NVARCHAR(MAX), @paramno INT, @newvalue NVARCHAR(MAX)) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransformTSQLSpecific].ExpandSQLParameter;  
GO
CREATE OR ALTER FUNCTION StripDownSQLModule(@input NVARCHAR(MAX), @toSingleLine BIT, @dropFullLineComments BIT) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransformTSQLSpecific].StripDownSQLModule; 
GO

/*
    Buggy still.
*/
--CREATE OR ALTER FUNCTION BuildRaiserrorMessage(@input NVARCHAR(MAX), @Param1 VARCHAR(MAX) = NULL, @Param2 VARCHAR(MAX) = NULL, @Param3 VARCHAR(MAX) = NULL, @Param4 VARCHAR(MAX) = NULL, @Param5 VARCHAR(MAX) = NULL, @Param6 VARCHAR(MAX) = NULL, @Param7 VARCHAR(MAX) = NULL, @Param8 VARCHAR(MAX) = NULL, @Param9 VARCHAR(MAX) = NULL, @Param10 VARCHAR(MAX) = NULL) RETURNS NVARCHAR(MAX) 
--WITH RETURNS NULL ON NULL INPUT
--AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransformTSQLSpecific].BuildRaiserrorMessage; 
--GO

