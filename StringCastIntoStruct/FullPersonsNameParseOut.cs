using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using static MySQLCLRFunctions.StringTest;
using static MySQLCLRFunctions.StringPivot;
using static MySQLCLRFunctions.StringMeasure;
using static MySQLCLRFunctions.StringFormat;
using static MySQLCLRFunctions.StringDecode;
using static MySQLCLRFunctions.StringReduce;
using static MySQLCLRFunctions.StringTransform;

namespace MySQLCLRFunctions.StringCastIntoStruct
{
    /*
     * Nelson, Richard
     * Davidson, Georgeanne
     * Armstrong, Crystalann
     * Peregrino Garcia, David
     * Alfaro Barroso, Jorge Luis                     -- 4 full names
     * Mohan-Ram, Vid
     * Cao, Thi Thuy Han                              -- 3 names after comma
     * 
     * -------------------------------------- Middle Initials
     * 
     * Collinge, Susan K
     * Ayala-Gutierrez, Jose M
     * Cortez Romero, Hazel V
     * Araujo-Lozano, J Jesus
     * Martinez Aviles, J Dolores
     * Adams, B K
     * Watkins, D C
     * Mallard, B T
     * Rucker, A.C.
     * Dominguez Ortiz, J Miguel
     * M, Srikanthreddy
     * Pressburg, Rebecca MP
     * Sandi, Alejandro MP
     * Witness, S. Angel
     * Mendiburu G Canton, Rafael I
     * 
     * -------------------------------------- Dotted names
     * 
     * Anderson, Nathaniel C.
     * Boggan, James R.
     * Garcia, Jose R.
     * Hlavka, Cody L.J.
     * Larsen, Rachel J.L.
     * 
     * -------------------------------------- Hyphenated
     * 
     * Davis, Billie-Jo
     * Abdulkarim Al-zuhairy, Zydon
     * Evens, Sara-Jane
     * Cason, Jon-Michael
     * Maier-Zinn, Jean E
     * Torres-Rivas, J M
     * Adams, B K
     * 
     * -------------------------------------- Accents and Unicode
     * 
     * Albarrán, Rosa                                 -- Accents raise confidence
     * Balasubramanian, Umaselvan                    -- UNICODE TRANLATION ERROR
     * D’Emanuele, Darren
     * D'Angelo, Gina
     * O'Donnell, Michelle
     * O'Hale, Kelly
     * O'callaghan, Simon
     * Zhao, Yun'an
     * --------------------------------------Generational Suffixes
     * 
     * Turner, Rick I
     * Godsil, Mike 1
     * Crane, Larry R Jr
     * Esparza, Domingo  Jr
     * Filibi, Francisco J Jr
     * Alonzo, JR                                    -- Uppercase may confuse classifier
     * De Vera, JR
     * Garza JR, Jose
     * Garza Jr, Arturo
     * Hinojosa, Jr., Daniel
     * Navarrete, Pedro  Sr
     * Garcia, Hector 2
     * Ali, Zekariya 2
     * Bishop, Timothy2                               -- Junior or second account?
     * MacBan, Barry A II                             -- 2
     * Forrest II, John A
     * Thiele, William L III
     * Garza III, Samuel
     * Aird III, Maurice J
     * Alvarado, Edgar V                               -- The fifth or V)ictor?
     * 
     * -------------------------------------- Middle parts
     *
     * D’Emanuele, Darren
     * D'Angelo, Gina
     * Rico De Ayala, Martha
     * Sifuentes De La O, Francisco J                -- Long
     * De Boer, John H
     * De Burgh, Scott
     * De La Cruz, Jose L
     * De La Torre Toscano, Flor
     * de la Rue, Craig
     * De Los Reyes, Brandon
     * de Braga, Justin L
     * de Braga, Kammie M
     * da Costa, Adam
     * Di Iorio, Angela
     * Carranza Jimenez, Ma Antonieta
     * Del Rio Leyva, Magdalena
     * dos Santos de Oliveira, Fabricio
     * Gomez de huerta, Antonia
     * Jansen van Beek, Dale
     * VanWeerdhuizen, Matthew R
     * Le, Loan
     * 
     * ------------------------------------------ Conjoined middle parts
     * DeLoera, Luis M
     * DeLeon, Leandro
     * DeMond, Jaylee
     * DeBoer, Bill
     * DiMatteo, Michael
     * West, ElRoy
     * DuBois, Scott
     * MacBan, Barry A II                             -- Mixed case words
     * McHugh, Michael C
     * MacDonald, Chad
     * McCall, Michael L
     * Berrett, McCall
     * Berg, DuWayne
     * Fox, NaShawn
     * LaFleur, Tim
     * Gan, MeiLi
     * Powell, LaVar
     * vanWijk, Nantko
     * VanDeSpiegle, Amy
     * VanMeter, James
     * MartinezDelgado, Ignacio
     * Ebarb, Russell                                 -- Unusual vowel lead before consonant
     * Voorhies, BreAnna
     * 
     * --------------------------------------Embedded name parts
     * 
     * Perez, Manuel (Jim) S                          -- Embedded Nickname
     * James (JC) Miller
     * 
     * -------------------------------------- Possible Duplicates
     * 
     * Ali, Zekariya
     * Ali, Zekariya 2
     * Anderson, Cassandra                  cassandra.anderson2                  (same manager
     * Anderson, Cassandra                  andersoc3
     * Cardenas, Eduardo
     * Cardenas, Eduardo 2
     * Golder, Hayden
     * Golder, Hayden 2
     * Applegate, Ty
     * Applegate, Tyler C
     * Coppleman, Ben
     * Coppleman, Benjamin
     * Cranfield, Chris
     * Cranfield, Chris [Admin]
     * Day, Ken                                 dayk2
     * Day, Ken 0                               ~dayk2
     * Delgado, Francisco J
     * Delgado, Francisco O
     * Garrison, Blake [WA]                     garrisob.sa
     * Garrison, Blake [WA]                     garrisob.wa
     * Jeff, Black [Admin]
     * Jeff, Black [Admin]
     * Mesia, Brooklyn [SA]
     * Mesia, Brooklyn [WA]
     * --------------------------------------Confusing
     * 
     * Garza, Rogelio (Colstor, Othello)
     * Alsumaeel, Musaab                              -- double vowels may reduce confidence
     * Alt, Jerry                                     -- "Alt" is not a surname
     * Ai, Xueyuan                                    -- Confuse with initials
     * Do, Minh                                       -- Confuse as short word
     * Du, Zhiqiang
     * Buchanan, Di
     * APPSwall, Service Account
     * Bledsoe-Healy, Dakotaha                        -- Rare spelling
     * Bol, Bol
     * Bribiescas, Jose j
     * Charlesworth, KC                               -- Is KC their name?
     * Garvin, TJ
     * Higley, AJ
     * Cheong, Wooi Kit                                -- Kit??
     * Choi, KY
     * Choi, Jin-joo
     * Chung, Dae-Ho
     * Collaborator, SPS
     * Cortez Cortez, Ma Salud                         -- Double words
     * Vonderhellen, DeeDee
     * Damm, John M                                    -- double-mm
     * David-Wilathgamuwa, Shaun
     * DOnofrio, Noemie                                -- Double-upper
     * Gomez, BD N
     * Eousr1, N
     * Jackson-Aman, Katrinka K
     * Kathirkamanathan, Kisokumar
     * Lara Barcenas, San Francisco
     * Lv, Lei
     * Luu, Nga B
     * Pinon Castro, Ma. Alejandra
     * VVG, Abhilash
     * winnr.tmp
     * Hinojosa, Jr., Daniel
     * 
     * -------------------------------------- Silly
     * 
     * McTesterson, Testy
     * McFly, Marty (test account)
     * 
     * -------------------------------------- Doubtful
     * Draper, D-O G
     * Eousr1, N                                              BANEOUSR1.shr
     * vnlinst.shr, VNL Instrumentation
     * 
     * --------------------------------------Type tags
     * 
     * Ward, Laura A (CWF)
     * AbrarAliKhan, Mohammed (CWF)
     * Bongale, Mahesh (InTimeTec)
     * Adam, Chris (RDI)
     * Kennedy, Tim (USA)
     * Brett, Randy [Admin]
     * Clark, Mel [Admin]
     * Coslett, Garrett [SA]
     * Hickey, Joey [WA]
     * Hunsaker, Lloyd (Western Sky Security)
     * Storey, Dan (Henningsen Cold Storage)
     * Hofstad, Richard (Henningsen Cold Storage)
     * Powers, Natalie (Americold Moses Lake)       == Embedded Location
     * Andersen Banducci  Law (eDiscovery)
     * Antognetti, Fernanda (Simplot Argentina)
     * JR Alonzo
     * Frankie Linford (backfill)
     * BI - TBH
     * POW01, SGS Powell Ranco Blend PC
     * Sensory Club, Simplot
     * sss_svnauto, Service Account
     * 
     * -------------------------------------- Alternate spellings
     * 
     * Hein, Jeffery W
     * -------------------------------------- Typos
     * Costello, Bryce 0
     * Flores, Erick }                                      Erick	NULL	Flores
     * Earll, Mariah N
     * McWHINNIE, John
     * Nyasenu, afuavi
     * BSGSTest,
     * BS01.sss
     * ECMUser18
     * ECM Invoices QA
     * 
     * -------------------------------------- Full names not combo of name parts
     * Gutierrez, Jose 0                    Joe
     * Gutierrez, Jose 1                    Jose
     * Helisa, Dorcus                       Tia
     * O'callaghan, Simon                   O'Callaghan
     * 
     * -------------------------------------- Test Values
     * 
     * Activate, KasunTest                          
     * *TEST* Matt, NA                              -- Domain name
     * *TEST*Huynh, Jason
     * Test, Thin Client
     * Echuca1, Test
       Echuca2, Test
       Echuca3, Test
     * EMPNONEMP, Test
     * Flemington, Test
     * Flemington1, Test
     * Flemington2, Test
     * Flemington3, Test
Test Payroll, Kronos
Test, A B
Test, Auckland
Test, David D
Test, JDE
Test, Nate
Test, SAPLBIUser
Test, Thin Client
Test_Account, AU_Cherwell
Test_Account, NA_Cherwell
TestAccount1, Sharepoint
TestAccount2, Sharepoint
TestAccount3, Sharepoint
TestAccount4, Sharepoint
TestAccount5, Sharepoint
TestTitle, Test

    * NO COMMA
    * 
    * Admin Kofax
    * Agriview Ecom
    * Agriview Ecom Dev
    * BrioTest
    * Brock Stringer
    * 
     * 
     * Service Accounts
     * 
     * BOI05 Avaya - Service Desk
     * AspenAdmin, Service
     * RM DON02 Explorer - Frontier
     * One Card Helpdesk (FootPrints)
     * FG Business Change Request (FootPrints)        == ERROR: NOT A PERSONAL NAME
     * AB Credit Requests (FootPrints)
     * Footprints [IT Support]                        == Bracket usage
     * SPLLC RKS I&E                                  == And sign
     * Payroll Time Entry (SS)
     * MPH01Breakroom Presentation PC
     * SPLLC RKS Security #1
     * System Notifications (IT)
     * Nampa Scale Ticket Operator
     * Truck Scales Rivergate
     * Check Special Handling (Team)
     * HR California (external address)
     * order toner (accuratelaser.com)              == Embedded URL
     * Product to Ship (Lathrop)
     * packlab1
     * wwline2lab
     * Isolated Host Instance, BizTalk
     * wwadmin
     * Bas Management Web Service Account
     * oth01dcs1.shr
     * CLD01, FG Tech Center Kiosk Machine
     * BOI01.SCN, SGS Scanner Account
     * ODBC USER FOR ACCESS MINE DB, SMK01MINEDB
     * Caldwell Transportation Shared  iPad
     * First Mailbox-Journal
     * Auto-Logon Account, don01guardhouse.shr
     * Autocad, SPLLC RKS
     * Audit, Transition
     * Auditorium, WHQ
     * Boardroom, WHQ
     * Backup Service Account, SMK01BACKUP
     * Biotech, Lab computer
     * Counter PC -Hershey, SGS Counter PC 71
     */
    [Serializable]
    [Microsoft.SqlServer.Server.SqlUserDefinedType(Format.Native)] // , IsByteOrdered=true, Name="Point",ValidationMethodName = "ValidatePoint"
    public struct FullPersonsNameParseOut : INullable
    {
        public override string ToString()
        {
            if (this.IsNull)
                return "NULL";
            else
            {
                StringBuilder builder = new StringBuilder();
                var PartsOfNameInOrder = new SortedList<int, string>(4);
                int i = 0;
                // Salutation
                if (!IsNullOrWhiteSpaceOrEmpty(GivenName.ToString())) PartsOfNameInOrder.Add(i++, GivenName.ToString());
                if (!IsNullOrWhiteSpaceOrEmpty(MiddleInitial.ToString()))
                {
                    if (DotInitials)
                        PartsOfNameInOrder.Add(i++, MiddleInitial.ToString() + ".");
                    else
                        PartsOfNameInOrder.Add(i++, MiddleInitial.ToString());

                }
                if (!IsNullOrWhiteSpaceOrEmpty(Surname.ToString())) PartsOfNameInOrder.Add(i++, Surname.ToString());
                // Suffix
                // extension/instruction in parens

                int lastIndexNamePart = PartsOfNameInOrder.Count - 1;
                int currentIndex = 0;
                StringBuilder FullNameWithGivenNameFirst = new StringBuilder(128);
                foreach(string namePart in PartsOfNameInOrder.Values)
                {
                    FullNameWithGivenNameFirst.Append(namePart);
                    if (currentIndex < lastIndexNamePart) FullNameWithGivenNameFirst.Append(" ");
                    currentIndex++;
                }

                return builder.ToString();
            }
        }

        public bool IsNull
        {
            get
            {
                // Put your code here
                return _null;
            }
        }

        public static FullPersonsNameParseOut Null
        {
            get
            {
                FullPersonsNameParseOut h = new FullPersonsNameParseOut();
                h._null = true;
                h.ConfidenceInputPersonName = 0.0f;
                return h;
            }
        }

        //private enum WordRoleEnum
        //{
        //    GivenName, MiddleName, MI, Surname, GenerationalSuffix
        //}
        //private class ClassifiedWord
        //{
        //    string word;
        //    WordRoleEnum wordRole;
        //    bool isAllLowerCase; // la, de
        //    bool isJustAfterComma;
        //}
        public static FullPersonsNameParseOut Parse(SqlString input)
        {

            if (input.IsNull)
                return Null;
            FullPersonsNameParseOut u = new FullPersonsNameParseOut();
            u.OriginalInput = input;
            u.state = "Unprocessed";
            if (IsEmptyOrWhiteSpace(input.ToString()))
            {
                u.state = "Empty";
                return u;
            }

            string l = input.ToString();
            var namepartsInInput = (IEnumerable<PiecesWithMatchesRecord>)PiecesWithMatchesX(l, "\\w+");
            int nofCommas = (int)input.HOWMANYC(',');
            if (nofCommas == 0)
            {
                u.NameOrder = "FirstNameLast";
                int offsetForMI = 0;
                foreach (PiecesWithMatchesRecord namepart in namepartsInInput)
                {

                    if (namepart.pieceOrderNo == 1)
                    {
                        u.GivenName = namepart.piece;
                        if (namepart.pieceOrderNo == 1 && namepart.piece.Length == 1 && namepartsInInput.Count() > 2)
                        {
                            u.MiddleInitial = namepart.piece;
                            offsetForMI = 1;
                        }
                    }
                    else if (namepart.pieceOrderNo == 2)
                    {
                        if (namepart.piece.Length > 1 && namepartsInInput.Count() > 2)
                        {
                            u.MiddleName = namepart.piece;
                            u.MiddleInitial = namepart.piece[0].ToString();
                            offsetForMI = 1;
                        }
                        else if (namepartsInInput.Count() == 2)
                        {
                            u.MiddleName = namepart.piece;
                            u.MiddleInitial = namepart.piece[0].ToString();
                            offsetForMI = 1;
                        }
                        else if (namepart.pieceOrderNo == 2 + offsetForMI) { u.Surname = namepart.piece; }
                    }
                    else if (namepart.pieceOrderNo == 3 + offsetForMI)
                    {
                        u.GenerationalSuffix = namepart.piece;
                    }
                    else
                    {
                        u.state = "Error";
                        u.NatureOfError = $"Unrecognized type of word: namepart.pieceOrder={namepart.pieceOrderNo}";
                        return u;
                    }
                }
            }
            else if (nofCommas == 1)
            {

                (string firstHalfInput, string lastHalfInput) = l.SplitIn2OnC(','); // I love tuples.
                u.NameOrder = "LastNameFirst";
                bool pastCommaYet = false;
                int howmanywordspastcomma = 0;
                int howmanywordsbeforecomma = 0;

                u.NofGivenNamesFound = 0;
                u.NofSurnamesFound = 0;
                u.NofMiddleInitialsFound = 0;
                u.NofMiddleNamesFound = 0;
                
                foreach (PiecesWithMatchesRecord namepart in namepartsInInput)
                {
                    if (namepart.matchAtStartOfPiece.Length > 0 && namepart.matchAtStartOfPiece[0] == ',')
                    {
                        pastCommaYet = true;
                        howmanywordspastcomma = 0;
                    }
                    else
                    {
                        howmanywordspastcomma++;
                    }

                    if (!pastCommaYet)
                    {
                        u.NofSurnamesFound = (SqlByte)(u.NofSurnamesFound.Value + 1);
                        if (namepart.pieceOrderNo == 0) u.Surname = namepart.piece;
                        else if (namepart.pieceOrderNo == 1) u.Surname2 = namepart.piece;
                        else if (namepart.pieceOrderNo == 2) u.Surname3 = namepart.piece;
                        else
                        {
                            u.state = "Error";
                            u.NatureOfError = $"Unsupported number of surnames before first comma in input: {nofCommas}, up to #{namepart.pieceOrderNo}";
                            return u;
                        }

                        if ((bool)namepart.piece.In("I", "II", "III", "IV", "JR", "SR", "Jr", "Sr")) u.GenerationalSuffix = namepart.piece;
                            
                        howmanywordsbeforecomma = namepart.pieceOrderNo + 1;
                    }
                    else
                    {
                        u.NofGivenNamesFound = (SqlByte)(u.NofGivenNamesFound.Value + 1);
                        if (namepart.pieceOrderNo == howmanywordsbeforecomma + 0) u.GivenName = namepart.piece;
                        else if (namepart.pieceOrderNo == howmanywordsbeforecomma + 1) u.GivenName2 = namepart.piece;
                        else if (namepart.pieceOrderNo == howmanywordsbeforecomma + 2) u.GivenName3 = namepart.piece;
                        else
                        {
                            u.state = "Error";
                            u.NatureOfError = $"Unsupported number of givennames after first comma in input: {nofCommas}, up to #{namepart.pieceOrderNo}";
                            return u;
                        }

                        howmanywordsbeforecomma++;
                    }
                }
            }
            else
            {
                u.state = "Error";
                u.NatureOfError = $"Unsupported number of commas in input: {nofCommas}";
                return u;
            }

            u.state = "Parsed";
            return u;
        }

        public static bool DotInitials = false;
        public static bool FLTrueLFFalse = true;

        //  Private member
        private bool _null;

        public SqlString OriginalInput;
        public SqlString NameOrder;
        public SqlString state;
        public SqlString NatureOfError;
        public SqlString Salutation;
        public SqlString NickName;
        public SqlString GivenName;
        public SqlString GivenName2;
        public SqlString GivenName3;
        public SqlByte NofGivenNamesFound;

        public SqlString MiddleInitial;
        public SqlString MiddleInitial2;
        public SqlString MiddleInitial3;
        public SqlByte NofMiddleInitialsFound;
 
        public SqlString MiddleName;
        public SqlString MiddleName2;
        public SqlString MiddleName3;
        public SqlByte NofMiddleNamesFound;
        
        public SqlString Surname;
        public SqlString Surname2;
        public SqlString Surname3;
        public SqlByte NofSurnamesFound;
        
        public SqlString GenerationalSuffix;
        public SqlDouble ConfidenceInputPersonName;
    }
}
