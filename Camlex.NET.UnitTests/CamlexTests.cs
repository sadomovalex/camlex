﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET.UnitTests.Helpers;
using NUnit.Framework;
using CamlexNET;

namespace CamlexNET.UnitTests
{
    [TestFixture]
    public class CamlexTests
    {
        [Test]
        public void test_THAT_single_eq_expression_IS_translated_sucessfully()
        {
            string caml = Camlex.Query().Where(x => (string)x["Title"] == "testValue").ToString();

            string expected =
                "<Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Title\" />" +
                "           <Value Type=\"Text\">testValue</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_2_eq_expression_with_andalso_ARE_translated_sucessfully()
        {
            string caml = Camlex.Query().Where(x => (string) x["Title"] == "testValue" &&
                                            (int) x["Count"] == 1).ToString();

            string expected =
               "<Query>" +
               "    <Where>" +
               "        <And>" +
               "            <Eq>" +
               "                <FieldRef Name=\"Title\" />" +
               "                <Value Type=\"Text\">testValue</Value>" +
               "            </Eq>" +
               "            <Eq>" +
               "                <FieldRef Name=\"Count\" />" +
               "                <Value Type=\"Integer\">1</Value>" +
               "            </Eq>" +
               "        </And>" +
               "    </Where>"+
               "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_expression_with_2_andalso_and_1_orelse_ARE_translated_sucessfully()
        {
            string caml = Camlex.Query().Where(x => ((string)x["Title"] == "testValue" &&
                                            (int)x["Count1"] == 1) || (int)x["Count2"] == 2).ToString();

            string expected =
                "<Query>" +
                "   <Where>" +
                "       <Or>" +
                "           <And>" +
                "               <Eq>" +
                "                   <FieldRef Name=\"Title\" />" +
                "                   <Value Type=\"Text\">testValue</Value>" +
                "               </Eq>" +
                "               <Eq>" +
                "                   <FieldRef Name=\"Count1\" />" +
                "                   <Value Type=\"Integer\">1</Value>" +
                "               </Eq>" +
                "           </And>" +
                "           <Eq>" +
                "               <FieldRef Name=\"Count2\" />" +
                "               <Value Type=\"Integer\">2</Value>" +
                "           </Eq>" +
                "       </Or>" +
                "   </Where>"+
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_single_eq_expression_with_string_variable_IS_translated_sucessfully()
        {
            string val = "testValue";
            string caml = Camlex.Query().Where(x => (string)x["Title"] == val).ToString();

            string expected =
                "<Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Title\" />" +
                "           <Value Type=\"Text\">testValue</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_single_eq_expression_with_string_variable_which_is_null_IS_translated_sucessfully()
        {
            string val = null;
            string caml = Camlex.Query().Where(x => (string)x["Title"] == val).ToString();

            string expected =
                "<Query>" +
                "   <Where>" +
                "       <IsNull>" +
                "           <FieldRef Name=\"Title\" />" +
                "       </IsNull>" +
                "   </Where>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        [ExpectedException(typeof(NullValueOperandCannotBeTranslatedToCamlException))]
        public void test_WHEN_eq_expression_with_null_variable_casted_to_string_based_syntax_THEN_exception_is_thrown()
        {
            string val = null;
            Camlex.Query().Where(x => x["Title"] == (DataTypes.Text)val).ToString();
        }

        [Test]
        public void test_THAT_single_eq_expression_with_indexer_with_string_variable_IS_translated_sucessfully()
        {
            string val = "Title";
            string caml = Camlex.Query().Where(x => (string)x[val] == val).ToString();

            string expected =
                "<Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Title\" />" +
                "           <Value Type=\"Text\">Title</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_string_based_single_eq_expression_with_variable_IS_translated_sucessfully()
        {
            string val = "123";
            string caml = Camlex.Query().Where(x => x["Count"] == (DataTypes.Integer)val).ToString();

            string expected =
                "<Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Count\" />" +
                "           <Value Type=\"Integer\">123</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_string_based_single_eq_expression_with_variable_both_for_lvalue_and_rvalue_IS_translated_sucessfully()
        {
            string val = "Title";
            string caml = Camlex.Query().Where(x => x[val] == (DataTypes.Note)val).ToString();

            string expected =
                "<Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Title\" />" +
                "           <Value Type=\"Note\">Title</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_single_eq_expression_with_parameterless_method_call_IS_translated_sucessfully()
        {
            string caml = Camlex.Query().Where(x => (int)x["Count"] == val1()).ToString();

            string expected =
                "<Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Count\" />" +
                "           <Value Type=\"Integer\">123</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        private int val1()
        {
            return 123;
        }

        [Test]
        public void test_THAT_single_eq_expression_with_parameterless_method_call_both_for_lvalue_and_rvalue_IS_translated_sucessfully()
        {
            string caml = Camlex.Query().Where(x => (string)x[val4()] == val4()).ToString();

            string expected =
                "<Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Foo\" />" +
                "           <Value Type=\"Text\">Foo</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        private string val4()
        {
            return "Foo";
        }

        [Test]
        public void test_THAT_single_eq_expression_with_parameterless_method_call_which_returns_boolean_IS_translated_sucessfully()
        {
            string caml = Camlex.Query().Where(x => (bool)x["Foo"] == val2()).ToString();

            string expected =
                "<Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Foo\" />" +
                "           <Value Type=\"Boolean\">0</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        private bool val2()
        {
            return false;
        }

        [Test]
        public void test_THAT_single_eq_expression_with_1_parameter_method_call_IS_translated_sucessfully()
        {
            string caml = Camlex.Query().Where(x => (int)x["Count"] == val2(456)).ToString();

            string expected =
                "<Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Count\" />" +
                "           <Value Type=\"Integer\">456</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        private int val2(int someParam)
        {
            return someParam;
        }

        [Test]
        public void test_THAT_single_eq_expression_with_2_parameters_internal_method_call_IS_translated_sucessfully()
        {
            Func<int, int, int> val3 = (i, j) => i + j;
            string caml = Camlex.Query().Where(x => (int)x["Count"] == val3(2, 3)).ToString();

            string expected =
                "<Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Count\" />" +
                "           <Value Type=\"Integer\">5</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        class Foo
        {
            public int Prop { get; set; }
            public string Func()
            {
                return string.Format("{0}", GetType());
            }
        }

        [Test]
        public void test_THAT_single_eq_expression_with_class_property_call_IS_translated_sucessfully()
        {
            var f = new Foo();
            f.Prop = 1;
            string caml = Camlex.Query().Where(x => (int)x["Count"] == f.Prop).ToString();

            string expected =
                "<Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Count\" />" +
                "           <Value Type=\"Integer\">1</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_single_eq_expression_with_class_method_call_IS_translated_sucessfully()
        {
            Foo f = new Foo();
            string caml = Camlex.Query().Where(x => (string)x["Title"] == f.Func()).ToString();

            string expected =
                "<Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Title\" />" +
                "           <Value Type=\"Text\">CamlexNET.UnitTests.CamlexTests+Foo</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_single_eq_expression_with_ternary_operator_IS_translated_sucessfully()
        {
            bool b = true;
            string caml = Camlex.Query().Where(x => (string)x["Title"] == (b ? val3() : "foo")).ToString();

            string expected =
                "<Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Title\" />" +
                "           <Value Type=\"Text\">val3</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        private string val3()
        {
            return "val3";
        }

        [Test]
        public void test_THAT_neq_or_isnull_expression_IS_translated_sucessfully()
        {
            string caml =
                Camlex.Query().Where(x => (string)x["Status"] != "Completed" || x["Status"] == null).ToString();

            string expected =
                "<Query>" +
                "  <Where>" +
                "    <Or>" +
                "      <Neq>" +
                "        <FieldRef Name=\"Status\" />" +
                "        <Value Type=\"Text\">Completed</Value>" +
                "      </Neq>" +
                "      <IsNull>" +
                "        <FieldRef Name=\"Status\" />" +
                "      </IsNull>" +
                "     </Or>" +
                "   </Where>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_neq_or_isnull_with_orderby_expression_IS_translated_sucessfully()
        {
            string caml =
                Camlex.Query().Where(x => (string)x["Status"] != "Completed" || x["Status"] == null).
                    OrderBy(x => new[] { x["Modified"] as Camlex.Desc }).ToString();

            string expected =
                "<Query>" +
                "  <Where>" +
                "    <Or>" +
                "      <Neq>" +
                "        <FieldRef Name=\"Status\" />" +
                "        <Value Type=\"Text\">Completed</Value>" +
                "      </Neq>" +
                "      <IsNull>" +
                "        <FieldRef Name=\"Status\" />" +
                "      </IsNull>" +
                "     </Or>" +
                "   </Where>" +
                "  <OrderBy>" +
                "    <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
                "  </OrderBy>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_beginswith_expression_IS_translated_sucessfully()
        {
            var caml = Camlex.Query().Where(x => ((string)x["Count"]).StartsWith("foo")).ToString();

            const string expected =
                "<Query>" +
                "   <Where>" +
                "       <BeginsWith>" +
                "           <FieldRef Name=\"Count\" />" +
                "           <Value Type=\"Text\">foo</Value>" +
                "       </BeginsWith>" +
                "   </Where>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_contains_expression_IS_translated_sucessfully()
        {
            var caml = Camlex.Query().Where(x => ((string)x["Count"]).Contains("foo")).ToString();

            const string expected =
                "<Query>" +
                "   <Where>" +
                "       <Contains>" +
                "           <FieldRef Name=\"Count\" />" +
                "           <Value Type=\"Text\">foo</Value>" +
                "       </Contains>" +
                "   </Where>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_contains_expression_with_variable_as_indexer_param_IS_translated_sucessfully()
        {
            string val = "Count";
            var caml = Camlex.Query().Where(x => ((string)x[val]).Contains("foo")).ToString();

            const string expected =
                "<Query>" +
                "   <Where>" +
                "       <Contains>" +
                "           <FieldRef Name=\"Count\" />" +
                "           <Value Type=\"Text\">foo</Value>" +
                "       </Contains>" +
                "   </Where>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_contains_expression_with_variable_as_method_param_IS_translated_sucessfully()
        {
            string val = "foo";
            var caml = Camlex.Query().Where(x => ((string)x["Count"]).Contains(val)).ToString();

            const string expected =
                "<Query>" +
                "   <Where>" +
                "       <Contains>" +
                "           <FieldRef Name=\"Count\" />" +
                "           <Value Type=\"Text\">foo</Value>" +
                "       </Contains>" +
                "   </Where>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_contains_expression_with_ternary_operator_as_method_param_IS_translated_sucessfully()
        {
            bool b = true;
            var caml = Camlex.Query().Where(x => ((string)x["Count"]).Contains(b ? "foo" : "bar")).ToString();

            const string expected =
                "<Query>" +
                "   <Where>" +
                "       <Contains>" +
                "           <FieldRef Name=\"Count\" />" +
                "           <Value Type=\"Text\">foo</Value>" +
                "       </Contains>" +
                "   </Where>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_contains_and_beginswith_expression_IS_translated_sucessfully()
        {
            var caml =
                Camlex.Query()
                    .Where(x => ((string)x["Title"]).StartsWith("Task") && ((string)x["Project"]).Contains("Camlex"))
                            .ToString();

            const string expected =
                "<Query>" +
                "   <Where>" +
                "       <And>" +
                "           <BeginsWith>" +
                "               <FieldRef Name=\"Title\" />" +
                "               <Value Type=\"Text\">Task</Value>" +
                "           </BeginsWith>" +
                "           <Contains>" +
                "               <FieldRef Name=\"Project\" />" +
                "               <Value Type=\"Text\">Camlex</Value>" +
                "           </Contains>" +
                "       </And>" +
                "   </Where>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_contains_and_isnotnull_expression_IS_translated_sucessfully()
        {
            var caml =
                Camlex.Query()
                    .Where(x => ((string)x["Title"]).StartsWith("Task") && x["Status"] != null)
                            .ToString();

            const string expected =
                "<Query>" +
                "   <Where>" +
                "       <And>" +
                "           <BeginsWith>" +
                "               <FieldRef Name=\"Title\" />" +
                "               <Value Type=\"Text\">Task</Value>" +
                "           </BeginsWith>" +
                "           <IsNotNull>" +
                "               <FieldRef Name=\"Status\" />" +
                "           </IsNotNull>" +
                "       </And>" +
                "   </Where>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_single_orderby_expression_IS_translated_sucessfully()
        {
            var caml = Camlex.Query().OrderBy(x => x["field1"] as Camlex.Desc).ToString();

            var expected =
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

            var expected =
                "<Query>" +
                "  <OrderBy>" +
                "    <FieldRef Name=\"field1\" />" +
                "    <FieldRef Name=\"field2\" Ascending=\"False\" />" +
                "    <FieldRef Name=\"field3\" Ascending=\"True\" />" +
                "  </OrderBy>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_single_groupby_expression_IS_translated_sucessfully()
        {
            var caml = Camlex.Query().GroupBy(x => x["field1"]).ToString();

            var expected =
                "<Query>" +
                "  <GroupBy>" +
                "    <FieldRef Name=\"field1\" />" +
                "  </GroupBy>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_multiple_groupby_expression_IS_translated_sucessfully()
        {
            var caml = Camlex.Query().GroupBy(x => new[] { x["field1"], x["field2"] }, true, 10).ToString();

            var expected =
                "<Query>" +
                "  <GroupBy Collapse=\"True\" GroupLimit=\"10\">" +
                "    <FieldRef Name=\"field1\" />" +
                "    <FieldRef Name=\"field2\" />" +
                "  </GroupBy>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_expression_with_native_and_string_based_syntax_IS_translated_sucessfully()
        {
            var caml =
                Camlex
                    .Query()
                        .Where(x => x["Id"] == (DataTypes.ContentTypeId)"0x05BB17ED9FB8406DA160511C12A3E0C2" ||
                                    x["Description"] != null)
                        .GroupBy(x => x["Title"], true)
                        .OrderBy(x => new[] { x["_Author"], x["AuthoringDate"], x["AssignedTo"] as Camlex.Asc })
                            .ToString();

            var expected =
                "<Query>" +
                "   <Where>" +
                "      <Or>" +
                "         <Eq>" +
                "            <FieldRef Name=\"Id\" />" +
                "            <Value Type=\"ContentTypeId\">0x05BB17ED9FB8406DA160511C12A3E0C2</Value>" +
                "         </Eq>" +
                "         <IsNotNull>" +
                "            <FieldRef Name=\"Description\" />" +
                "         </IsNotNull>" +
                "      </Or>" +
                "   </Where>" +
                "   <OrderBy>" +
                "      <FieldRef Name=\"_Author\" />" +
                "      <FieldRef Name=\"AuthoringDate\" />" +
                "      <FieldRef Name=\"AssignedTo\" Ascending=\"True\" />" +
                "   </OrderBy>" +
                "   <GroupBy Collapse=\"True\">" +
                "      <FieldRef Name=\"Title\" />" +
                "   </GroupBy>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        [ExpectedException(typeof(NonSupportedExpressionException))]
        public void test_WHEN_integer_indexer_param_is_used_THEN_exception_is_thrown()
        {
            string caml = Camlex.Query().Where(x => (string)x[1] == "testValue").ToString();
        }

        [Test]
        public void test_THAT_single_eq_expression_with_arbitrary_expressions_IS_translated_sucessfully()
        {
            string caml = Camlex.Query().Where(x => (string)x[(string)val5()] == "1" + 2.ToString()).ToString();

            string expected =
                "<Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Foo\" />" +
                "           <Value Type=\"Text\">12</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        private object val5()
        {
            return "Foo";
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void test_THAT_expression_with_nested_argument_IS_translated_sucessfully()
        {
            string caml = Camlex.Query().Where(x => (string) x[(string) x["Title"]] == "foo").ToString();
        }
    }
}
