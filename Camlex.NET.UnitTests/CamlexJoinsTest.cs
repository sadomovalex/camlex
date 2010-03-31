using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CamlexNET.UnitTests.Helpers;
using Microsoft.SharePoint;
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

            string expected =
                //               "<Query>" +
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
               "    </Where>";
            //               "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_expression_with_2_andalso_and_1_orelse_ARE_translated_sucessfully()
        {
            string caml = Camlex.Query().Where(x => ((string)x["Title"] == "testValue" &&
                                            (int)x["Count1"] == 1) || (int)x["Count2"] == 2).ToString();

            string expected =
                //                "<Query>" +
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
                "   </Where>";
            //                "</Query>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_join_all_IS_translated_sucessfully()
        {
            var ids = new List<int> {1, 2, 3};
            var expressions = new List<Expression<Func<SPListItem, bool>>>();

            foreach (int i in ids)
            {
                int i1 = i;
                expressions.Add(x => (int)x["ID"] == i1);
            }

            string caml = Camlex.Query().WhereAll(expressions).ToString();

            string expected =
                "   <Where>" +
                "       <And>" +
                "           <And>" +
                "               <Eq>" +
                "                   <FieldRef Name=\"ID\" />" +
                "                   <Value Type=\"Integer\">1</Value>" +
                "               </Eq>" +
                "               <Eq>" +
                "                   <FieldRef Name=\"ID\" />" +
                "                   <Value Type=\"Integer\">2</Value>" +
                "               </Eq>" +
                "           </And>" +
                "           <Eq>" +
                "               <FieldRef Name=\"ID\" />" +
                "               <Value Type=\"Integer\">3</Value>" +
                "           </Eq>" +
                "       </And>" +
                "   </Where>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_join_any_IS_translated_sucessfully()
        {
            var ids = new List<int> { 1, 2, 3 };
            var expressions = new List<Expression<Func<SPListItem, bool>>>();

            foreach (int i in ids)
            {
                int i1 = i;
                expressions.Add(x => (int)x["ID"] == i1);
            }

            string caml = Camlex.Query().WhereAny(expressions).ToString();

            string expected =
                "   <Where>" +
                "       <Or>" +
                "           <Or>" +
                "               <Eq>" +
                "                   <FieldRef Name=\"ID\" />" +
                "                   <Value Type=\"Integer\">1</Value>" +
                "               </Eq>" +
                "               <Eq>" +
                "                   <FieldRef Name=\"ID\" />" +
                "                   <Value Type=\"Integer\">2</Value>" +
                "               </Eq>" +
                "           </Or>" +
                "           <Eq>" +
                "               <FieldRef Name=\"ID\" />" +
                "               <Value Type=\"Integer\">3</Value>" +
                "           </Eq>" +
                "       </Or>" +
                "   </Where>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        [ExpectedException(typeof(EmptyExpressionsListException))]
        public void test_WHEN_expressions_list_is_empty_THEN_exception_is_thrown()
        {
            var expressions = new List<Expression<Func<SPListItem, bool>>>();
            Camlex.Query().WhereAny(expressions).ToString();
        }

        [Test]
        [ExpectedException(typeof(EmptyExpressionsListException))]
        public void test_WHEN_expressions_list_is_null_THEN_exception_is_thrown()
        {
            Camlex.Query().WhereAny(null).ToString();
        }

        [Test]
        public void test_THAT_join_any_with_1_element_IS_translated_sucessfully()
        {
            var ids = new List<int> { 1 };
            var expressions = new List<Expression<Func<SPListItem, bool>>>();

            foreach (int i in ids)
            {
                int i1 = i;
                expressions.Add(x => (int)x["ID"] == i1);
            }

            string caml = Camlex.Query().WhereAny(expressions).ToString();

            string expected =
                "   <Where>" +
                "               <Eq>" +
                "                   <FieldRef Name=\"ID\" />" +
                "                   <Value Type=\"Integer\">1</Value>" +
                "               </Eq>" +
                "   </Where>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_join_all_with_1_element_IS_translated_sucessfully()
        {
            var ids = new List<int> { 1 };
            var expressions = new List<Expression<Func<SPListItem, bool>>>();

            foreach (int i in ids)
            {
                int i1 = i;
                expressions.Add(x => (int)x["ID"] == i1);
            }

            string caml = Camlex.Query().WhereAll(expressions).ToString();

            string expected =
                "   <Where>" +
                "               <Eq>" +
                "                   <FieldRef Name=\"ID\" />" +
                "                   <Value Type=\"Integer\">1</Value>" +
                "               </Eq>" +
                "   </Where>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

//        [Test]
//        [ExpectedException(typeof(DifferentArgumentsNamesExceptions))]
//        public void test_WHEN_expressions_contain_different_arguments_THEN_exception_is_thrown()
//        {
//            var ids = new List<int> {1, 2, 3};
//            var expressions = new List<Expression<Func<SPListItem, bool>>>();
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
            var expressions = new List<Expression<Func<SPListItem, bool>>>();

            expressions.Add(x => (int)x["ID"] == 1);
            expressions.Add(y => (int)y["ID"] == 2);
            expressions.Add(z => (int)z["ID"] == 3);

            string caml = Camlex.Query().WhereAny(expressions).ToString();

            string expected =
                "   <Where>" +
                "       <Or>" +
                "           <Or>" +
                "               <Eq>" +
                "                   <FieldRef Name=\"ID\" />" +
                "                   <Value Type=\"Integer\">1</Value>" +
                "               </Eq>" +
                "               <Eq>" +
                "                   <FieldRef Name=\"ID\" />" +
                "                   <Value Type=\"Integer\">2</Value>" +
                "               </Eq>" +
                "           </Or>" +
                "           <Eq>" +
                "               <FieldRef Name=\"ID\" />" +
                "               <Value Type=\"Integer\">3</Value>" +
                "           </Eq>" +
                "       </Or>" +
                "   </Where>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}
