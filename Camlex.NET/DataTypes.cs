#region Copyright(c) Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
// -----------------------------------------------------------------------------
// Copyright(c) 2007 Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
//   1.  No Trademark License - Microsoft Public License (Ms-PL) does not grant you rights to use
//       authors names, logos, or trademarks.
//   2.  If you distribute any portion of the software, you must retain all copyright,
//       patent, trademark, and attribution notices that are present in the software.
//   3.  If you distribute any portion of the software in source code form, you may do
//       so only under this license by including a complete copy of Microsoft Public License (Ms-PL)
//       with your distribution. If you distribute any portion of the software in compiled
//       or object code form, you may only do so under a license that complies with
//       Microsoft Public License (Ms-PL).
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
        public static explicit operator BaseFieldType(string s)
        {
            return null;
        }
    }

    public class StringBasedFieldType : BaseFieldType
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
        public class Calculated : BaseFieldType { }
        public class Choice : BaseFieldType { }
        public class Computed : BaseFieldType { }
        public class ContentTypeId : BaseFieldType { }
        public class Counter : BaseFieldType { }
        public class CrossProjectLink : BaseFieldType { }
        public class Currency : BaseFieldType { }
        public class DateTime : BaseFieldType { public DateTime IncludeTimeValue() { return this; } }
        public class Error : BaseFieldType { }
        public class File : BaseFieldType { }
        public class GridChoice : BaseFieldType { }
        public class Guid : BaseFieldType { }
        public class Integer : BaseFieldType { }
        public class Invalid : BaseFieldType { }
        public class Lookup : BaseFieldType { }
        public class MaxItems : BaseFieldType { }
        public class ModStat : BaseFieldType { }
        public class MultiChoice : BaseFieldType { }
        public class Note : StringBasedFieldType { }
        public class Number : BaseFieldType { }
        public class PageSeparator : BaseFieldType { }
        public class Recurrence : BaseFieldType { }
        public class Text : StringBasedFieldType { }
        public class ThreadIndex : BaseFieldType { }
        public class Threading : BaseFieldType { }
        public class URL : BaseFieldType { }
        public class User : BaseFieldType { }
        public class WorkflowEventType : BaseFieldType { }
        public class WorkflowStatus : BaseFieldType { }
    }
}
