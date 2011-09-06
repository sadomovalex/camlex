using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CamlexNET.Impl.ReverseEngeneering.Caml;
using Microsoft.SharePoint;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering
{
    [TestFixture]
    public class ReLinkerFromCamlTests
    {
        [Test]
        public void test_WHEN_where_is_specified_THEN_expressions_are_linked_correctly()
        {
            var l = new ReLinkerFromCaml(null, null, null, null);
            var expr = l.Link((Expression<Func<SPListItem, bool>>)(x => (int) x["foo"] == 1), null, null, null);
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => (Convert(x.get_Item(\"foo\")) = 1))"));
        }
    }
}
