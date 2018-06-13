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

using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering
{
    [TestFixture]
    public class ReJoinTests
    {
        [Test]
        public void test_THAT_left_join_IS_translated_sucessfully()
        {
            string xml =
                "<View>" +
                  "<Joins>" +
                    "<Join Type=\"LEFT\" ListAlias=\"Customers\">" +
                      "<Eq>" +
                        "<FieldRef Name=\"CustomerName\" RefType=\"Id\" />" +
                        "<FieldRef List=\"Customers\" Name=\"Id\" />" +
                      "</Eq>" +
                    "</Join>" +
                  "</Joins>" +
                "</View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().LeftJoin(x => x.get_Item(\"CustomerName\").ForeignList(\"Customers\"))"));
        }

        [Test]
        public void test_THAT_inner_join_IS_translated_sucessfully()
        {
            string xml =
                "<View>" +
                  "<Joins>" +
                    "<Join Type=\"INNER\" ListAlias=\"Customers\">" +
                      "<Eq>" +
                        "<FieldRef Name=\"CustomerName\" RefType=\"Id\" />" +
                        "<FieldRef List=\"Customers\" Name=\"Id\" />" +
                      "</Eq>" +
                    "</Join>" +
                  "</Joins>" +
                "</View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().InnerJoin(x => x.get_Item(\"CustomerName\").ForeignList(\"Customers\"))"));
        }

        [Test]
        public void test_THAT_projected_fields_ARE_translated_sucessfully()
        {
            string xml =
                "<View>" +
                  "<ProjectedFields>" +
                    "<Field Name=\"test1\" Type=\"Lookup\" List=\"foo1\" ShowField=\"bar1\" />" +
                    "<Field Name=\"test2\" Type=\"Lookup\" List=\"foo2\" ShowField=\"bar2\" />" +
                  "</ProjectedFields>" +
                "</View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().ProjectedField(x => x.get_Item(\"test1\").List(\"foo1\").ShowField(\"bar1\")).ProjectedField(x => x.get_Item(\"test2\").List(\"foo2\").ShowField(\"bar2\"))"));
        }
    }
}
