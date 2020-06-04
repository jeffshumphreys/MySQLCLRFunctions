:SETVAR svr "xxxx" --< You set these
:SETVAR db "yyyyy" --< You set these

:ON ERROR EXIT
SELECT @@servername, DB_NAME(), ORIGINAL_LOGIN()                 
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
DROP FUNCTION IF EXISTS ReadEventLog
DROP FUNCTION IF EXISTS Pieces
DROP FUNCTION IF EXISTS Matches
DROP FUNCTION IF EXISTS StringSplitOnRegEx
DROP FUNCTION IF EXISTS GetFirstName
DROP FUNCTION IF EXISTS HumanizeDateTimeDiff
DROP FUNCTION IF EXISTS GreatestOf2DateTimes
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
/*
    Embedded in Table Def!!!
*/
--DROP FUNCTION IF EXISTS HowMany

--DROP ASSEMBLY IF EXISTS MySQLCLRFunctions
--Msg 10327, Level 14, State 1, Line 44
--CREATE ASSEMBLY for assembly 'MySQLCLRFunctions' failed because assembly 'MySQLCLRFunctions' is not trusted. The assembly is trusted when either of the following is true: the assembly is signed with a certificate or an asymmetric key that has a corresponding login with UNSAFE ASSEMBLY permission, or the assembly is trusted using sp_add_trusted_assembly.

--.Net SqlClient Data Provider: Msg 6285, Level 16, State 1, Line 57
--ALTER ASSEMBLY failed because the source assembly is, according to MVID, identical to an assembly that is already registered under the name "MySQLCLRFunctions".

ALTER ASSEMBLY MySQLCLRFunctions from 'C:\Users\humphrej2\Source\Repos\jeffshumphreys\MySQLCLRFunctions\bin\Release\MySQLCLRFunctions.dll' WITH PERMISSION_SET = UNSAFE, UNCHECKED DATA
GO
DROP FUNCTION IF EXISTS Pieces
GO
CREATE FUNCTION Pieces(@stringtosplitintopieces NVARCHAR(MAX), @regexsplitterpattern NVARCHAR(4000)) RETURNS TABLE (piece NVARCHAR(MAX), pieceorderno INT) AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransform].Pieces;  
GO  
SELECT ssore.piece, ssore.pieceorderno FROM Pieces('this is 100 times, or maybe 1503', '(\d+)') AS ssore
DROP FUNCTION IF EXISTS Matches
GO
CREATE FUNCTION Matches(@stringtoextractmatchesintorows NVARCHAR(MAX), @regexcapturepattern NVARCHAR(4000)) RETURNS TABLE(match NVARCHAR(MAX), matchat INT) AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransform].Matches;  
GO  
SELECT ssore.match, ssore.matchat FROM Matches('this is 100 times, or maybe 1503', '(\d+)') AS ssore
DROP FUNCTION IF EXISTS VarBin2Hex
GO
CREATE FUNCTION VarBin2Hex(@InputAsVarBin VARBINARY(MAX)) RETURNS NVARCHAR(MAX) AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.Adaptors].VarBin2Hex;  
GO  
DROP FUNCTION IF EXISTS Ping
GO
CREATE FUNCTION Ping(@Machine NVARCHAR(257)) RETURNS BIT AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.NetworkTest].Ping;  
GO  
SELECT Ping = dbo.Ping('EDWPROD') 
DROP FUNCTION IF EXISTS PingGetAddress
GO
CREATE FUNCTION PingGetAddress(@Machine NVARCHAR(257)) RETURNS NVARCHAR(120) AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.NetworkCollect].PingGetAddress;  
GO  
SELECT PingGetAddress = dbo.PingGetAddress('EDWPROD') 
DROP FUNCTION IF EXISTS PingGetReturnBuffer
GO
CREATE FUNCTION PingGetReturnBuffer(@Machine NVARCHAR(257)) RETURNS VARBINARY(200) AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.NetworkCollect].PingGetReturnBuffer;  
GO  
SELECT PingGetReturnBuffer = CAST(dbo.PingGetReturnBuffer('EDWPROD') AS VARCHAR(400))
DROP FUNCTION IF EXISTS GetHostNames
GO
CREATE FUNCTION GetHostNames(@Machine NVARCHAR(257)) RETURNS NVARCHAR(600) AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.NetworkCollect].GetHostNames;  
GO  
--SELECT GetHostNames = CAST(dbo.GetHostNames('10.10.11.14') AS VARCHAR(400))
DROP FUNCTION IF EXISTS GetHostAliases
GO
CREATE FUNCTION GetHostAliases(@Machine NVARCHAR(257)) RETURNS NVARCHAR(600) AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.NetworkCollect].GetHostAliases;  
GO  
SELECT GetHostAliases = CAST(dbo.GetHostAliases('EDWPROD') AS VARCHAR(400))
DROP FUNCTION IF EXISTS GetHostRealName
GO
CREATE FUNCTION GetHostRealName(@Machine NVARCHAR(257)) RETURNS NVARCHAR(600) AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.NetworkCollect].GetHostRealName;  
GO  
SELECT GetHostRealName = dbo.GetHostRealName('STANDARDCOSTPRD\CA_PROD')
SELECT GetHostRealName = dbo.GetHostRealName('STANDARDCOSTPRD')
DROP FUNCTION IF EXISTS GetFirstName
GO
CREATE FUNCTION GetFirstName(@FullName NVARCHAR(500)) RETURNS NVARCHAR(500) AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringExtract].GetFirstName;  
GO  
SELECT GetFirstName = dbo.GetFirstName('Humphreys, Jeff')
DROP FUNCTION IF EXISTS GreatestOf2DateTimes
GO
CREATE FUNCTION GreatestOf2DateTimes(@d1 DATETIME2, @d2 DATETIME2) RETURNS DATETIME AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.Compares].GreatestOf2DateTimes;  
GO  
SELECT GreatestOf2DateTimes = dbo.GreatestOf2DateTimes(GETDATE(), DATEADD(DAY, 1, GETDATE()))
DROP FUNCTION IF EXISTS RTrimChar
GO
CREATE FUNCTION RTrimChar(@input NVARCHAR(MAX), @trimAllOccFromRight NVARCHAR(MAX)) RETURNS NVARCHAR(MAX) AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransform].RTrimChar;  
GO  
SELECT RTrimChar = CAST(dbo.RTrimChar('100.20000000', '0') AS DECIMAL(10,2))
DROP FUNCTION IF EXISTS Beep
GO
CREATE FUNCTION Beep(@frequencyHz INT, @durationMs INT) RETURNS INT AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.Environmental].Beep;  
GO  
SELECT Beep = dbo.Beep(400, 500)
DROP FUNCTION IF EXISTS BeepStandard
GO
CREATE FUNCTION BeepStandard() RETURNS INT AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.Environmental].BeepStandard;  
GO  
SELECT BeepStandard = dbo.BeepStandard()
DROP FUNCTION IF EXISTS HumanizeDateTimeDiff
GO
CREATE FUNCTION HumanizeDateTimeDiff(@from DATETIME2) RETURNS NVARCHAR(500) AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.Humanization].HumanizeDateTimeDiff;  
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

DROP FUNCTION IF EXISTS LPad
GO
CREATE FUNCTION LPad(@input NVARCHAR(MAX), @padToLen INT) RETURNS NVARCHAR(MAX) AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransform].LPad;  
GO
SELECT LPad_______________________________________________________________ = dbo.LPad('test', 100)

DROP FUNCTION IF EXISTS RPad
GO
CREATE FUNCTION RPad(@input NVARCHAR(MAX), @padToLen INT) RETURNS NVARCHAR(MAX) AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransform].RPad;  
GO
SELECT RPad_______________________________________________________________ = dbo.RPad('test', 100)

DROP FUNCTION IF EXISTS LeftOf
GO
CREATE FUNCTION LeftOf(@input NVARCHAR(MAX), @marker NVARCHAR(MAX)) RETURNS NVARCHAR(MAX) AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringExtract].LeftOf;  
GO
SELECT LeftOf_______________________________________________________________ = dbo.LeftOf('Test\x', '\')

DROP FUNCTION IF EXISTS LeftOfNth
GO
CREATE FUNCTION LeftOfNth(@input NVARCHAR(MAX), @marker NVARCHAR(MAX), @n INT) RETURNS NVARCHAR(MAX) AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringExtract].LeftOfNth;  
GO
SELECT LeftOfNth_______________________________________________________________ = dbo.LeftOfNth('Test\x\', '\', 2)

DROP FUNCTION IF EXISTS LeftMOfNth
GO
CREATE FUNCTION LeftMOfNth(@input NVARCHAR(MAX), @marker NVARCHAR(MAX), @n INT, @howmanyback INT) RETURNS NVARCHAR(MAX) AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringExtract].LeftMOfNth;  
GO
SELECT LeftMOfNth_______________________________________________________________ = dbo.LeftMOfNth('Test\x\', '\', 2, 2)

DROP FUNCTION IF EXISTS RightOf
GO
CREATE FUNCTION RightOf(@input NVARCHAR(MAX), @marker NVARCHAR(MAX)) RETURNS NVARCHAR(MAX) AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringExtract].RightOf;  
GO
SELECT RightOf_______________________________________________________________ = dbo.RightOf('Test\x', '\')

DROP FUNCTION IF EXISTS StartsWith
GO
CREATE FUNCTION StartsWith(@input NVARCHAR(MAX), @searchFor NVARCHAR(MAX)) RETURNS BIT AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTest].StartsWith;  
GO
SELECT StartsWith_______________________________________________________________ = dbo.StartsWith('test', 't')

DROP FUNCTION IF EXISTS EndsWith
GO
CREATE FUNCTION EndsWith(@input NVARCHAR(MAX), @searchFor NVARCHAR(MAX)) RETURNS BIT AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTest].EndsWith;  
GO
SELECT EndsWith_______________________________________________________________ = dbo.EndsWith('test', 't')

DROP FUNCTION IF EXISTS LegalName
GO
CREATE FUNCTION LegalName(@input NVARCHAR(MAX), @searchFor NVARCHAR(MAX)) RETURNS BIT AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTest].LegalName;  
GO
SELECT LegalName_______________________________________________________________ = dbo.LegalName('HOSTNAMEUSUALLY\R12345678901234567', 'SQL Server Server Name')

DROP FUNCTION IF EXISTS FileNameExtension
GO
CREATE FUNCTION FileNameExtension(@fullfilepath NVARCHAR(500)) RETURNS NVARCHAR(12) AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.FileNameExtract].FileNameExtension;  
GO

DROP FUNCTION IF EXISTS FileNameWithoutExtension
GO
CREATE FUNCTION FileNameWithoutExtension(@fullfilepath NVARCHAR(500)) RETURNS NVARCHAR(400)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.FileNameExtract].FileNameWithoutExtension;
GO

DROP FUNCTION IF EXISTS FileNameWithExtension
GO
CREATE FUNCTION FileNameWithExtension(@fullfilepath NVARCHAR(500)) RETURNS NVARCHAR(500)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.FileNameExtract].FileNameWithExtension;  
GO

DROP FUNCTION IF EXISTS FileInDirectory
GO
CREATE FUNCTION FileInDirectory(@fullfilepath NVARCHAR(500)) RETURNS NVARCHAR(500)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.FileNameExtract].FileInDirectory;  
GO

DROP FUNCTION IF EXISTS FileInFolder
GO
CREATE FUNCTION FileInFolder(@fullfilepath NVARCHAR(500)) RETURNS NVARCHAR(400)
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.FileNameExtract].FileInFolder;  
GO

DROP FUNCTION IF EXISTS BlankOut
GO
CREATE FUNCTION BlankOut(@input NVARCHAR(MAX), @blankanyofthese NVARCHAR(MAX), @sep NVARCHAR(MAX)) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransform].BlankOut;  
GO

DROP FUNCTION IF EXISTS RemoveSQLServerNameDelimiters
GO
CREATE FUNCTION RemoveSQLServerNameDelimiters(@input NVARCHAR(MAX)) RETURNS NVARCHAR(MAX) 
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTransform].RemoveSQLServerNameDelimiters;  
GO

DROP FUNCTION IF EXISTS AnyOfTheseAreAnyOfThose
GO
CREATE FUNCTION AnyOfTheseAreAnyOfThose(@inputstrings NVARCHAR(MAX), @stringsitmightcontain NVARCHAR(MAX), @sep NVARCHAR(MAX)) RETURNS BIT
WITH RETURNS NULL ON NULL INPUT
AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringTest].AnyOfTheseAreAnyOfThose;  
GO

DROP FUNCTION IF EXISTS HowMany
GO
CREATE FUNCTION HowMany(@input NVARCHAR(MAX), @howManyOfThese NVARCHAR(MAX)) RETURNS INT AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.StringMeasure].HowMany;  
GO  
SELECT HowMany = dbo.HowMany('hi!', '!')
SELECT HowMany = dbo.HowMany('Hell%%%%o', '%%')
SELECT HowMany = dbo.HowMany('Hell%%%o', '%%')
GO
DROP FUNCTION IF EXISTS TempFilePath
GO
CREATE FUNCTION TempFilePath() RETURNS NVARCHAR(MAX) AS EXTERNAL NAME MySQLCLRFunctions.[MySQLCLRFunctions.Files].TempFilePath;  
GO  
SELECT dbo.TempFilePath()