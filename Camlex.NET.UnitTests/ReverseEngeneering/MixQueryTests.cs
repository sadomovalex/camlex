using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CamlexNET.UnitTests.Helpers;
using Microsoft.SharePoint;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering
{
    [TestFixture]
    public class MixQueryTests
    {
        [Test]
        public void test_THAT_existing_single_eq_expression_IS_mixed_with_single_expression_correctly_using_and()
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

        [Test]
        public void test_THAT_existing_single_eq_expression_with_query_tag_IS_mixed_with_single_expression_correctly_using_and()
        {
            string existingQuery =
                "<Query>" +
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Title\" />" +
                "           <Value Type=\"Text\">testValue</Value>" +
                "       </Eq>" +
                "   </Where>" +
                "</Query>";

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

            var query = Camlex.Query().WhereAll(existingQuery, x => (string)x["Title"] == "foo").ToString();
            Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_existing_several_expressions_ARE_mixed_with_several_expressions_correctly_using_and()
        {
            string existingQuery =
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

            string expected =
                "<Where>" +
                "  <And>" +
                "    <And>" +
                "      <Gt>" +
                "        <FieldRef Name=\"Count\" />" +
                "        <Value Type=\"Integer\">1</Value>" +
                "      </Gt>" +
                "      <IsNotNull>" +
                "        <FieldRef Name=\"Status\" />" +
                "      </IsNotNull>" +
                "    </And>" +
                "    <And>" +
                "      <Eq>" +
                "        <FieldRef Name=\"Title\" />" +
                "        <Value Type=\"Text\">foo</Value>" +
                "      </Eq>" +
                "      <Eq>" +
                "        <FieldRef Name=\"Title\" />" +
                "        <Value Type=\"Text\">testValue</Value>" +
                "      </Eq>" +
                "    </And>" +
                "  </And>" +
                "</Where>";

            var query = Camlex.Query().WhereAll(existingQuery, x => (int)x["Count"] > 1 && x["Status"] != null).ToString();
            Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_existing_single_eq_expression_IS_mixed_with_single_expression_correctly_using_or()
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
                "  <Or>" +
                "    <Eq>" +
                "      <FieldRef Name=\"Title\" />" +
                "      <Value Type=\"Text\">foo</Value>" +
                "    </Eq>" +
                "    <Eq>" +
                "      <FieldRef Name=\"Title\" />" +
                "      <Value Type=\"Text\">testValue</Value>" +
                "    </Eq>" +
                "  </Or>" +
                "</Where>";

            var query = Camlex.Query().WhereAny(existingQuery, x => (string)x["Title"] == "foo").ToString();
            Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_existing_several_expressions_ARE_mixed_with_several_expressions_correctly_using_or()
        {
            string existingQuery =
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

            string expected =
                "<Where>" +
                "  <Or>" +
                "    <And>" +
                "      <Gt>" +
                "        <FieldRef Name=\"Count\" />" +
                "        <Value Type=\"Integer\">1</Value>" +
                "      </Gt>" +
                "      <IsNotNull>" +
                "        <FieldRef Name=\"Status\" />" +
                "      </IsNotNull>" +
                "    </And>" +
                "    <And>" +
                "      <Eq>" +
                "        <FieldRef Name=\"Title\" />" +
                "        <Value Type=\"Text\">foo</Value>" +
                "      </Eq>" +
                "      <Eq>" +
                "        <FieldRef Name=\"Title\" />" +
                "        <Value Type=\"Text\">testValue</Value>" +
                "      </Eq>" +
                "    </And>" +
                "  </Or>" +
                "</Where>";

            var query = Camlex.Query().WhereAny(existingQuery, x => (int)x["Count"] > 1 && x["Status"] != null).ToString();
            Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        [ExpectedException(typeof(IncorrectCamlException))]
        public void test_WHEN_where_is_not_provided_THEN_exception_is_thrown()
        {
            string existingQuery =
                "<Query>" +
                "  <OrderBy>" +
                "    <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
                "  </OrderBy>" +
                "</Query>";
            var query = Camlex.Query().WhereAll(existingQuery, x => (string) x["Title"] == "foo").ToString();
        }

        [Test]
        public void test_THAT_single_order_by_IS_mixed_with_single_order_by_sucessfully()
        {
            string existingQuery =
                "  <OrderBy>" +
                "    <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
                "  </OrderBy>";

            string expected =
                "<OrderBy>" +
                "  <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
                "  <FieldRef Name=\"Title\" />" +
                "</OrderBy>";

            var query = Camlex.Query().OrderBy(existingQuery, x => x["Title"]).ToString();
            Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_single_order_by_IS_mixed_with_several_order_by_sucessfully()
        {
            string existingQuery =
                "  <OrderBy>" +
                "    <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
                "  </OrderBy>";

            string expected =
                "<OrderBy>" +
                "  <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
                "  <FieldRef Name=\"Title\" />" +
                "  <FieldRef Name=\"State\" Ascending=\"True\" />" +
                "</OrderBy>";

            var query = Camlex.Query().OrderBy(existingQuery, x => new[]{x["Title"], x["State"] as Camlex.Asc}).ToString();
            Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_several_order_by_IS_mixed_with_several_order_by_sucessfully()
        {
            string existingQuery =
                "  <OrderBy>" +
                "    <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
                "    <FieldRef Name=\"ModifiedBy\" />" +
                "  </OrderBy>";

            string expected =
                "<OrderBy>" +
                "  <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
                "  <FieldRef Name=\"ModifiedBy\" />" +
                "  <FieldRef Name=\"Title\" />" +
                "  <FieldRef Name=\"State\" Ascending=\"True\" />" +
                "</OrderBy>";

            var query = Camlex.Query().OrderBy(existingQuery, x => new[] { x["Title"], x["State"] as Camlex.Asc }).ToString();
            Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_order_by_collection_IS_mixed_with_several_order_by_sucessfully()
        {
            string existingQuery =
                "  <OrderBy>" +
                "    <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
                "    <FieldRef Name=\"ModifiedBy\" />" +
                "  </OrderBy>";

            string expected =
                "<OrderBy>" +
                "  <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
                "  <FieldRef Name=\"ModifiedBy\" />" +
                "  <FieldRef Name=\"Title\" />" +
                "  <FieldRef Name=\"State\" Ascending=\"True\" />" +
                "</OrderBy>";

            var exprs = new List<Expression<Func<SPListItem, object>>>();
            exprs.Add(x => x["Title"]);
            exprs.Add(x => x["State"] as Camlex.Asc);

            var query = Camlex.Query().OrderBy(existingQuery, exprs).ToString();
            Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        [ExpectedException(typeof(IncorrectCamlException))]
        public void test_WHEN_order_by_is_not_provided_THEN_exception_is_thrown()
        {
            string existingQuery =
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Title\" />" +
                "           <Value Type=\"Text\">testValue</Value>" +
                "       </Eq>" +
                "   </Where>";
            var query = Camlex.Query().OrderBy(existingQuery, x => x["Title"]).ToString();
        }

        [Test]
        public void test_THAT_single_group_by_IS_mixed_with_single_group_by_sucessfully()
        {
            string existingQuery =
                "  <GroupBy>" +
                "    <FieldRef Name=\"Modified\" />" +
                "  </GroupBy>";

            string expected =
                "<GroupBy>" +
                "  <FieldRef Name=\"Modified\" />" +
                "  <FieldRef Name=\"Title\" />" +
                "</GroupBy>";

            var query = Camlex.Query().GroupBy(existingQuery, x => x["Title"]).ToString();
            Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_single_group_by_IS_mixed_with_several_group_by_sucessfully()
        {
            string existingQuery =
                "  <GroupBy>" +
                "    <FieldRef Name=\"Modified\" />" +
                "  </GroupBy>";

            string expected =
                "<GroupBy>" +
                "  <FieldRef Name=\"Modified\" />" +
                "  <FieldRef Name=\"Title\" />" +
                "  <FieldRef Name=\"State\" />" +
                "</GroupBy>";

            var query = Camlex.Query().GroupBy(existingQuery, x => new[] { x["Title"], x["State"] }).ToString();
            Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_several_group_by_IS_mixed_with_several_group_by_sucessfully()
        {
            string existingQuery =
                "  <GroupBy>" +
                "    <FieldRef Name=\"Modified\" />" +
                "    <FieldRef Name=\"ModifiedBy\" />" +
                "  </GroupBy>";

            string expected =
                "<GroupBy>" +
                "  <FieldRef Name=\"Modified\" />" +
                "  <FieldRef Name=\"ModifiedBy\" />" +
                "  <FieldRef Name=\"Title\" />" +
                "  <FieldRef Name=\"State\" />" +
                "</GroupBy>";

            var query = Camlex.Query().GroupBy(existingQuery, x => new[] { x["Title"], x["State"] }).ToString();
            Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_existing_group_by_HAS_more_priority()
        {
            string existingQuery =
                "  <GroupBy Collapse=\"False\" GroupLimit=\"20\">" +
                "    <FieldRef Name=\"Modified\" />" +
                "    <FieldRef Name=\"ModifiedBy\" />" +
                "  </GroupBy>";

            string expected =
                "<GroupBy Collapse=\"False\" GroupLimit=\"20\">" +
                "  <FieldRef Name=\"Modified\" />" +
                "  <FieldRef Name=\"ModifiedBy\" />" +
                "  <FieldRef Name=\"Title\" />" +
                "  <FieldRef Name=\"State\" />" +
                "</GroupBy>";

            var query = Camlex.Query().GroupBy(existingQuery, x => new[] { x["Title"], x["State"] }).ToString();
            Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
        }

//        [Test]
//        public void test_THAT_group_by_collection_IS_mixed_with_several_group_by_sucessfully()
//        {
//            string existingQuery =
//                "  <GroupBy>" +
//                "    <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
//                "    <FieldRef Name=\"ModifiedBy\" />" +
//                "  </GroupBy>";
//
//            string expected =
//                "<GroupBy>" +
//                "  <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
//                "  <FieldRef Name=\"ModifiedBy\" />" +
//                "  <FieldRef Name=\"Title\" />" +
//                "  <FieldRef Name=\"State\" Ascending=\"True\" />" +
//                "</GroupBy>";
//
//            var exprs = new List<Expression<Func<SPListItem, object>>>();
//            exprs.Add(x => x["Title"]);
//            exprs.Add(x => x["State"] as Camlex.Asc);
//
//            var query = Camlex.Query().GroupBy(existingQuery, exprs).ToString();
//            Assert.That(query, Is.EqualTo(expected).Using(new CamlComparer()));
//        }

        [Test]
        [ExpectedException(typeof(IncorrectCamlException))]
        public void test_WHEN_group_by_is_not_provided_THEN_exception_is_thrown()
        {
            string existingQuery =
                "   <Where>" +
                "       <Eq>" +
                "           <FieldRef Name=\"Title\" />" +
                "           <Value Type=\"Text\">testValue</Value>" +
                "       </Eq>" +
                "   </Where>";
            var query = Camlex.Query().GroupBy(existingQuery, x => x["Title"]).ToString();
        }
    }
}
