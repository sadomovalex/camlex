using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Camlex.NET
{
    public class BaseFieldType
    {
        public static explicit operator BaseFieldType(string s)
        {
            return null;
        }
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
        public class DateTime : BaseFieldType { }
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
        public class Note : BaseFieldType { }
        public class Number : BaseFieldType { }
        public class PageSeparator : BaseFieldType { }
        public class Recurrence : BaseFieldType { }
        public class Text : BaseFieldType { }
        public class ThreadIndex : BaseFieldType { }
        public class Threading : BaseFieldType { }
        public class URL : BaseFieldType { }
        public class User : BaseFieldType { }
        public class WorkflowEventType : BaseFieldType { }
        public class WorkflowStatus : BaseFieldType { }
    }

//    public enum DataType
//    {
//        Integer = 0,
//        Text = 1
//        Attachments,
//        Boolean,
//        Choice,
//        Computed,
//        Counter,
//        DateTime,
//        Lookup,
//        ModStat,
//        MultiChoice,
//        Number,
//        Text,
//        User,
//        Note
//    }
}
