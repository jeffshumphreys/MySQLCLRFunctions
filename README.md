# MySQLCLRFunctions
## Free SQLCLR functions tested on SQL Server 2019

These are simple functions I've written in order to avoid spending $500 for a two year license (https://sqlsharp.com/full/) of SQL#. For one, the product hasn't been updated in 2 years, and that's not very cool for a pay product.

FYI, warning, buyer beware on my stuff: I use UNSAFE assemblies, because half the reason to use SQLCLR is to get to functions that are not in SQL Server already, which is where the safe stuff is.

Another bug I have with SQL# is that their names are overly precious.  They will name a function SQL#.string_Is_BEGINNING_WITH whereas I call it StartsWith, which is not coincidentally the same as the C# method.  I try to align my names with C# when the functionality aligns.  Less to remember.

I also have less arguments on functions than SQL#.  SQL Server does not support optional arguments on functions, and so these are annoying to have to fill out. Also, I forget what they are supposed to be, and I don't recall their documentation telling me if using DEFAULT will work.
Rather than every possible argument under the sun, I do it the old fashion way: I make a new function.
For example, hypothetically, I would do this:
<li> CompareThese(a,b) </li>
<li> BestOf3(a,b,3) </li>
<li> BestOf4(a,b,c,d) </li>
<li> Or make an agg or a TVF. </li>

For reduced maintenance, I pass everything as NVARCHAR(MAX) as well as return NVARCHAR(MAX).  Speed is not my main problem.  The main problem is that SQL Server functions are severely lacking and new functions are added every third decade.  STRING_AGG is great, but a SQLCLR function can go back to at least 2012.

I do suspect that there may be memory allocation issues with this design, and so I may come up with a generative way to make VARCHAR(8000) or NVARCHAR(4000) clones.  Due to the way SQL Server pre-allocates memory, it may be even better to support smaller sizes.  Not really sure.

If it's possible to think of a logical algorithm that can easily be described and understood, and there's no confusion about what to expect in the output, then it's worth being a function.

For instance, I have a function called AnyOneOfTheseIsInThose, or something like that.  It is for when I to know if there is a non-empty intersection of a list of values that I don't want to stuff into tables.  It answers the simple question: Are any of these items in this other list?

To pass in lists, I use delimited strings, but I usually have an argument for the separating string.  SPLIT_STRING from Microsoft, and most home-grown SQLCLR splitters seem to think all splitting is based on single characters.  I support the string for the cases that come up.

## Structure or class organization, namespaces in other words:
<li>
  Adaptors - Converts something to something else as an electronic power adaptor would.  It's not a transformation, and it's reversible.
</li>
<li>
  Compares - Comparisons.  Since generics aren't supported, The name includes the type (like olden days). Often in SQL I need to get the greatest
  between two or a few things.  Greatest between PURCHASE_DATE and QUOTE or some such.  Usually between two or more events that could happen in
  either order.
</li>
<li>
  Environmental - I'm Windows centric.  Sorry.  So I try to "beep" sometimes in code, and I'm pretty sure the SQLCLR context is boxed out of 
  beeping, so it doesn't work.  But it will, someday.  I want things like "You're data is ready, Captain."  Might help the alertness quotient.
</li>
<li>
  FileNameExtract - Dumb name.  But using .NET FileInfo is a major load for processing say millions of documents off a shared drive.
  I should be able to replace FileInfo with some straight-forward parsing.
</li>
<li>
  Humanization - One of the most common functions around, but never really fully cooked, is it.  So this is yet-another attempt, and I may
  look around out there for better ones to borrow.
  DBAs/Developers never pause to think: Who is my audience?  Do they need to see rows and rows with 30 date-time columns?  If they're all
  sub-second from now, then say so.  We need a "yada yada yada" function.  If one sticks out and says "Last year", it should stick out!
  If there's a 9198, maybe it's really 1998?  I've had SQL Server blow on DATETIME with an Oracle DATE value of 0200, a typo which Oracle tolerates
  in a DATE column, but SQL Server did not.  DATE and DATETIME2 are better.
</li>
<li>
  NetworkCollect - Not sure what I was thinking.  Collect?
</li>
<li>
  NetworkTest - I use the "Test" suffix a lot.  "Ping" is important, but SQL Server feels like that's a job for System Engineers.  Well,
  if I'm scanning my SQL Servers or a list of possible Servers, then if I try to connect to each one, there will be a lot of time outs,
  and the default time out is about 30 seconds, or 20.  I turn that down, but if the host doesn't exist at all in Active Directory,
  then the timeout is defined by the OS, and SQL Server goes blithely on with waiting.
</li>
<li>
  StringExtract - This is a bit Polish-Hungarian for sorting.  These are all the functions that extract something from a string or a string of 
  substrings.  Here's an example of constant use in C#, which is not really something you get in SSMS or SQLCMD.  It's nice!
  I add functions that seem like an oversight.  "LeftOf" some charactor or string.  Why do I have to do 
            CASE WHEN CHARINDEX(',', s) > 0 THEN SUBSTRING(s, CHARINDEX(',', s)-1 ELSE NULL END??????
            <br><h1>ARGH!</h1>
</li>
<li>
  StringMeasure - Separate from extraction, we are instead taking measure of something, so new information is being gained, even though it 
  is deductive information.  Extracts are to avoid providing new information, but rather only provide a substring that exists within the source.
  <br> HowMany.  Simple enough!  How many commas?  We are pulling in a CSV, and missing commas or extras will break it.
</li>
<li>
  StringTest - Returns 1 or 0!  Very bitty.  These are probably deterministic.  I hope.  
  A lot of "Is it" and "Does it" questions.
</li>
<li>
  StringTransform - Actual change being made, new information by structural abruption.
</li>
<br/>
My favorites have to be Matches, Ping, StartsWith, EndsWith, AnyOfTheseeAreAnyOfThose, HowMany, LeftOf, LeftOfNth, LeftOfNth, IndexOfLast
Also GetFirstName, Pieces, RTrimChar.

I have a bunch somewhere, and I need to add more.

<h1>Some ideas:</h1>
- Better captures of stored procedure output or xp_cmdshell return strings.
- Escape function for multi-level dynamic SQL generation.
- A UNIX cut command for fixed-width.
- CSV, XML, Excel in/out.
- HTML table generation for emails.
- Intelligent Substring Title casing, like "ROTC" is not "Rotc".
- Smarter name normalization, accent removals for us Americanos.
- SQL formating more to my tastes.
- Some default formatters, like LogFileTimeStamp = "YYYYmmDDhh24missffffff" or some such.  So I don't have to remember.
- Pivot code I found for true column-to-row pivot.
- Possibly push SQL work down?  Like a TRUNCATE TABLE that pre-strips FKs, truncates, reloads, and tries to add the FKs back.
- Unix to/from for Active Directory columns.
- Natural language extraction of patterned speech.
