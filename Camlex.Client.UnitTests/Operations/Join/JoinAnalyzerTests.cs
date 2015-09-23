using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CamlexNET.Impl.Factories;
using CamlexNET.Impl.Operations.Join;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using NUnit.Framework;

namespace CamlexNET.UnitTests.Operations.Join
{
    [TestFixture]
    public class JoinAnalyzerTests
    {
        [Test]
        public void test_THAT_join_with_foreign_list_IS_valid()
        {
            var a = new JoinAnalyzer(null, null);
            Expression<Func<ListItem, object>> expr = x => x["test"].ForeignList("foo");
            Assert.IsTrue(a.IsValid(expr));
        }

        [Test]
        public void test_THAT_join_with_foreign_and_primary_lists_IS_valid()
        {
            var a = new JoinAnalyzer(null, null);
            Expression<Func<ListItem, object>> expr = x => x["test"].PrimaryList("foo").ForeignList("bar");
            Assert.IsTrue(a.IsValid(expr));
        }

        [Test]
        public void test_THAT_join_with_foreign_and_primary_lists_and_non_constants_params_IS_valid()
        {
            Func<string> f = () => "foo";
            var a = new JoinAnalyzer(null, null);
            Expression<Func<ListItem, object>> expr = x => x[f()].PrimaryList(f()).ForeignList(f());
            Assert.IsTrue(a.IsValid(expr));
        }
    }
}
