using System.Linq.Expressions;
using CamlexNET;
using ExpressionToCodeLib;

namespace CamlexOnlineX.Tests
{
    public class Parent
    {
        public class Child
        {
        }
    }

    [TestFixture]
    public class ExpressionToCodeTests
    {
        [Test]
        public void test_THAT_cast_to_nested_class_IS_translated_properly()
        {
            var expr = Expression.TypeAs(Expression.Constant(null), typeof (Parent.Child));
            string code = ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("null as Parent.Child"));
        }

        [Test]
        public void test_THAT_cast_to_parent_class_IS_translated_properly()
        {
            var expr = Expression.TypeAs(Expression.Constant(null), typeof(Parent));
            string code = ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("null as Parent"));
        }

        [Test]
        public void test_THAT_date_time_IS_printed_correctly()
        {
            var dt = new DateTime(2011, 10, 25, 21, 41, 44, 192);
            var expr = Expression.Constant(dt);
            string code = ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("new DateTime(2011, 10, 25, 21, 41, 44, 192)"));
        }

        [Test]
        public void test_THAT_convert_to_basefieldtype_IS_not_printed()
        {
            var expr = Expression.Convert(Expression.Convert(Expression.Constant("foo"), typeof(BaseFieldType)), typeof(DataTypes.DateTime));
            string code = ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("(DataTypes.DateTime)\"foo\""));
        }

        [Test]
        public void test_THAT_array_of_objects_IS_printed_without_object_type()
        {
            var expr = Expression.NewArrayInit(typeof(object), Expression.Constant("foo"));
            string code = ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("new[] { \"foo\" }"));
        }

        private class Foo
        {
            public static explicit operator Foo(string s) { return null; }
        }

        [Test]
        public void test_THAT_string_convert_IS_printed_correctly()
        {
            var expr = Expression.Convert(Expression.Constant("foo"), typeof(Foo));
            string code = ExpressionToCode.ToCode(expr);
            Assert.That(code, Is.EqualTo("(ExpressionToCodeTests.Foo)\"foo\""));
        }
    }
}
