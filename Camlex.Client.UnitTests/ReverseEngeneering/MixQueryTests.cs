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
using CamlexNET.UnitTests.Helpers;
using Microsoft.SharePoint.Client;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering
{
	[TestFixture]
	public class MixQueryTests
	{
		[Test]
		public void test_THAT_existing_single_eq_expression_IS_mixed_with_single_expression_correctly_using_and()
		{
			const string existingQuery =
				"<Where>" +
				"    <Eq>" +
				"        <FieldRef Name=\"Title\" />" +
				"        <Value Type=\"Text\">testValue</Value>" +
				"    </Eq>" +
				"</Where>";

			const string expected =
				"<Where>" +
				"  <And>" +
				"    <Eq>" +
				"      <FieldRef Name=\"Title\" />" +
				"      <Value Type=\"Text\">foo</Value>" +
				"    </Eq>" +
				"    <Eq>" +
				"      <FieldRef Name=\"Title\" />" +
				"      <Value Type=\"Text\">testValue</Value>" +
				"    </Eq>" +
				"  </And>" +
				"</Where>";

			var query = Camlex.Query().WhereAll(existingQuery, x => (string)x["Title"] == "foo").ToString();
			Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_existing_single_eq_expression_with_query_tag_IS_mixed_with_single_expression_correctly_using_and()
		{
			const string existingQuery =
				"<Query>" +
				"   <Where>" +
				"       <Eq>" +
				"           <FieldRef Name=\"Title\" />" +
				"           <Value Type=\"Text\">testValue</Value>" +
				"       </Eq>" +
				"   </Where>" +
				"</Query>";

			const string expected =
				"<Where>" +
				"  <And>" +
				"    <Eq>" +
				"      <FieldRef Name=\"Title\" />" +
				"      <Value Type=\"Text\">foo</Value>" +
				"    </Eq>" +
				"    <Eq>" +
				"      <FieldRef Name=\"Title\" />" +
				"      <Value Type=\"Text\">testValue</Value>" +
				"    </Eq>" +
				"  </And>" +
				"</Where>";

			var query = Camlex.Query().WhereAll(existingQuery, x => (string)x["Title"] == "foo").ToString();
			Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_existing_several_expressions_ARE_mixed_with_several_expressions_correctly_using_and()
		{
			const string existingQuery =
				"<Where>" +
				"  <And>" +
				"    <Eq>" +
				"      <FieldRef Name=\"Title\" />" +
				"      <Value Type=\"Text\">foo</Value>" +
				"    </Eq>" +
				"    <Eq>" +
				"      <FieldRef Name=\"Title\" />" +
				"      <Value Type=\"Text\">testValue</Value>" +
				"    </Eq>" +
				"  </And>" +
				"</Where>";

			const string expected =
				"<Where>" +
				"  <And>" +
				"    <And>" +
				"      <Gt>" +
				"        <FieldRef Name=\"Count\" />" +
				"        <Value Type=\"Integer\">1</Value>" +
				"      </Gt>" +
				"      <IsNotNull>" +
				"        <FieldRef Name=\"Status\" />" +
				"      </IsNotNull>" +
				"    </And>" +
				"    <And>" +
				"      <Eq>" +
				"        <FieldRef Name=\"Title\" />" +
				"        <Value Type=\"Text\">foo</Value>" +
				"      </Eq>" +
				"      <Eq>" +
				"        <FieldRef Name=\"Title\" />" +
				"        <Value Type=\"Text\">testValue</Value>" +
				"      </Eq>" +
				"    </And>" +
				"  </And>" +
				"</Where>";

			var query = Camlex.Query().WhereAll(existingQuery, x => (int)x["Count"] > 1 && x["Status"] != null).ToString();
			Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_existing_single_eq_expression_IS_mixed_with_single_expression_correctly_using_or()
		{
			const string existingQuery =
				"   <Where>" +
				"       <Eq>" +
				"           <FieldRef Name=\"Title\" />" +
				"           <Value Type=\"Text\">testValue</Value>" +
				"       </Eq>" +
				"   </Where>";

			const string expected =
				"<Where>" +
				"  <Or>" +
				"    <Eq>" +
				"      <FieldRef Name=\"Title\" />" +
				"      <Value Type=\"Text\">foo</Value>" +
				"    </Eq>" +
				"    <Eq>" +
				"      <FieldRef Name=\"Title\" />" +
				"      <Value Type=\"Text\">testValue</Value>" +
				"    </Eq>" +
				"  </Or>" +
				"</Where>";

			var query = Camlex.Query().WhereAny(existingQuery, x => (string)x["Title"] == "foo").ToString();
			Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_existing_several_expressions_ARE_mixed_with_several_expressions_correctly_using_or()
		{
			const string existingQuery =
				"<Where>" +
				"  <And>" +
				"    <Eq>" +
				"      <FieldRef Name=\"Title\" />" +
				"      <Value Type=\"Text\">foo</Value>" +
				"    </Eq>" +
				"    <Eq>" +
				"      <FieldRef Name=\"Title\" />" +
				"      <Value Type=\"Text\">testValue</Value>" +
				"    </Eq>" +
				"  </And>" +
				"</Where>";

			const string expected =
				"<Where>" +
				"  <Or>" +
				"    <And>" +
				"      <Gt>" +
				"        <FieldRef Name=\"Count\" />" +
				"        <Value Type=\"Integer\">1</Value>" +
				"      </Gt>" +
				"      <IsNotNull>" +
				"        <FieldRef Name=\"Status\" />" +
				"      </IsNotNull>" +
				"    </And>" +
				"    <And>" +
				"      <Eq>" +
				"        <FieldRef Name=\"Title\" />" +
				"        <Value Type=\"Text\">foo</Value>" +
				"      </Eq>" +
				"      <Eq>" +
				"        <FieldRef Name=\"Title\" />" +
				"        <Value Type=\"Text\">testValue</Value>" +
				"      </Eq>" +
				"    </And>" +
				"  </Or>" +
				"</Where>";

			var query = Camlex.Query().WhereAny(existingQuery, x => (int)x["Count"] > 1 && x["Status"] != null).ToString();
			Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		[ExpectedException(typeof(IncorrectCamlException))]
		public void test_WHEN_where_is_not_provided_THEN_exception_is_thrown()
		{
			const string existingQuery =
				"<Query>" +
				"  <OrderBy>" +
				"    <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
				"  </OrderBy>" +
				"</Query>";
			var query = Camlex.Query().WhereAll(existingQuery, x => (string)x["Title"] == "foo").ToString();
		}

		[Test]
		public void test_THAT_single_order_by_IS_mixed_with_single_order_by_correctly()
		{
			const string existingQuery =
				"  <OrderBy>" +
				"    <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
				"  </OrderBy>";

			const string expected =
				"<OrderBy>" +
				"  <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
				"  <FieldRef Name=\"Title\" />" +
				"</OrderBy>";

			var query = Camlex.Query().OrderBy(existingQuery, x => x["Title"]).ToString();
			Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_single_order_by_IS_mixed_with_several_order_by_correctly()
		{
			const string existingQuery =
				"  <OrderBy>" +
				"    <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
				"  </OrderBy>";

			const string expected =
				"<OrderBy>" +
				"  <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
				"  <FieldRef Name=\"Title\" />" +
				"  <FieldRef Name=\"State\" Ascending=\"True\" />" +
				"</OrderBy>";

			var query = Camlex.Query().OrderBy(existingQuery, x => new[] { x["Title"], x["State"] as Camlex.Asc }).ToString();
			Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_several_order_by_IS_mixed_with_several_order_by_correctly()
		{
			const string existingQuery =
				"  <OrderBy>" +
				"    <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
				"    <FieldRef Name=\"ModifiedBy\" />" +
				"  </OrderBy>";

			const string expected =
				"<OrderBy>" +
				"  <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
				"  <FieldRef Name=\"ModifiedBy\" />" +
				"  <FieldRef Name=\"Title\" />" +
				"  <FieldRef Name=\"State\" Ascending=\"True\" />" +
				"</OrderBy>";

			var query = Camlex.Query().OrderBy(existingQuery, x => new[] { x["Title"], x["State"] as Camlex.Asc }).ToString();
			Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_order_by_collection_IS_mixed_with_several_order_by_correctly()
		{
			const string existingQuery =
				"  <OrderBy>" +
				"    <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
				"    <FieldRef Name=\"ModifiedBy\" />" +
				"  </OrderBy>";

			const string expected =
				"<OrderBy>" +
				"  <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
				"  <FieldRef Name=\"ModifiedBy\" />" +
				"  <FieldRef Name=\"Title\" />" +
				"  <FieldRef Name=\"State\" Ascending=\"True\" />" +
				"</OrderBy>";

			var exprs = new List<Expression<Func<ListItem, object>>>();
			exprs.Add(x => x["Title"]);
			exprs.Add(x => x["State"] as Camlex.Asc);

			var query = Camlex.Query().OrderBy(existingQuery, exprs).ToString();
			Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		[ExpectedException(typeof(IncorrectCamlException))]
		public void test_WHEN_order_by_is_not_provided_THEN_exception_is_thrown()
		{
			const string existingQuery =
				"   <Where>" +
				"       <Eq>" +
				"           <FieldRef Name=\"Title\" />" +
				"           <Value Type=\"Text\">testValue</Value>" +
				"       </Eq>" +
				"   </Where>";
			var query = Camlex.Query().OrderBy(existingQuery, x => x["Title"]).ToString();
		}

		[Test]
		public void test_THAT_single_group_by_IS_mixed_with_single_group_by_correctly()
		{
			const string existingQuery =
				"  <GroupBy>" +
				"    <FieldRef Name=\"Modified\" />" +
				"  </GroupBy>";

			const string expected =
				"<GroupBy>" +
				"  <FieldRef Name=\"Modified\" />" +
				"  <FieldRef Name=\"Title\" />" +
				"</GroupBy>";

			var query = Camlex.Query().GroupBy(existingQuery, x => x["Title"]).ToString();
			Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_single_group_by_IS_mixed_with_several_group_by_correctly()
		{
			const string existingQuery =
				"  <GroupBy>" +
				"    <FieldRef Name=\"Modified\" />" +
				"  </GroupBy>";

			const string expected =
				"<GroupBy>" +
				"  <FieldRef Name=\"Modified\" />" +
				"  <FieldRef Name=\"Title\" />" +
				"  <FieldRef Name=\"State\" />" +
				"</GroupBy>";

			var query = Camlex.Query().GroupBy(existingQuery, x => new[] { x["Title"], x["State"] }).ToString();
			Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_several_group_by_IS_mixed_with_several_group_by_correctly()
		{
			const string existingQuery =
				"  <GroupBy>" +
				"    <FieldRef Name=\"Modified\" />" +
				"    <FieldRef Name=\"ModifiedBy\" />" +
				"  </GroupBy>";

			const string expected =
				"<GroupBy>" +
				"  <FieldRef Name=\"Modified\" />" +
				"  <FieldRef Name=\"ModifiedBy\" />" +
				"  <FieldRef Name=\"Title\" />" +
				"  <FieldRef Name=\"State\" />" +
				"</GroupBy>";

			var query = Camlex.Query().GroupBy(existingQuery, x => new[] { x["Title"], x["State"] }).ToString();
			Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_existing_group_by_HAS_more_priority()
		{
			const string existingQuery =
				"<GroupBy Collapse=\"False\" GroupLimit=\"20\">" +
				"	<FieldRef Name=\"Modified\" />" +
				"	<FieldRef Name=\"ModifiedBy\" />" +
				"</GroupBy>";

			const string expected =
				"<GroupBy Collapse=\"False\" GroupLimit=\"20\">" +
				"	<FieldRef Name=\"Modified\" />" +
				"	<FieldRef Name=\"ModifiedBy\" />" +
				"	<FieldRef Name=\"Title\" />" +
				"	<FieldRef Name=\"State\" />" +
				"</GroupBy>";

			var query = Camlex.Query().GroupBy(existingQuery, x => new[] { x["Title"], x["State"] }).ToString();
			Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		//        [Test]
		//        public void test_THAT_group_by_collection_IS_mixed_with_several_group_by_correctly()
		//        {
		//            const string existingQuery =
		//                "  <GroupBy>" +
		//                "    <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
		//                "    <FieldRef Name=\"ModifiedBy\" />" +
		//                "  </GroupBy>";
		//
		//            const string expected =
		//                "<GroupBy>" +
		//                "  <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
		//                "  <FieldRef Name=\"ModifiedBy\" />" +
		//                "  <FieldRef Name=\"Title\" />" +
		//                "  <FieldRef Name=\"State\" Ascending=\"True\" />" +
		//                "</GroupBy>";
		//
		//            var exprs = new List<Expression<Func<ListItem, object>>>();
		//            exprs.Add(x => x["Title"]);
		//            exprs.Add(x => x["State"] as Camlex.Asc);
		//
		//            var query = Camlex.Query().GroupBy(existingQuery, exprs).ToString();
		//            Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
		//        }

		[Test]
		[ExpectedException(typeof(IncorrectCamlException))]
		public void test_WHEN_group_by_is_not_provided_THEN_exception_is_thrown()
		{
			const string existingQuery =
				"   <Where>" +
				"       <Eq>" +
				"           <FieldRef Name=\"Title\" />" +
				"           <Value Type=\"Text\">testValue</Value>" +
				"       </Eq>" +
				"   </Where>";
			var query = Camlex.Query().GroupBy(existingQuery, x => x["Title"]).ToString();
		}

		[Test]
		public void test_THAT_single_view_fields_IS_mixed_with_single_view_fields_correctly()
		{
			const string existingQuery =
				"    <FieldRef Name=\"Modified\" />";

			const string expected =
				"  <FieldRef Name=\"Modified\" />" +
				"  <FieldRef Name=\"Title\" />";

			var query = Camlex.Query().ViewFields(existingQuery, x => x["Title"]).ToString();
			Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_single_view_fields_IS_mixed_with_several_view_fields_correctly()
		{
			const string existingQuery =
				"    <FieldRef Name=\"Modified\" />";

			const string expected =
				"  <FieldRef Name=\"Modified\" />" +
				"  <FieldRef Name=\"Title\" />" +
				"  <FieldRef Name=\"State\" />";

			var query = Camlex.Query().ViewFields(existingQuery, x => new[] { x["Title"], x["State"] }).ToString();
			Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_several_view_fields_IS_mixed_with_several_view_fields_correctly()
		{
			const string existingQuery =
				"    <FieldRef Name=\"Modified\" />" +
				"    <FieldRef Name=\"ModifiedBy\" />";

			const string expected =
				"  <FieldRef Name=\"Modified\" />" +
				"  <FieldRef Name=\"ModifiedBy\" />" +
				"  <FieldRef Name=\"Title\" />" +
				"  <FieldRef Name=\"State\" />";

			var query = Camlex.Query().ViewFields(existingQuery, x => new[] { x["Title"], x["State"] }).ToString();
			Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_several_view_fields_IS_mixed_with_several_view_fields_with_parent_tag_correctly()
		{
			const string existingQuery =
				"    <FieldRef Name=\"Modified\" />" +
				"    <FieldRef Name=\"ModifiedBy\" />";

			const string expected =
				"<ViewFields>" +
				"  <FieldRef Name=\"Modified\" />" +
				"  <FieldRef Name=\"ModifiedBy\" />" +
				"  <FieldRef Name=\"Title\" />" +
				"  <FieldRef Name=\"State\" />" +
				"</ViewFields>";

			var query = Camlex.Query().ViewFields(existingQuery, x => new[] { x["Title"], x["State"] }, true).ToString();
			Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_view_fields_collection_IS_mixed_with_several_view_fields_correctly()
		{
			const string existingQuery =
				"    <FieldRef Name=\"Modified\" />" +
				"    <FieldRef Name=\"ModifiedBy\" />";

			const string expected =
				"  <FieldRef Name=\"Modified\" />" +
				"  <FieldRef Name=\"ModifiedBy\" />" +
				"  <FieldRef Name=\"Title\" />" +
				"  <FieldRef Name=\"State\" />";

			var exprs = new List<string>();
			exprs.Add("Title");
			exprs.Add("State");

			var query = Camlex.Query().ViewFields(existingQuery, exprs).ToString();
			Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_view_fields_collection_IS_mixed_with_several_view_fields_with_parent_tag_correctly()
		{
			const string existingQuery =
				"    <FieldRef Name=\"Modified\" />" +
				"    <FieldRef Name=\"ModifiedBy\" />";

			const string expected =
				"<ViewFields>" +
				"  <FieldRef Name=\"Modified\" />" +
				"  <FieldRef Name=\"ModifiedBy\" />" +
				"  <FieldRef Name=\"Title\" />" +
				"  <FieldRef Name=\"State\" />" +
				"</ViewFields>";

			var exprs = new List<string>();
			exprs.Add("Title");
			exprs.Add("State");

			var query = Camlex.Query().ViewFields(existingQuery, exprs, true).ToString();
			Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		// GUID not support in Client Object Model
		//[Test]
		//public void test_THAT_view_fields_collection_IS_mixed_with_several_view_fields_guids_correctly()
		//{
		//    const string existingQuery =
		//        "    <FieldRef Name=\"Modified\" />" +
		//        "    <FieldRef Name=\"ModifiedBy\" />";

		//    const string expected =
		//        "  <FieldRef Name=\"Modified\" />" +
		//        "  <FieldRef Name=\"ModifiedBy\" />" +
		//        "  <FieldRef ID=\"5a2c145b-d9c1-4dfd-a2d7-d4aed9e5aa78\" />" +
		//        "  <FieldRef ID=\"19a4ad63-23b9-4c02-8753-bb7c3a64cd86\" />";

		//    var exprs = new List<Guid>();
		//    exprs.Add(new Guid("{5A2C145B-D9C1-4dfd-A2D7-D4AED9E5AA78}"));
		//    exprs.Add(new Guid("{19A4AD63-23B9-4c02-8753-BB7C3A64CD86}"));

		//    var query = Camlex.Query().ViewFields(existingQuery, exprs).ToString();
		//    Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
		//}

		[Test]
		[ExpectedException(typeof(IncorrectCamlException))]
		public void test_WHEN_view_fields_is_not_provided_THEN_exception_is_thrown()
		{
			const string existingQuery =
				"   <Where>" +
				"       <Eq>" +
				"           <FieldRef Name=\"Title\" />" +
				"           <Value Type=\"Text\">testValue</Value>" +
				"       </Eq>" +
				"   </Where>";
			var query = Camlex.Query().ViewFields(existingQuery, x => x["Title"]).ToString();
		}
	}
}
