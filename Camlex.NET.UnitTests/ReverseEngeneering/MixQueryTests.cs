using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET.UnitTests.Helpers;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering
{
    [TestFixture]
    public class MixQueryTests
    {
        [Test]
        public void test_THAT_existing_single_eq_expression_IS_mixed_with_single_expression_correctly_using_and()
        {
            string existingQuery =
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Title\" />" +
                "           <Value Type=\"Text\">testValue</Value>" +
                "       </Eq>" +
                "   </Where>";

            string expected =
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

            var query = Camlex.Query().WhereAll(existingQuery, x => (string) x["Title"] == "foo").ToString();
            Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_existing_several_expressions_ARE_mixed_with_several_expressions_correctly_using_and()
        {
            string existingQuery =
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

            string expected =
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
    }
}
