﻿#region Copyright(c) Alexey Sadomov, Vladimir Timashkov, Stef Heyenrath. All Rights Reserved.
// -----------------------------------------------------------------------------
// Copyright(c) 2010 Alexey Sadomov, Vladimir Timashkov, Stef Heyenrath. All Rights Reserved.
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
using CamlexNET.UnitTests.Helpers;
using NUnit.Framework;

namespace CamlexNET.UnitTests
{
	[TestFixture]
	public class CamlexUserTests
	{
		[Test]
		public void test_THAT_user_id_field_ref_with_name_IS_translated_successfully()
		{
			string caml = Camlex.Query().Where(x => x["foo"] == (DataTypes.UserId)"123").ToString();

			const string expected =
               "<Query>" +
               "  <Where>" +
				"    <Eq>" +
				"        <FieldRef Name=\"foo\" LookupId=\"True\" />" +
				"        <Value Type=\"User\">123</Value>" +
				"    </Eq>" +
               "  </Where>" +
               "</Query>";

			Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		// Not supported by Client Object Model
		//[Test]
		//public void test_THAT_user_id_field_ref_with_guid_IS_translated_successfully()
		//{
		//    var guid = new Guid("{4feaf1f3-5b04-4d93-b0fc-4e48d0c60eed}");
		//    string caml = Camlex.Query().Where(x => x[guid] == (DataTypes.UserId)"123").ToString();

		//    string expected =
		//        "   <Where>" +
		//        "       <Eq>" +
		//        "           <FieldRef ID=\"4feaf1f3-5b04-4d93-b0fc-4e48d0c60eed\" LookupId=\"True\" />" +
		//        "           <Value Type=\"User\">123</Value>" +
		//        "       </Eq>" +
		//        "   </Where>";

		//    Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
		//}

		[Test]
		public void test_THAT_expression_with_user_IS_translated_successfully()
		{
			string caml = Camlex.Query().Where(x => x["foo"] == (DataTypes.User)"Foo Bar").ToString();

			const string expected =
               "<Query>" +
               "  <Where>" +
				"    <Eq>" +
				"        <FieldRef Name=\"foo\" />" +
				"        <Value Type=\"User\">Foo Bar</Value>" +
				"    </Eq>" +
               "  </Where>" +
               "</Query>";

			Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_expression_with_user_id_call_IS_translated_successfully()
		{
			string caml = Camlex.Query().Where(x => x["foo"] == (DataTypes.Integer)Camlex.UserID).ToString();

			const string expected =
               "<Query>" +
               "  <Where>" +
				"    <Eq>" +
				"        <FieldRef Name=\"foo\" />" +
				"        <Value Type=\"Integer\"><UserID /></Value>" +
				"    </Eq>" +
               "  </Where>" +
               "</Query>";

			Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
		}
	}
}