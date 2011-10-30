#region Copyright(c) Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
// -----------------------------------------------------------------------------
// Copyright(c) 2010 Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
//   1. No Trademark License - Microsoft Public License (Ms-PL) does not grant you rights to use
//      authors names, logos, or trademarks.
//   2. If you distribute any portion of the software, you must retain all copyright,
//      patent, trademark, and attribution notices that are present in the software.
//   3. If you distribute any portion of the software in source code form, you may do
//      so only under this license by including a complete copy of Microsoft Public License (Ms-PL)
//      with your distribution. If you distribute any portion of the software in compiled
//      or object code form, you may only do so under a license that complies with
//      Microsoft Public License (Ms-PL).
//   4. The names of the authors may not be used to endorse or promote products
//      derived from this software without specific prior written permission.
//
// The software is licensed "as-is." You bear the risk of using it. The authors
// give no express warranties, guarantees or conditions. You may have additional consumer
// rights under your local laws which this license cannot change. To the extent permitted
// under your local laws, the authors exclude the implied warranties of merchantability,
// fitness for a particular purpose and non-infringement.
// -----------------------------------------------------------------------------
#endregion
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
