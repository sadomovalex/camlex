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
using System.Text;
using System.Xml.Linq;
using CamlexNET.Impl;
using CamlexNET.Impl.Factories;
using CamlexNET.Impl.ReverseEngeneering;
using CamlexNET.Impl.ReverseEngeneering.Caml.Factories;
using CamlexNET.Interfaces;
using CamlexNET.Interfaces.ReverseEngeneering;
using Microsoft.SharePoint;

namespace CamlexNET
{
    public class Camlex
    {
        #region DatetTime

        // Equivalent of DateTime.Now for string-based syntax
        public const string Now = "Now";
        // Equivalent of DateTime.Today for string-based syntax
        public const string Today = "Today";
        // Used only in DateRangeOverlap in string-based syntax
        public const string Week = "Week";
        // Used only in DateRangeOverlap in string-based syntax
        public const string Month = "Month";
        // Used only in DateRangeOverlap in string-based syntax
        public const string Year = "Year";

        // Function taking parameters needed for CAML's DateRangesOverlap function
        /// <param name="startField">Field containing start value of recurrent event</param>
        /// <param name="stopField">Field containing stop value of recurrent event</param>
        /// <param name="recurrenceField">Field containing recurrence ID of recurrent event</param>
        /// <param name="dateTime">DateTime value which the ranges should contain</param>
        /// <returns>Flag indicating whether date/time is inside range</returns>
        public static bool DateRangesOverlap(object startField, object stopField, object recurrenceField, DateTime dateTime) { return false; }

        // Function taking parameters needed for CAML's DateRangesOverlap function
        /// <param name="startField">Field containing start value of recurrent event</param>
        /// <param name="stopField">Field containing stop value of recurrent event</param>
        /// <param name="recurrenceField">Field containing recurrence ID of recurrent event</param>
        /// <param name="dateTime">DateTime value which the ranges should contain</param>
        /// <returns>Flag indicating whether date/time is inside range</returns>
        public static bool DateRangesOverlap(object startField, object stopField, object recurrenceField, DataTypes.DateTime dateTime) { return false; }

        #endregion

        #region Membership

        internal const string Membership_SPWebAllUsers = "SPWeb.AllUsers";
        internal const string Membership_SPGroup = "SPGroup";
        internal const string Membership_SPWebGroups = "SPWeb.Groups";
        internal const string Membership_CurrentUserGroups = "CurrentUserGroups";
        internal const string Membership_SPWebUsers = "SPWeb.Users";

        public class MembershipType
        {
            public override string ToString() { return string.Empty; }
        }
        // Marker class representing SPWeb.AllUsers type for "Membership" functionality
        public class SPWebAllUsers : MembershipType { public override string ToString() { return Membership_SPWebAllUsers; } }
        // Marker class representing SPGroup type for "Membership" functionality
        public class SPGroup : MembershipType
        {
            public int GroupId { get; private set; }
            public SPGroup(int groupId)
            {
                this.GroupId = groupId;
            }

            public override string ToString() { return Membership_SPGroup; }
        }
        // Marker class representing SPWeb.Groups type for "Membership" functionality
        public class SPWebGroups : MembershipType { public override string ToString() { return Membership_SPWebGroups; } }
        // Marker class representing CurrentUserGroups type for "Membership" functionality
        public class CurrentUserGroups : MembershipType { public override string ToString() { return Membership_CurrentUserGroups; } }
        // Marker class representing SPWeb.Users type for "Membership" functionality
        public class SPWebUsers : MembershipType { public override string ToString() { return Membership_SPWebUsers; } }

        // Function taking parameters needed for CAML's Membership function
        /// <param name="field">Field to check membership</param>
        /// <param name="membershipType">Type of membership</param>
        public static bool Membership(object field, MembershipType membershipType) { return false; }

        #endregion

        #region OrderBy functionality

        // Marker class representing ASC order direction for "OrderBy" functionality
        public class OrderDirection
        {
            public static OrderDirection Default { get { return new None(); } }
            public static OrderDirection Convert(Type type)
            {
                if (type == typeof(Asc)) return new Asc();
                if (type == typeof(Desc)) return new Desc();
                return Default;
            }
            public bool IsDefault()
            {
                return this.GetType() == Default.GetType();
            }
        }
        // Marker class representing absence of order direction for "OrderBy" functionality
        public class None : OrderDirection { public override string ToString() { return string.Empty; } }
        // Marker class representing ASC order direction for "OrderBy" functionality
        public class Asc : OrderDirection { public override string ToString() { return true.ToString(); } }
        // Marker class representing DESC order direction for "OrderBy" functionality
        public class Desc : OrderDirection { public override string ToString() { return false.ToString(); } }

        #endregion

        #region User
        public static string UserID = "";
        #endregion

        private static ITranslatorFactory translatorFactory;
        private static IReTranslatorFactory retranslatorFactory;
        private static IReLinkerFactory relinkerFactory;

        static Camlex()
        {
            // factories setup
            var operandBuilder = new OperandBuilder();
            var operationResultBuilder = new OperationResultBuilder();
            var analyzerFactory = new AnalyzerFactory(operandBuilder, operationResultBuilder);
            translatorFactory = new TranslatorFactory(analyzerFactory);

            // re
            var reoperandBuilder = new ReOperandBuilderFromCaml();
            var reanalyzerFactory = new ReAnalyzerFromCamlFactory(reoperandBuilder);
            retranslatorFactory = new ReTranslatorFromCamlFactory(reanalyzerFactory);
            relinkerFactory = new ReLinkerFromCamlFactory();
        }

        public static IQuery Query()
        {
            return new Query(translatorFactory, retranslatorFactory);
        }

        public static IReQuery QueryFromString(string input)
        {
            return new ReQuery(retranslatorFactory, relinkerFactory, input);
        }
    }
}
