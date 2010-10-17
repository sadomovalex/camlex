using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET.UnitTests.Helpers;
using Microsoft.SharePoint;
using NUnit.Framework;

namespace CamlexNET.UnitTests
{
    [TestFixture]
    public class CamlexViewFieldsTests
    {
        [Test]
        public void test_THAT_viewfields_with_single_field_title_IS_translated_sucessfully()
        {
            string caml =
                Camlex.Query().ViewFields(x => x["Title"]);

            string expected = "<FieldRef Name=\"Title\" />";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_viewfields_with_single_field_title_with_parent_tag_IS_translated_sucessfully()
        {
            string caml =
                Camlex.Query().ViewFields(x => x["Title"], true);

            string expected =
                @"<ViewFields>" +
                    "<FieldRef Name=\"Title\" />" +
                "</ViewFields>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_viewfields_with_several_fields_IS_translated_sucessfully()
        {
            string caml =
                Camlex.Query().ViewFields(x => new [] { x["Title"], x["Status"] });

            string expected = "<FieldRef Name=\"Title\" /><FieldRef Name=\"Status\" />";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_viewfields_with_several_fields_ids_with_parent_tag_IS_translated_sucessfully()
        {
            string caml =
                Camlex.Query().ViewFields(x => new[] { x[SPBuiltInFieldId.Title], x[SPBuiltInFieldId.Modified] }, true);

            string expected =
                @"<ViewFields>" +
                    "<FieldRef ID=\"fa564e0f-0c70-4ab9-b863-0177e6ddd247\" />" +
                    "<FieldRef ID=\"28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f\" />" +
                "</ViewFields>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_viewfields_with_several_fields_ids_IS_translated_sucessfully()
        {
            string caml =
                Camlex.Query().ViewFields(x => new[] { x[SPBuiltInFieldId.Title], x[SPBuiltInFieldId.Modified] });

            string expected = "<FieldRef ID=\"fa564e0f-0c70-4ab9-b863-0177e6ddd247\" /><FieldRef ID=\"28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f\" />";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_viewfield_non_const_title_IS_translated_sucessfully()
        {
            Func<string> f = () => "Title";

            string caml =
                Camlex.Query().ViewFields(x => x[f()]);

            string expected = "<FieldRef Name=\"Title\" />";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}
