using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CamlexNET.UnitTests.Helpers;
using Microsoft.SharePoint;
using NUnit.Framework;

namespace CamlexNET.UnitTests
{
    [TestFixture]
    public class CamlexOrderByTests
    {
        [Test]
        public void test_THAT_single_orderby_expression_IS_translated_sucessfully()
        {
            var caml = Camlex.Query().OrderBy(x => x["field1"] as Camlex.Desc).ToString();

            var expected =
                "  <OrderBy>" +
                "    <FieldRef Name=\"field1\" Ascending=\"False\" />" +
                "  </OrderBy>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_multiple_orderby_expression_IS_translated_sucessfully()
        {
            var caml = Camlex.Query().OrderBy(
                x => new[] { x["field1"], x["field2"] as Camlex.Desc, x["field3"] as Camlex.Asc }).ToString();

            var expected =
                "  <OrderBy>" +
                "    <FieldRef Name=\"field1\" />" +
                "    <FieldRef Name=\"field2\" Ascending=\"False\" />" +
                "    <FieldRef Name=\"field3\" Ascending=\"True\" />" +
                "  </OrderBy>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_orderby_expression_with_non_constant_parameters_IS_translated_sucessfully()
        {
            bool b = true;
            var caml = Camlex.Query().OrderBy(x => new[] { x[b ? SPBuiltInFieldId.Title : SPBuiltInFieldId.UniqueId], x[SPBuiltInFieldId.Modified] as Camlex.Asc }).ToString();

            var expected =
            "<OrderBy>" +
            "  <FieldRef ID=\"fa564e0f-0c70-4ab9-b863-0177e6ddd247\" />" +
            "  <FieldRef ID=\"28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f\" Ascending=\"True\" />" +
            "</OrderBy>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_2_order_by_ARE_joined_properly()
        {
            var orderByList = new List<Expression<Func<SPListItem, object>>>();
            orderByList.Add(x => x["Title"] as Camlex.Asc);
            orderByList.Add(x => x["Date"] as Camlex.Desc);

            var caml = Camlex.Query().OrderBy(orderByList).ToString();

            var expected =
                "  <OrderBy>" +
                "    <FieldRef Name=\"Title\" Ascending=\"True\" />" +
                "    <FieldRef Name=\"Date\" Ascending=\"False\" />" +
                "  </OrderBy>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_2_order_by_with_differenet_params_ARE_joined_properly()
        {
            var orderByList = new List<Expression<Func<SPListItem, object>>>();
            orderByList.Add(x => x["Title"] as Camlex.Asc);
            orderByList.Add(y => y["Date"] as Camlex.Desc);

            var caml = Camlex.Query().OrderBy(orderByList).ToString();

            var expected =
                "  <OrderBy>" +
                "    <FieldRef Name=\"Title\" Ascending=\"True\" />" +
                "    <FieldRef Name=\"Date\" Ascending=\"False\" />" +
                "  </OrderBy>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        [ExpectedException(typeof(EmptyExpressionsListException))]
        public void test_WHEN_order_by_list_is_empty_THEN_exception_is_thrown()
        {
            var orderByList = new List<Expression<Func<SPListItem, object>>>();
            var caml = Camlex.Query().OrderBy(orderByList).ToString();
        }

        [Test]
        public void test_THAT_3_order_by_with_non_constant_fields_ARE_joined_properly()
        {
            var orderByList = new List<Expression<Func<SPListItem, object>>>();
            string name = "Title";
            orderByList.Add(x => x[name]);

            Func<string> f = () => "Date";
            orderByList.Add(x => x[f()]);

            var sb = new StringBuilder();
            for (int i = 0; i < 5; i++)
            {
                sb.Append("s");
            }
            orderByList.Add(x => x[sb.ToString()]);

            var caml = Camlex.Query().OrderBy(orderByList).ToString();

            var expected =
                "  <OrderBy>" +
                "    <FieldRef Name=\"Title\" />" +
                "    <FieldRef Name=\"Date\" />" +
                "    <FieldRef Name=\"sssss\" />" +
                "  </OrderBy>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}
