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
            string caml = Camlex.Where(x => (string)x["Title"] == "testValue");

            string expected =
               "<Where>" +
               "   <Eq>" +
               "      <FieldRef Name=\"Title\" />" +
               "      <Value Type=\"Text\">testValue</Value>" +
               "   </Eq>" +
               "</Where>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_2_eq_expression_with_andalso_ARE_translated_sucessfully()
        {
            string caml = Camlex.Where(x => (string) x["Title"] == "testValue" &&
                                            (int) x["Count"] == 1);

            string expected =
               "<Where>" +
               "    <And>" +
               "        <Eq>" +
               "            <FieldRef Name=\"Title\" />" +
               "            <Value Type=\"Text\">testValue</Value>" +
               "        </Eq>" +
               "        <Eq>" +
               "            <FieldRef Name=\"Count\" />" +
               "            <Value Type=\"Integer\">1</Value>" +
               "        </Eq>" +
               "   </And>" +
               "</Where>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_expression_with_2_andalso_and_1_orelse_ARE_translated_sucessfully()
        {
            string caml = Camlex.Where(x => ((string)x["Title"] == "testValue" &&
                                            (int)x["Count1"] == 1) || (int)x["Count2"] == 2);

            string expected =
               "<Where>" +
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
               "   </Or>" +
               "</Where>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_single_eq_expression_with_variable_IS_translated_sucessfully()
        {
            string val = "testValue";
            string caml = Camlex.Where(x => (string)x["Title"] == val);

            string expected =
               "<Where>" +
               "   <Eq>" +
               "      <FieldRef Name=\"Title\" />" +
               "      <Value Type=\"Text\">testValue</Value>" +
               "   </Eq>" +
               "</Where>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_single_eq_expression_with_parameterless_method_call_IS_translated_sucessfully()
        {
            string caml = Camlex.Where(x => (int)x["Count"] == val());

            string expected =
               "<Where>" +
               "   <Eq>" +
               "      <FieldRef Name=\"Count\" />" +
               "      <Value Type=\"Integer\">123</Value>" +
               "   </Eq>" +
               "</Where>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        private int val()
        {
            return 123;
        }
    }
}
