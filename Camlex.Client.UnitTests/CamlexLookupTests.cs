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
using CamlexNET.UnitTests.Helpers;
using NUnit.Framework;

namespace CamlexNET.UnitTests
{
    [TestFixture]
    public class CamlexLookupTests
    {
        [Test]
        public void test_THAT_lookup_id_field_ref_with_name_IS_translated_successfully()
        {
            string caml = Camlex.Query().Where(x => x["Ref"] == (DataTypes.LookupId)"123").ToString();

            string expected =
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Ref\" LookupId=\"True\" />" +
                "           <Value Type=\"Lookup\">123</Value>" +
                "       </Eq>" +
                "   </Where>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_lookup_id_field_ref_with_guid_IS_translated_successfully()
        {
            var guid = new Guid("{4feaf1f3-5b04-4d93-b0fc-4e48d0c60eed}");
            string caml = Camlex.Query().Where(x => x[guid] == (DataTypes.LookupId)"123").ToString();

            string expected =
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef ID=\"4feaf1f3-5b04-4d93-b0fc-4e48d0c60eed\" LookupId=\"True\" />" +
                "           <Value Type=\"Lookup\">123</Value>" +
                "       </Eq>" +
                "   </Where>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_lookup_value_IS_translated_successfully()
        {
            string caml = Camlex.Query().Where(x => x["Ref"] == (DataTypes.LookupValue)"123").ToString();

            string expected =
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Ref\" />" +
                "           <Value Type=\"Lookup\">123</Value>" +
                "       </Eq>" +
                "   </Where>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}
