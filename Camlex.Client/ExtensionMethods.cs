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

namespace CamlexNET
{
    public static class ExtensionMethods
    {
        /// <summary>Marker method toindicate that DateTime value should have IncludeTimeValue attribute</summary>
        /// <param name="dateTime">DateTime value</param>
        /// <returns>Not modified DateTime value</returns>
        public static DateTime IncludeTimeValue(this DateTime dateTime) { return dateTime; }

        public static object PrimaryList(this object val, string primaryListAlias) { return val; }
        public static object ForeignList(this object val, string foreignListAlias) { return val; }
        public static object List(this object val, string foreignListAlias) { return val; }
        public static object ShowField(this object val, string fieldTitle) { return val; }
    }
}
