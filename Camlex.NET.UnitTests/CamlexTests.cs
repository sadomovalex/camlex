using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Camlex.NET.UnitTests.Helpers;
using NUnit.Framework;
using Camlex.NET;

namespace Camlex.NET.UnitTests
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
        public void test_THAT_single_eq_expression_with_variable_IS_translated_sucessfully()
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
                "           <Value Type=\"Text\">Camlex.NET.UnitTests.CamlexTests+Foo</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
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
    }
}
