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
using CamlexNET.Interfaces;
using Microsoft.SharePoint;

namespace CamlexNET
{
    public class Camlex
    {
        #region DatetTime

        /// <summary>Equivalent of DateTime.Now for string-based syntax</summary>
        public const string Now = "Now";
        /// <summary>Equivalent of DateTime.Today for string-based syntax</summary>
        public const string Today = "Today";
        /// <summary>Used only in DateRangeOverlar in string-based syntax</summary>
        public const string Week = "Week";
        /// <summary>Used only in DateRangeOverlar in string-based syntax</summary>
        public const string Month = "Month";
        /// <summary>Used only in DateRangeOverlar in string-based syntax</summary>
        public const string Year = "Year";

        /// <summary>Function taking parameters needed for CAML's DateRangesOverlap function</summary>
        /// <param name="startField">Field containing start value of recurrent event</param>
        /// <param name="stopField">Field containing stop value of recurrent event</param>
        /// <param name="recurrenceField">Field containing recurrnec ID of recurrent event</param>
        /// <param name="dateTime">DateTime value which the ranges should contain</param>
        /// <returns>Flag indicating whether date/time is inside ranfe</returns>
        public static bool DateRangesOverlap(object startField, object stopField, object recurrenceField, DateTime dateTime) { return false; }

        /// <summary>Function taking parameters needed for CAML's DateRangesOverlap function</summary>
        /// <param name="startField">Field containing start value of recurrent event</param>
        /// <param name="stopField">Field containing stop value of recurrent event</param>
        /// <param name="recurrenceField">Field containing recurrnec ID of recurrent event</param>
        /// <param name="dateTime">DateTime value which the ranges should contain</param>
        /// <returns>Flag indicating whether date/time is inside ranfe</returns>
        public static bool DateRangesOverlap(object startField, object stopField, object recurrenceField, DataTypes.DateTime dateTime) { return false; }

        #endregion

        #region OrderBy functionality

        /// <summary>Marker class representing ASC order direction for "OrderBy" functionality</summary>
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
        /// <summary>Marker class representing absence of order direction for "OrderBy" functionality</summary>
        public class None : OrderDirection { public override string ToString() { return string.Empty; } }
        /// <summary>Marker class representing ASC order direction for "OrderBy" functionality</summary>
        public class Asc : OrderDirection { public override string ToString() { return true.ToString(); } }
        /// <summary>Marker class representing DESC order direction for "OrderBy" functionality</summary>
        public class Desc : OrderDirection { public override string ToString() { return false.ToString(); } }

        #endregion

        private static ITranslatorFactory translatorFactory;

        static Camlex()
        {
            // factories setup
            var operandBuilder = new OperandBuilder();
            var operationResultBuilder = new OperationResultBuilder();
            var analyzerFactory = new AnalyzerFactory(operandBuilder, operationResultBuilder);
            translatorFactory = new TranslatorFactory(analyzerFactory);
        }

        public static IQuery Query()
        {
            return new Query(translatorFactory);
        }
    }
}
