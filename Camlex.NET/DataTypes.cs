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
using System.Text;

namespace CamlexNET
{
    public class BaseFieldType
    {
        public static explicit operator BaseFieldType(string s) { return null; }
    }

    public class BaseFieldTypeWithOperators : BaseFieldType
    {
        public static bool operator <(object c1, BaseFieldTypeWithOperators c2) { return false; }
        public static bool operator >(object c1, BaseFieldTypeWithOperators c2) { return false; }
        public static bool operator <=(object c1, BaseFieldTypeWithOperators c2) { return false; }
        public static bool operator >=(object c1, BaseFieldTypeWithOperators c2) { return false; }
    }

    public class StringBasedFieldType : BaseFieldTypeWithOperators
    {
        public bool Contains(string text) { return true; }
        public bool StartsWith(string text) { return true; }
    }

    // See http://msdn.microsoft.com/en-us/library/microsoft.sharepoint.spfieldtype.aspx
    public static class DataTypes
    {
        public class AllDayEvent : BaseFieldType { }
        public class Attachments : BaseFieldType { }
        public class Boolean : BaseFieldType { }
        public class Calculated : BaseFieldTypeWithOperators { }
        public class Choice : BaseFieldType { }
        public class Computed : BaseFieldTypeWithOperators { }
        public class ContentTypeId : BaseFieldType { }
        public class Counter : BaseFieldTypeWithOperators { }
        public class CrossProjectLink : BaseFieldType { }
        public class Currency : BaseFieldTypeWithOperators { }
        public class DateTime : BaseFieldTypeWithOperators
        {
            public DateTime IncludeTimeValue() { return this; }
        }
        public class Error : BaseFieldType { }
        public class File : BaseFieldType { }
        public class GridChoice : BaseFieldType { }
        public class Guid : BaseFieldType { }
        public class Integer : BaseFieldTypeWithOperators { }
        public class Invalid : BaseFieldType { }
        
        // Lookup class is made internal. LookupValue and LookupId should be used instead
        internal class Lookup : BaseFieldType { }

        // There is no LookupId and LookupValue datatypes in CAML. There is only
        // Lookup datatype. We introduced different lookup datatypes in order
        // to simplify distinguish between lookup values and lookup ids search.
        // See http://camlex.codeplex.com/Thread/View.aspx?ThreadId=203560 for details
        public class LookupId : BaseFieldTypeWithOperators
        {
            // todo: allow cast from integer to LookupId
//            public static explicit operator LookupId(int id)
//            {
//                return null;
//            }
        }
        public class LookupValue : BaseFieldType { }

        public class MaxItems : BaseFieldType { }
        public class ModStat : BaseFieldType { }
        public class MultiChoice : BaseFieldType { }
        public class Note : StringBasedFieldType { }
        public class Number : BaseFieldTypeWithOperators { }
        public class PageSeparator : BaseFieldType { }
        public class Recurrence : BaseFieldType { }
        public class Text : StringBasedFieldType { }
        public class ThreadIndex : BaseFieldTypeWithOperators { }
        public class Threading : BaseFieldType { }
        public class URL : BaseFieldType { }
        public class User : BaseFieldType { }

        // As with LookupId there is no such data type as UserId. It is introduced
        // for simplifying querying by user id. See http://camlex.codeplex.com/discussions/264821
        public class UserId : BaseFieldType { }

        public class WorkflowEventType : BaseFieldType { }
        public class WorkflowStatus : BaseFieldType { }
    }
}
