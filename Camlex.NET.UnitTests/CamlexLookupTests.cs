using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET.UnitTests.Helpers;
using NUnit.Framework;

namespace CamlexNET.UnitTests
{
    [TestFixture]
    public class CamlexLookupTests
    {
        [Test]
        public void test_THAT_lookup_id_IS_translated_successfully()
        {
            string caml = Camlex.Query().Where(x => x["Ref"] == (DataTypes.LookupId)"123").ToString();

            string expected =
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Ref\" LookupId=\"True\" />" +
                "           <Value Type=\"Lookup\">123</Value>" +
                "       </Eq>" +
                "   </Where>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_lookup_value_IS_translated_successfully()
        {
            string caml = Camlex.Query().Where(x => x["Ref"] == (DataTypes.LookupValue)"123").ToString();

            string expected =
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Ref\" />" +
                "           <Value Type=\"Lookup\">123</Value>" +
                "       </Eq>" +
                "   </Where>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}
