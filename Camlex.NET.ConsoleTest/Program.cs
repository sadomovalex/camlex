#region Copyright(c) Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
// -----------------------------------------------------------------------------
// Copyright(c) 2010 Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
//   1. No Trademark License - Microsoft Public License (Ms-PL) does not grant you rights to use
//      authors names, logos, or trademarks.
//   2. If you distribute any portion of the software, you must retain all copyright,
//      patent, trademark, and attribution notices that are present in the software.
//   3. If you distribute any portion of the software in source code form, you may do
//      so only under this license by including a complete copy of Microsoft Public License (Ms-PL)
//      with your distribution. If you distribute any portion of the software in compiled
//      or object code form, you may only do so under a license that complies with
//      Microsoft Public License (Ms-PL).
//   4. The names of the authors may not be used to endorse or promote products
//      derived from this software without specific prior written permission.
//
// The software is licensed "as-is." You bear the risk of using it. The authors
// give no express warranties, guarantees or conditions. You may have additional consumer
// rights under your local laws which this license cannot change. To the extent permitted
// under your local laws, the authors exclude the implied warranties of merchantability,
// fitness for a particular purpose and non-infringement.
// -----------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Xml.Linq;
using Microsoft.SharePoint;


namespace CamlexNET.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Scenario1();
            //Scenario2();
            //Scenario3();
            //Scenario4();
            //Scenario5();
            //Scenario6();
            //Scenario7();
            //Scenario8();
            Scenario9();
        }

        // Scenario 1. Simple query.
        // Suppose that you need to select all items which have Status field set to Completed
        // (following is the standard syntax of CAML):
        // <Query>
        //   <Where>
        //     <Eq>
        //       <FieldRef Name="Status" />
        //       <Value Type="Text">Completed</Value>
        //     </Eq>
        //   </Where>
        // </Query>
        public static void Scenario1()
        {
            string caml =
                Camlex.Query()
                    .Where(x => (string) x["Status"] == "Completed").ToString();
            Console.WriteLine(caml);
        }

        // Scenario 2. Query with “and”/”or” conditions
        // Suppose that you need to select items which have ProductID = 1000 and IsCompleted set to false or null. Syntax of appropriate standard CAML query follows:
        // <Query>
        //   <Where>
        //     <And>
        //       <Eq>
        //         <FieldRef Name="ProductID" />
        //         <Value Type="Integer">1000</Value>
        //       </Eq>
        //       <Or>
        //         <Eq>
        //           <FieldRef Name="IsCompleted" />
        //           <Value Type="Boolean">0</Value>
        //         </Eq>
        //         <IsNull>
        //           <FieldRef Name="IsCompleted" />
        //         </IsNull>
        //       </Or>
        //     </And>
        //   </Where>
        // </Query>
        public static void Scenario2()
        {
            var caml =
                Camlex.Query()
                    .Where(x => (int)x["ProductID"] == 1000 &&
                          ((bool)x["IsCompleted"] == false || x["IsCompleted"] == null))
                        .ToString();
            Console.WriteLine(caml);
        }

        // Scenario 3. Query with DateTime
        // Lets suppose that you need to retrieve items which were modified on 01-Jan-2010:
        // <Query>
        //   <Where>
        //     <Eq>
        //       <FieldRef Name="ModifiedBy" />
        //       <Value Type="DateTime">2010-01-01T12:00:00</Value>
        //     </Eq>
        //   </Where>
        // </Query>
        public static void Scenario3()
        {
            var caml =
                Camlex.Query()
                    .Where(x => (DateTime)x["ModifiedBy"] == new DateTime(2010, 01, 01)).ToString();
            Console.WriteLine(caml);
        }

        // Scenario 4. Query with BeginsWith and Contains operations
        // Consider the query that should return items which Title field starts with “Task” and Project field contains “Camlex”:
        // <Query>
        //   <Where>
        //     <And>
        //       <BeginsWith>
        //         <FieldRef Name="Title" />
        //         <Value Type="Text">Task</Value>
        //       </BeginsWith>
        //       <Contains>
        //         <FieldRef Name="Project" />
        //         <Value Type="Text">Camlex</Value>
        //       </Contains>
        //     </And>
        //   </Where>
        // </Query>
        public static void Scenario4()
        {
            var caml =
                Camlex.Query()
                    .Where(x => ((string)x["Title"]).StartsWith("Task") && ((string)x["Project"]).Contains("Camlex")).ToString();

            Console.WriteLine(caml);
        }

        // Scenario 5. Query with none C# native data types
        // Suppose that you need to retrieve all items modified by Administrator:
        // <Query>
        //   <Where>
        //     <Eq>
        //       <FieldRef Name="ModifiedBy" />
        //       <Value Type="User">Administrator</Value>
        //     </Eq>
        //   </Where>
        // </Query>
        public static void Scenario5()
        {
            var caml =
                Camlex.Query()
                    .Where(x => x["ModifiedBy"] == (DataTypes.User)"Administrator")
                            .ToString();

            Console.WriteLine(caml);
        }

        // Scenario 6. Query with sorting (OrderBy)
        // Suppose that you need to select all items which have ID >= 5
        // and the result should be sorted by Modified field:
        // <Query>
        //   <Where>
        //     <Geq>
        //       <FieldRef Name="ID" />
        //       <Value Type="Integer">5</Value>
        //     </Geq>
        //   </Where>
        //   <OrderBy>
        //     <FieldRef Name="Modified" />
        //   </OrderBy>
        // </Query>
        public static void Scenario6()
        {
            var caml =
                Camlex.Query()
                    .Where(x => (int)x["ID"] >= 5)
                    .OrderBy(x => x["Modified"]).ToString();

            Console.WriteLine(caml);
        }

        // Scenario 7. Query with grouping (GroupBy)
        // Suppose that we need to select items having not-null Status field and result set should be groupped by CreatedBy field:
        // <Query>
        //   <Where>
        //     <IsNotNull>
        //       <FieldRef Name="Status" />
        //     </IsNotNull>
        //   </Where>
        //   <GroupBy>
        //     <FieldRef Name="CreatedBy" />
        //   </GroupBy>
        // </Query>
        public static void Scenario7()
        {
            var caml =
                Camlex.Query()
                    .Where(x => x["Status"] != null)
                    .GroupBy(x => x["CreatedBy"]).ToString();

            Console.WriteLine(caml);
        }

        // Scenario 8. Query with non-constant expressions in lvalue and rvalue
        // Non-constant expression gives you more control over CAML. Suppose that you need to select items depending on current locale: for English locale you need to select items which have TitleEng field set to “eng”; for non-English locale you need to select items which have Title field set to “non-eng”. I.e.:
        // Query for English locale:
        // <Query>
        //   <Where>
        //     <Eq>
        //       <FieldRef Name="TitleEng" />
        //       <Value Type="Text">eng</Value>
        //     </Eq>
        //   </Where>
        // </Query>
        // Query for non-English locale:
        // <Query>
        //   <Where>
        //     <Eq>
        //       <FieldRef Name="Title" />
        //       <Value Type="Text">non-eng</Value>
        //     </Eq>
        //   </Where>
        // </Query>
        public static void Scenario8()
        {
            bool isEng = true; // or false depending on Thread.CurrentThread.CurrentUICulture

            var caml =
                Camlex.Query()
                    .Where(x => (string)x[isEng ? "TitleEng" : "Title"] == (isEng ? "eng" : "non-eng")).ToString();

            Console.WriteLine(caml);
        }

        public static void Scenario9()
        {
            var caml =
                Camlex.Query()
                    .Where(x => Camlex.DateRangesOverlap(x["start"], x["stop"], x["recurrence"], DateTime.Now)).ToString();
            Console.WriteLine(caml);
        }
    }
}