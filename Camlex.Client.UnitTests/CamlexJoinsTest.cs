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
using CamlexNET.Impl.Helpers;
using CamlexNET.UnitTests.Helpers;
using Microsoft.SharePoint.Client;
using NUnit.Framework;

namespace CamlexNET.UnitTests
{
	[TestFixture]
	public class CamlexJoinsTest
	{
		[Test]
		public void test_THAT_2_eq_expression_with_andalso_ARE_translated_sucessfully()
		{
			string caml = Camlex.Query().Where(x => (string)x["Title"] == "testValue" &&
											(int)x["Count"] == 1).ToString();

			const string expected =
               "<Query>" +
			   "  <Where>" +
			   "    <And>" +
			   "        <Eq>" +
			   "            <FieldRef Name=\"Title\" />" +
			   "            <Value Type=\"Text\">testValue</Value>" +
			   "        </Eq>" +
			   "        <Eq>" +
			   "            <FieldRef Name=\"Count\" />" +
			   "            <Value Type=\"Integer\">1</Value>" +
			   "        </Eq>" +
			   "    </And>" +
			   "  </Where>" +
               "</Query>";

			Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_expression_with_2_andalso_and_1_orelse_ARE_translated_sucessfully()
		{
			string caml = Camlex.Query().Where(x => ((string)x["Title"] == "testValue" &&
											(int)x["Count1"] == 1) || (int)x["Count2"] == 2).ToString();

			const string expected =
                "<Query>" +
                "  <Where>" +
				"    <Or>" +
				"        <And>" +
				"            <Eq>" +
				"                <FieldRef Name=\"Title\" />" +
				"                <Value Type=\"Text\">testValue</Value>" +
				"            </Eq>" +
				"            <Eq>" +
				"                <FieldRef Name=\"Count1\" />" +
				"                <Value Type=\"Integer\">1</Value>" +
				"            </Eq>" +
				"        </And>" +
				"        <Eq>" +
				"            <FieldRef Name=\"Count2\" />" +
				"            <Value Type=\"Integer\">2</Value>" +
				"        </Eq>" +
				"    </Or>" +
                "  </Where>" +
                "</Query>";

			Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_join_all_IS_translated_sucessfully()
		{
			var ids = new List<int> { 1, 2, 3 };
			var expressions = new List<Expression<Func<ListItem, bool>>>();

			foreach (int i in ids)
			{
				int i1 = i;
				expressions.Add(x => (int)x["ID"] == i1);
			}

			string caml = Camlex.Query().WhereAll(expressions).ToString();

			const string expected =
                "<Query>" +
                "  <Where>" +
				"    <And>" +
				"        <And>" +
				"            <Eq>" +
				"                <FieldRef Name=\"ID\" />" +
				"                <Value Type=\"Integer\">1</Value>" +
				"            </Eq>" +
				"            <Eq>" +
				"                <FieldRef Name=\"ID\" />" +
				"                <Value Type=\"Integer\">2</Value>" +
				"            </Eq>" +
				"        </And>" +
				"        <Eq>" +
				"            <FieldRef Name=\"ID\" />" +
				"            <Value Type=\"Integer\">3</Value>" +
				"        </Eq>" +
				"    </And>" +
                "  </Where>" +
                "</Query>";

			Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_join_any_IS_translated_sucessfully()
		{
			var ids = new List<int> { 1, 2, 3 };
			var expressions = new List<Expression<Func<ListItem, bool>>>();

			foreach (int i in ids)
			{
				int i1 = i;
				expressions.Add(x => (int)x["ID"] == i1);
			}

			string caml = Camlex.Query().WhereAny(expressions).ToString();

			const string expected =
                "<Query>" +
                "  <Where>" +
				"    <Or>" +
				"        <Or>" +
				"            <Eq>" +
				"                <FieldRef Name=\"ID\" />" +
				"                <Value Type=\"Integer\">1</Value>" +
				"            </Eq>" +
				"            <Eq>" +
				"                <FieldRef Name=\"ID\" />" +
				"                <Value Type=\"Integer\">2</Value>" +
				"            </Eq>" +
				"        </Or>" +
				"        <Eq>" +
				"            <FieldRef Name=\"ID\" />" +
				"            <Value Type=\"Integer\">3</Value>" +
				"        </Eq>" +
				"    </Or>" +
                "  </Where>" +
                "</Query>";

			Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		[ExpectedException(typeof(EmptyExpressionsListException))]
		public void test_WHEN_expressions_list_is_empty_THEN_exception_is_thrown()
		{
			var expressions = new List<Expression<Func<ListItem, bool>>>();
			Camlex.Query().WhereAny(expressions).ToString();
		}

		[Test]
		[ExpectedException(typeof(EmptyExpressionsListException))]
		public void test_WHEN_expressions_list_is_null_THEN_exception_is_thrown()
		{
            Camlex.Query().WhereAny((IEnumerable<Expression<Func<ListItem, bool>>>)null).ToString();
		}

		[Test]
		public void test_THAT_join_any_with_1_element_IS_translated_sucessfully()
		{
			var ids = new List<int> { 1 };
			var expressions = new List<Expression<Func<ListItem, bool>>>();

			foreach (int i in ids)
			{
				int i1 = i;
				expressions.Add(x => (int)x["ID"] == i1);
			}

			string caml = Camlex.Query().WhereAny(expressions).ToString();

			const string expected =
                "<Query>" +
                "  <Where>" +
				"    <Eq>" +
				"        <FieldRef Name=\"ID\" />" +
				"        <Value Type=\"Integer\">1</Value>" +
				"    </Eq>" +
                "  </Where>" +
                "</Query>";

			Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_join_all_with_1_element_IS_translated_sucessfully()
		{
			var ids = new List<int> { 1 };
			var expressions = new List<Expression<Func<ListItem, bool>>>();

			foreach (int i in ids)
			{
				int i1 = i;
				expressions.Add(x => (int)x["ID"] == i1);
			}

			string caml = Camlex.Query().WhereAll(expressions).ToString();

			const string expected =
                "<Query>" +
                "  <Where>" +
				"    <Eq>" +
				"        <FieldRef Name=\"ID\" />" +
				"        <Value Type=\"Integer\">1</Value>" +
				"    </Eq>" +
                "  </Where>" +
                "</Query>";

			Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		//        [Test]
		//        [ExpectedException(typeof(DifferentArgumentsNamesExceptions))]
		//        public void test_WHEN_expressions_contain_different_arguments_THEN_exception_is_thrown()
		//        {
		//            var ids = new List<int> {1, 2, 3};
		//            var expressions = new List<Expression<Func<ListItem, bool>>>();
		//
		//            expressions.Add(x => (int) x["ID"] == 1);
		//            expressions.Add(x => (int) x["ID"] == 2);
		//            expressions.Add(y => (int) y["ID"] == 3);
		//
		//            Camlex.Query().WhereAny(expressions).ToString();
		//        }

		[Test]
		public void test_THAT_expressions_with_different_arguments_IS_translated_successfully()
		{
			var expressions = new Expression<Func<ListItem, bool>>[]
			{
				x => (int)x["ID"] == 1,
				y => (int)y["ID"] == 2,
				z => (int)z["ID"] == 3
			};

			string caml = Camlex.Query().WhereAny(expressions).ToString();

			const string expected =
                "<Query>" +
                "  <Where>" +
				"    <Or>" +
				"        <Or>" +
				"            <Eq>" +
				"                <FieldRef Name=\"ID\" />" +
				"                <Value Type=\"Integer\">1</Value>" +
				"            </Eq>" +
				"            <Eq>" +
				"                <FieldRef Name=\"ID\" />" +
				"                <Value Type=\"Integer\">2</Value>" +
				"            </Eq>" +
				"        </Or>" +
				"        <Eq>" +
				"            <FieldRef Name=\"ID\" />" +
				"            <Value Type=\"Integer\">3</Value>" +
				"        </Eq>" +
				"    </Or>" +
                "  </Where>" +
                "</Query>";

			Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_expressions_with_different_methods_IS_translated_successfully()
		{
			var expressionsList = new List<Expression<Func<ListItem, bool>>>
			{
				x => (int) x["ID"] == 1,
				y => (int) y["ID"] == 2,
				z => (int) z["ID"] == 3
			};
			Expression<Func<ListItem, bool>> additionalExpression = (w => (string)w["Title"] == "Test");

			var combinedExpressions = ExpressionsHelper.CombineOr(expressionsList);
			var finalExpression = ExpressionsHelper.CombineAnd(new[] { combinedExpressions, additionalExpression });
			var caml = Camlex.Query().Where(finalExpression).ToString();

			const string expected =
                "<Query>" +
                "  <Where>" +
				"    <And>" +
				"        <Or>" +
				"            <Or>" +
				"                <Eq>" +
				"                    <FieldRef Name=\"ID\" />" +
				"                    <Value Type=\"Integer\">1</Value>" +
				"                </Eq>" +
				"                <Eq>" +
				"                    <FieldRef Name=\"ID\" />" +
				"                    <Value Type=\"Integer\">2</Value>" +
				"                </Eq>" +
				"            </Or>" +
				"            <Eq>" +
				"                <FieldRef Name=\"ID\" />" +
				"                <Value Type=\"Integer\">3</Value>" +
				"            </Eq>" +
				"        </Or>" +
				"        <Eq>" +
				"            <FieldRef Name=\"Title\" />" +
				"            <Value Type=\"Text\">Test</Value>" +
				"        </Eq>" +
				"    </And>" +
                "  </Where>" +
                "</Query>";

			Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
		}

        [Test]
        public void test_THAT_join_all_with_string_expressions_ARE_translated_sucessfully()
        {
            var expr = new List<string>();
            expr.Add(
                "<Query>" +
                "  <Where>" +
                "   <Eq>" +
                "       <FieldRef Name=\"ID\" />" +
                "       <Value Type=\"Integer\">1</Value>" +
                "   </Eq>" +
                "  </Where>" +
                "</Query>");
            expr.Add(
                "<Query>" +
                "  <Where>" +
                "   <Eq>" +
                "       <FieldRef Name=\"ID\" />" +
                "       <Value Type=\"Integer\">2</Value>" +
                "   </Eq>" +
                "  </Where>" +
                "</Query>");
            expr.Add(
                "<Query>" +
                "  <Where>" +
                "   <Eq>" +
                "       <FieldRef Name=\"ID\" />" +
                "       <Value Type=\"Integer\">3</Value>" +
                "   </Eq>" +
                "  </Where>" +
                "</Query>");

            string caml = Camlex.Query().WhereAll(expr).ToString();

            string expected =
                "<Query>" +
                "  <Where>" +
                "  <And>" +
                "    <And>" +
                "      <Eq>" +
                "        <FieldRef Name=\"ID\" />" +
                "        <Value Type=\"Integer\">1</Value>" +
                "      </Eq>" +
                "      <Eq>" +
                "        <FieldRef Name=\"ID\" />" +
                "        <Value Type=\"Integer\">2</Value>" +
                "      </Eq>" +
                "    </And>" +
                "    <Eq>" +
                "      <FieldRef Name=\"ID\" />" +
                "      <Value Type=\"Integer\">3</Value>" +
                "    </Eq>" +
                "  </And>" +
                "  </Where>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_join_any_with_string_expressions_IS_translated_sucessfully()
        {
            var expr = new List<string>();
            expr.Add(
                "<Query>" +
                "  <Where>" +
                "   <Eq>" +
                "       <FieldRef Name=\"ID\" />" +
                "       <Value Type=\"Integer\">1</Value>" +
                "   </Eq>" +
                "  </Where>" +
                "</Query>");
            expr.Add(
                "<Query>" +
                "  <Where>" +
                "   <Eq>" +
                "       <FieldRef Name=\"ID\" />" +
                "       <Value Type=\"Integer\">2</Value>" +
                "   </Eq>" +
                "  </Where>" +
                "</Query>");
            expr.Add(
                "<Query>" +
                "  <Where>" +
                "   <Eq>" +
                "       <FieldRef Name=\"ID\" />" +
                "       <Value Type=\"Integer\">3</Value>" +
                "   </Eq>" +
                "  </Where>" +
                "</Query>");

            string caml = Camlex.Query().WhereAny(expr).ToString();

            string expected =
                "<Query>" +
                "  <Where>" +
                "  <Or>" +
                "    <Or>" +
                "      <Eq>" +
                "        <FieldRef Name=\"ID\" />" +
                "        <Value Type=\"Integer\">1</Value>" +
                "      </Eq>" +
                "      <Eq>" +
                "        <FieldRef Name=\"ID\" />" +
                "        <Value Type=\"Integer\">2</Value>" +
                "      </Eq>" +
                "    </Or>" +
                "    <Eq>" +
                "      <FieldRef Name=\"ID\" />" +
                "      <Value Type=\"Integer\">3</Value>" +
                "    </Eq>" +
                "  </Or>" +
                "  </Where>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        [ExpectedException(typeof(EmptyExpressionsListException))]
        public void test_WHEN_list_of_expressions_is_null_THEN_exception_is_thrown()
        {
            Camlex.Query().WhereAny((IEnumerable<string>)null).ToString();
        }

        [Test]
        [ExpectedException(typeof(EmptyExpressionsListException))]
        public void test_WHEN_list_of_expressions_is_empty_THEN_exception_is_thrown()
        {
            Camlex.Query().WhereAny(new string[]{}).ToString();
        }

        [Test]
        public void test_THAT_join_all_with_true_explicit_boolean_cast_expressions_ARE_translated_sucessfully()
        {
            var expressions = new List<Expression<Func<SPListItem, bool>>>();
            Enumerable.Range(0, 3).ToList().ForEach(i => expressions.Add(x => (bool)x["foo" + i]));

            string caml = Camlex.Query().WhereAll(expressions).ToString();

            string expected =
                "   <Where>" +
                "       <And>" +
                "           <And>" +
                "               <Eq>" +
                "                   <FieldRef Name=\"foo0\" />" +
                "                   <Value Type=\"Boolean\">1</Value>" +
                "               </Eq>" +
                "               <Eq>" +
                "                   <FieldRef Name=\"foo1\" />" +
                "                   <Value Type=\"Boolean\">1</Value>" +
                "               </Eq>" +
                "           </And>" +
                "           <Eq>" +
                "               <FieldRef Name=\"foo2\" />" +
                "               <Value Type=\"Boolean\">1</Value>" +
                "           </Eq>" +
                "       </And>" +
                "   </Where>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_join_all_with_false_explicit_boolean_cast_expressions_ARE_translated_sucessfully()
        {
            var expressions = new List<Expression<Func<SPListItem, bool>>>();
            Enumerable.Range(1, 3).ToList().ForEach(i => expressions.Add(x => !(bool)x["foo" + i]));

            string caml = Camlex.Query().WhereAll(expressions).ToString();

            string expected =
                "   <Where>" +
                "       <And>" +
                "           <And>" +
                "               <Eq>" +
                "                   <FieldRef Name=\"foo1\" />" +
                "                   <Value Type=\"Boolean\">0</Value>" +
                "               </Eq>" +
                "               <Eq>" +
                "                   <FieldRef Name=\"foo2\" />" +
                "                   <Value Type=\"Boolean\">0</Value>" +
                "               </Eq>" +
                "           </And>" +
                "           <Eq>" +
                "               <FieldRef Name=\"foo3\" />" +
                "               <Value Type=\"Boolean\">0</Value>" +
                "           </Eq>" +
                "       </And>" +
                "   </Where>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_mixed_join_explicit_boolean_cast_expressions_ARE_translated_sucessfully()
        {
            string caml = Camlex.Query().Where(x => ((bool)x["foo1"] && !(bool)x["foo2"]) || (bool)x["foo3"]).ToString();

            string expected =
                "   <Where>" +
                "       <Or>" +
                "           <And>" +
                "               <Eq>" +
                "                   <FieldRef Name=\"foo1\" />" +
                "                   <Value Type=\"Boolean\">1</Value>" +
                "               </Eq>" +
                "               <Eq>" +
                "                   <FieldRef Name=\"foo2\" />" +
                "                   <Value Type=\"Boolean\">0</Value>" +
                "               </Eq>" +
                "           </And>" +
                "           <Eq>" +
                "               <FieldRef Name=\"foo3\" />" +
                "               <Value Type=\"Boolean\">1</Value>" +
                "           </Eq>" +
                "       </Or>" +
                "   </Where>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
	}
}
