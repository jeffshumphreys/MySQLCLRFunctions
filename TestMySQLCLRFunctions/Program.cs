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
            output = MySQLCLRFunctions.StringTransformTSQLSpecific.RemoveSQLServerNameDelimiters($"{input}"); if (output == null) output = "{null}";
            Debug.Print($"MySQLCLRFunctions.StringTransform.RemoveSQLServerNameDelimiters(\"{input}\");=>{output}<=");

            input = @"
--===================================================================================================

-----------------------------------------------------------------------------------------------------
CREATE trigger [dbo].[JRS_apx_core_r_CT]
	on [dbo].[apx_core_r]
	for delete, insert, update
as
begin
	declare 
		@Operation nchar(1) = N'I'

	set nocount on

	-- Insert deletes and before updates into change table
	insert into jrs.apx_core_r_CT
		(Change_Operation,
		Change_Datetime,
		r_object_id,
		i_position)
	select
		N'D' as Change_Operation,
		sysdatetime() as Change_Datetime,
		r_object_id,
		i_position
	from Deleted

	if @@rowcount > 0
	begin
		set @Operation = N'U'
	end

	-- Insert inserts and after updates into change table
	insert into jrs.apx_core_r_CT 
		(Change_Operation,
		Change_Datetime,
		r_object_id,
		i_position)
	select
		@Operation as Change_Operation,
		sysdatetime() as Change_Datetime,
		r_object_id,
		i_position
	from Inserted
end
";
            output = MySQLCLRFunctions.StringTransformTSQLSpecific.StripDownSQLModule(input, toSingleLine:true, dropFullLineComments:true);
            Debug.Print($"MySQLCLRFunctions.StringTransformStripDownCustomizations.StripDownSQLModule(\"{input}\", toSingleLine:true, dropFullLineComments:true);=>{output}<=");
            output = MySQLCLRFunctions.StringTransformTSQLSpecific.StripDownSQLModule(input, toSingleLine: false, dropFullLineComments: false);
            Debug.Print($"MySQLCLRFunctions.StringTransformStripDownCustomizations.StripDownSQLModule(\"{input}\", toSingleLine:false, dropFullLineComments:false);=>{output}<=");
            input = @"
                SET NOCOUNT ON 
        BEGIN x end ";
            output = MySQLCLRFunctions.StringTransformTSQLSpecific.StripDownSQLModule(input, toSingleLine: true, dropFullLineComments: true);
            Debug.Print($"MySQLCLRFunctions.StringTransformStripDownCustomizations.StripDownSQLModule(\"{input}\", toSingleLine:true, dropFullLineComments:true);=>{output}<=");
        }
    }
}
