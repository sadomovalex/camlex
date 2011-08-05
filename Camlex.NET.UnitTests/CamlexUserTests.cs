using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET.UnitTests.Helpers;
using NUnit.Framework;

namespace CamlexNET.UnitTests
{
    [TestFixture]
    public class CamlexUserTests
    {
        [Test]
        public void test_THAT_user_id_field_ref_with_name_IS_translated_successfully()
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
        public void test_THAT_lookup_id_field_ref_with_guid_IS_translated_successfully()
        {
            var guid = new Guid("{4feaf1f3-5b04-4d93-b0fc-4e48d0c60eed}");
            string caml = Camlex.Query().Where(x => x[guid] == (DataTypes.LookupId)"123").ToString();

            string expected =
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef ID=\"4feaf1f3-5b04-4d93-b0fc-4e48d0c60eed\" LookupId=\"True\" />" +
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
