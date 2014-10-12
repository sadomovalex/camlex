using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET.UnitTests.Helpers;
using NUnit.Framework;

namespace CamlexNET.UnitTests
{
    [TestFixture]
    public class CamlexListJoinTests
    {
        [Test]
        public void test_THAT_single_left_join_IS_translated_properly()
        {
            string caml = Camlex.Query().Joins().Left(x => x["test"].ForeignList("foo")).ToString();
            string expected =
               "<Join Type=\"LEFT\" ListAlias=\"foo\">" +
               "    <Eq>" +
               "      <FieldRef Name=\"test\" RefType=\"Id\" />" +
               "      <FieldRef List=\"foo\" Name=\"Id\" />" +
               "    </Eq>" +
               "</Join>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_single_left_join_with_primary_list_IS_translated_properly()
        {
            string caml = Camlex.Query().Joins().Left(x => x["test"].PrimaryList("foo").ForeignList("bar")).ToString();
            string expected =
               "<Join Type=\"LEFT\" ListAlias=\"bar\">" +
               "    <Eq>" +
               "      <FieldRef List=\"foo\" Name=\"test\" RefType=\"Id\" />" +
               "      <FieldRef List=\"bar\" Name=\"Id\" />" +
               "    </Eq>" +
               "</Join>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_multiple_left_joins_ARE_translated_properly()
        {
            string caml = Camlex.Query().Joins().Left(x => x["test1"].ForeignList("foo1")).Left(x => x["test2"].PrimaryList("foo2").ForeignList("bar2")).ToString();
            string expected =
               "<Join Type=\"LEFT\" ListAlias=\"foo1\">" +
               "    <Eq>" +
               "      <FieldRef Name=\"test1\" RefType=\"Id\" />" +
               "      <FieldRef List=\"foo1\" Name=\"Id\" />" +
               "    </Eq>" +
               "</Join>" +
               "<Join Type=\"LEFT\" ListAlias=\"bar2\">" +
               "    <Eq>" +
               "      <FieldRef List=\"foo2\" Name=\"test2\" RefType=\"Id\" />" +
               "      <FieldRef List=\"bar2\" Name=\"Id\" />" +
               "    </Eq>" +
               "</Join>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_multiple_joins_ARE_translated_properly()
        {
            string caml = Camlex.Query().Joins().Left(x => x["test1"].ForeignList("foo1"))
                .Left(x => x["test2"].PrimaryList("foo2").ForeignList("bar2"))
                .Inner(x => x["test3"].PrimaryList("foo3").ForeignList("bar3")).ToString();
            string expected =
                "<Join Type=\"LEFT\" ListAlias=\"foo1\">" +
                "    <Eq>" +
                "      <FieldRef Name=\"test1\" RefType=\"Id\" />" +
                "      <FieldRef List=\"foo1\" Name=\"Id\" />" +
                "    </Eq>" +
                "</Join>" +
                "<Join Type=\"LEFT\" ListAlias=\"bar2\">" +
                "    <Eq>" +
                "      <FieldRef List=\"foo2\" Name=\"test2\" RefType=\"Id\" />" +
                "      <FieldRef List=\"bar2\" Name=\"Id\" />" +
                "    </Eq>" +
                "</Join>" +
                "<Join Type=\"INNER\" ListAlias=\"bar3\">" +
                "    <Eq>" +
                "      <FieldRef List=\"foo3\" Name=\"test3\" RefType=\"Id\" />" +
                "      <FieldRef List=\"bar3\" Name=\"Id\" />" +
                "    </Eq>" +
                "</Join>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_single_inner_join_IS_translated_properly()
        {
            string caml = Camlex.Query().Joins().Inner(x => x["test"].ForeignList("foo")).ToString();
            string expected =
               "<Join Type=\"INNER\" ListAlias=\"foo\">" +
               "    <Eq>" +
               "      <FieldRef Name=\"test\" RefType=\"Id\" />" +
               "      <FieldRef List=\"foo\" Name=\"Id\" />" +
               "    </Eq>" +
               "</Join>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_single_inner_join_with_primary_list_IS_translated_properly()
        {
            string caml = Camlex.Query().Joins().Inner(x => x["test"].PrimaryList("foo").ForeignList("bar")).ToString();
            string expected =
               "<Join Type=\"INNER\" ListAlias=\"bar\">" +
               "    <Eq>" +
               "      <FieldRef List=\"foo\" Name=\"test\" RefType=\"Id\" />" +
               "      <FieldRef List=\"bar\" Name=\"Id\" />" +
               "    </Eq>" +
               "</Join>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_joins_with_non_constants_parameters_ARE_translated_properly()
        {
            Func<string, string> f = s => { return "test" + s; };

            string caml = Camlex.Query().Joins().Left(x => x[f("1")].ForeignList(f("1"))).Inner(x => x[f("2")].PrimaryList(f("2")).ForeignList(f("2"))).ToString();
            string expected =
               "<Join Type=\"LEFT\" ListAlias=\"test1\">" +
               "    <Eq>" +
               "      <FieldRef Name=\"test1\" RefType=\"Id\" />" +
               "      <FieldRef List=\"test1\" Name=\"Id\" />" +
               "    </Eq>" +
               "</Join>" +
               "<Join Type=\"INNER\" ListAlias=\"test2\">" +
               "    <Eq>" +
               "      <FieldRef List=\"test2\" Name=\"test2\" RefType=\"Id\" />" +
               "      <FieldRef List=\"test2\" Name=\"Id\" />" +
               "    </Eq>" +
               "</Join>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_single_projected_field_IS_translated_properly()
        {
            string caml = Camlex.Query().ProjectedFields(x => x["test"].List("foo").ShowField("Title"));
            string expected = "<Field Name=\"test\" Type=\"Lookup\" List=\"foo\" ShowField=\"Title\" />";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}
