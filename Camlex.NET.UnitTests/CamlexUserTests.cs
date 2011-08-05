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
            string caml = Camlex.Query().Where(x => x["foo"] == (DataTypes.UserId)"123").ToString();

            string expected =
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"foo\" LookupId=\"True\" />" +
                "           <Value Type=\"User\">123</Value>" +
                "       </Eq>" +
                "   </Where>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_user_id_field_ref_with_guid_IS_translated_successfully()
        {
            var guid = new Guid("{4feaf1f3-5b04-4d93-b0fc-4e48d0c60eed}");
            string caml = Camlex.Query().Where(x => x[guid] == (DataTypes.UserId)"123").ToString();

            string expected =
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef ID=\"4feaf1f3-5b04-4d93-b0fc-4e48d0c60eed\" LookupId=\"True\" />" +
                "           <Value Type=\"User\">123</Value>" +
                "       </Eq>" +
                "   </Where>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_expression_with_user_IS_translated_successfully()
        {
            string caml = Camlex.Query().Where(x => x["foo"] == (DataTypes.User)"Foo Bar").ToString();

            string expected =
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"foo\" />" +
                "           <Value Type=\"User\">Foo Bar</Value>" +
                "       </Eq>" +
                "   </Where>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_expression_with_user_id_call_IS_translated_successfully()
        {
            string caml = Camlex.Query().Where(x => x["foo"] == (DataTypes.Integer)Camlex.UserID.ToString()).ToString();

            string expected =
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"foo\" />" +
                "           <Value Type=\"Integer\"><UserID /></Value>" +
                "       </Eq>" +
                "   </Where>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}
