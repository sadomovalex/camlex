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
using System.Xml.Linq;

namespace CamlexNET
{
    internal static class Tags
    {
        public const string Query = "Query";
        public const string Where = "Where";
        public const string OrderBy = "OrderBy";
        public const string GroupBy = "GroupBy";
        public const string FieldRef = "FieldRef";
        public const string Value = "Value";
        public const string And = "And";
        public const string Or = "Or";
        public const string Eq = "Eq";
        public const string Neq = "Neq";
        public const string Geq = "Geq";
        public const string Gt = "Gt";
        public const string Leq = "Leq";
        public const string Lt = "Lt";
        public const string IsNotNull = "IsNotNull";
        public const string IsNull = "IsNull";
        public const string BeginsWith = "BeginsWith";
        public const string Contains = "Contains";
        public const string Includes = "Includes";
        public const string NotIncludes = "NotIncludes";
        public const string DateRangesOverlap = "DateRangesOverlap";
        public const string ViewFields = "ViewFields";
        public const string UserID = "UserID";
        public const string In = "In";
        public const string Values = "Values";
        public const string Joins = "Joins";
        public const string Join = "Join";
        public const string ProjectedFields = "ProjectedFields";
        public const string Field = "Field";
        public const string Membership = "Membership";
    }
}
