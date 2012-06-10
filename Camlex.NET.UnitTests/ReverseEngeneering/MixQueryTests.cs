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
        public void test_THAT_existing_single_eq_expression_IS_mixed_with_single_expression_correctly()
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
    }
}
