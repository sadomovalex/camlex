#region Copyright(c) Alexey Sadomov, Vladimir Timashkov, Stef Heyenrath. All Rights Reserved.
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
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using CamlexNET.UnitTests.Helpers;
using Microsoft.SharePoint.Client;
using NUnit.Framework;

namespace CamlexNET.UnitTests
{
	[TestFixture]
	public class CamlexOrderByTests
	{
		[Test]
		public void test_THAT_single_orderby_expression_IS_translated_sucessfully()
		{
			var caml = Camlex.Query().OrderBy(x => x["field1"] as Camlex.Desc).ToString();

			const string expected =
                "<Query>" +
				"  <OrderBy>" +
				"    <FieldRef Name=\"field1\" Ascending=\"False\" />" +
				"  </OrderBy>" +
                "</Query>";

			Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_multiple_orderby_expression_IS_translated_sucessfully()
		{
			var caml = Camlex.Query().OrderBy(
				x => new[] { x["field1"], x["field2"] as Camlex.Desc, x["field3"] as Camlex.Asc }).ToString();

			const string expected =
                "<Query>" +
                "  <OrderBy>" +
				"    <FieldRef Name=\"field1\" />" +
				"    <FieldRef Name=\"field2\" Ascending=\"False\" />" +
				"    <FieldRef Name=\"field3\" Ascending=\"True\" />" +
                "  </OrderBy>" +
                "</Query>";

			Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		// Not supported in Client Object Model
		//[Test]
		//public void test_THAT_orderby_expression_with_non_constant_parameters_IS_translated_sucessfully()
		//{
		//    bool b = true;
		//    var caml = Camlex.Query().OrderBy(x => new[] { x[b ? SPBuiltInFieldId.Title : SPBuiltInFieldId.UniqueId], x[SPBuiltInFieldId.Modified] as Camlex.Asc }).ToString();

		//    var expected =
		//    "<OrderBy>" +
		//    "  <FieldRef ID=\"fa564e0f-0c70-4ab9-b863-0177e6ddd247\" />" +
		//    "  <FieldRef ID=\"28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f\" Ascending=\"True\" />" +
		//    "</OrderBy>";

		//    Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
		//}

		[Test]
		public void test_THAT_2_order_by_ARE_joined_properly()
		{
			var orderByList = new List<Expression<Func<ListItem, object>>>();
			orderByList.Add(x => x["Title"] as Camlex.Asc);
			orderByList.Add(x => x["Date"] as Camlex.Desc);

			var caml = Camlex.Query().OrderBy(orderByList).ToString();

			const string expected =
                "<Query>" +
                "  <OrderBy>" +
				"    <FieldRef Name=\"Title\" Ascending=\"True\" />" +
				"    <FieldRef Name=\"Date\" Ascending=\"False\" />" +
                "  </OrderBy>" +
                "</Query>";

			Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_2_order_by_with_differenet_params_ARE_joined_properly()
		{
			var orderByList = new List<Expression<Func<ListItem, object>>>
			{
				x => x["Title"] as Camlex.Asc,
				y => y["Date"] as Camlex.Desc
			};

			var caml = Camlex.Query().OrderBy(orderByList).ToString();

			const string expected =
                "<Query>" +
                "  <OrderBy>" +
				"    <FieldRef Name=\"Title\" Ascending=\"True\" />" +
				"    <FieldRef Name=\"Date\" Ascending=\"False\" />" +
                "  </OrderBy>" +
                "</Query>";

			Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		[ExpectedException(typeof(EmptyExpressionsListException))]
		public void test_WHEN_order_by_list_is_empty_THEN_exception_is_thrown()
		{
			var orderByList = new List<Expression<Func<ListItem, object>>>();
			var caml = Camlex.Query().OrderBy(orderByList).ToString();
		}

		[Test]
		public void test_THAT_3_order_by_with_non_constant_fields_ARE_joined_properly()
		{
			var orderByList = new List<Expression<Func<ListItem, object>>>();
			const string name = "Title";
			orderByList.Add(x => x[name]);

			Func<string> f = () => "Date";
			orderByList.Add(x => x[f()]);

			var sb = new StringBuilder();
			for (int i = 0; i < 5; i++)
			{
				sb.Append("s");
			}
			orderByList.Add(x => x[sb.ToString()]);

			var caml = Camlex.Query().OrderBy(orderByList).ToString();

			const string expected =
                "<Query>" +
                "  <OrderBy>" +
				"    <FieldRef Name=\"Title\" />" +
				"    <FieldRef Name=\"Date\" />" +
				"    <FieldRef Name=\"sssss\" />" +
                "  </OrderBy>" +
                "</Query>";

			Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
		}
	}
}