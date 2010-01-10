using System;
using System.Linq.Expressions;
using Camlex.NET.Impl.Operands;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;
using NUnit.Framework;

namespace Camlex.NET.UnitTests.Factories
{
    [TestFixture]
    public class OperandBuilderTests
    {
        [Test]
        public void test_THAT_field_ref_operand_IS_created_successfully()
        {
            var operandBuilder = new OperandBuilder();
            Expression<Func<SPItem, bool>> expr = x => (string) x["Email"] == "test@example.com";
            var operand = operandBuilder.CreateFieldRefOperand(((BinaryExpression)expr.Body).Left);
            Assert.That(operand, Is.InstanceOf<FieldRefOperand>());
            Assert.That(((FieldRefOperand)operand).FieldName, Is.EqualTo("Email"));
        }

        [Test]
        public void test_WHEN_native_value_is_text_THEN_text_operand_is_created()
        {
            var operandBuilder = new OperandBuilder();
            Expression<Func<SPItem, bool>> expr = x => (string)x["Email"] == "test@example.com";
            var operand = operandBuilder.CreateValueOperandForNativeSyntax(((BinaryExpression)expr.Body).Right);
            
            Assert.That(operand, Is.InstanceOf<TextValueOperand>());

            var valueOperand = operand as TextValueOperand;
            Assert.That(valueOperand.Type, Is.EqualTo(typeof(DataTypes.Text)));
            Assert.That(valueOperand.Value, Is.EqualTo("test@example.com"));
        }

        [Test]
        public void test_WHEN_string_based_value_is_text_THEN_text_operand_is_created()
        {
            var operandBuilder = new OperandBuilder();
            Expression<Func<SPItem, bool>> expr = x => x["Email"] == (DataTypes.Text)"test@example.com";
            var operand = operandBuilder.CreateValueOperandForStringBasedSyntax(((BinaryExpression)expr.Body).Right);

            Assert.That(operand, Is.InstanceOf<TextValueOperand>());

            var valueOperand = operand as TextValueOperand;
            Assert.That(valueOperand.Type, Is.EqualTo(typeof(DataTypes.Text)));
            Assert.That(valueOperand.Value, Is.EqualTo("test@example.com"));
        }

        [Test]
        public void test_WHEN_value_is_note_THEN_note_operand_is_created()
        {
            var operandBuilder = new OperandBuilder();
            Expression<Func<SPItem, bool>> expr = x => ((DataTypes.Note)x["Count"]).StartsWith("foo");
            var operand = operandBuilder.CreateValueOperandForNativeSyntax(((MethodCallExpression)expr.Body).Arguments[0], typeof(DataTypes.Note));

            Assert.That(operand, Is.InstanceOf<NoteValueOperand>());

            var valueOperand = operand as NoteValueOperand;
            Assert.That(valueOperand.Type, Is.EqualTo(typeof(DataTypes.Note)));
            Assert.That(valueOperand.Value, Is.EqualTo("foo"));
        }

        [Test]
        public void test_WHEN_native_value_is_integer_THEN_integer_operand_is_created()
        {
            var operandBuilder = new OperandBuilder();
            Expression<Func<SPItem, bool>> expr = x => (int)x["Foo"] == 1;
            var operand = operandBuilder.CreateValueOperandForNativeSyntax(((BinaryExpression)expr.Body).Right);

            Assert.That(operand, Is.InstanceOf<IntegerValueOperand>());

            var valueOperand = operand as IntegerValueOperand;
            Assert.That(valueOperand.Type, Is.EqualTo(typeof(DataTypes.Integer)));
            Assert.That(valueOperand.Value, Is.EqualTo(1));
        }

        [Test]
        public void test_WHEN_string_based_value_is_integer_THEN_integer_operand_is_created()
        {
            var operandBuilder = new OperandBuilder();
            Expression<Func<SPItem, bool>> expr = x => x["Foo"] == (DataTypes.Integer)"1";
            var operand = operandBuilder.CreateValueOperandForStringBasedSyntax(((BinaryExpression)expr.Body).Right);

            Assert.That(operand, Is.InstanceOf<IntegerValueOperand>());

            var valueOperand = operand as IntegerValueOperand;
            Assert.That(valueOperand.Type, Is.EqualTo(typeof(DataTypes.Integer)));
            Assert.That(valueOperand.Value, Is.EqualTo(1));
        }

        [Test]
        public void test_WHEN_native_value_is_boolean_THEN_boolean_operand_is_created()
        {
            var operandBuilder = new OperandBuilder();
            Expression<Func<SPItem, bool>> expr = x => (bool)x["Foo"] == true;
            var operand = operandBuilder.CreateValueOperandForNativeSyntax(((BinaryExpression)expr.Body).Right);

            Assert.That(operand, Is.InstanceOf<BooleanValueOperand>());

            var valueOperand = operand as BooleanValueOperand;
            Assert.That(valueOperand.Type, Is.EqualTo(typeof(DataTypes.Boolean)));
            Assert.That(valueOperand.Value, Is.True);
        }

        [Test]
        public void test_WHEN_string_based_value_is_boolean_THEN_boolean_operand_is_created()
        {
            var operandBuilder = new OperandBuilder();
            Expression<Func<SPItem, bool>> expr = x => x["Foo"] == (DataTypes.Boolean)"false";
            var operand = operandBuilder.CreateValueOperandForStringBasedSyntax(((BinaryExpression)expr.Body).Right);

            Assert.That(operand, Is.InstanceOf<BooleanValueOperand>());

            var valueOperand = operand as BooleanValueOperand;
            Assert.That(valueOperand.Type, Is.EqualTo(typeof(DataTypes.Boolean)));
            Assert.That(valueOperand.Value, Is.False);
        }

        [Test]
        public void test_WHEN_rvalue_is_null_THEN_nullvalue_operand_is_created()
        {
            var operandBuilder = new OperandBuilder();
            Expression<Func<SPItem, bool>> expr = x => x["Foo"] == null;
            var operand = operandBuilder.CreateValueOperandForNativeSyntax(((BinaryExpression)expr.Body).Right);

            Assert.That(operand, Is.InstanceOf<NullValueOperand>());
        }

        [Test]
        public void test_WHEN_rvalue_is_null_and_lvalue_is_casted_to_string_THEN_nullvalue_operand_is_created()
        {
            var operandBuilder = new OperandBuilder();
            Expression<Func<SPItem, bool>> expr = x => (string)x["Foo"] == null;
            var operand = operandBuilder.CreateValueOperandForNativeSyntax(((BinaryExpression)expr.Body).Right);

            Assert.That(operand, Is.InstanceOf<NullValueOperand>());
        }

        [Test]
        public void test_WHEN_rvalue_is_null_casted_to_string_based_THEN_nullvalue_operand_is_created()
        {
            var operandBuilder = new OperandBuilder();
            Expression<Func<SPItem, bool>> expr = x => x["Foo"] == (DataTypes.Text)null;
            var operand = operandBuilder.CreateValueOperandForNativeSyntax(((BinaryExpression)expr.Body).Right);

            Assert.That(operand, Is.InstanceOf<NullValueOperand>());
        }

//        [Test]
//        public void test_WHEN_rvalue_is_null_casted_to_string_based_and_lvalue_is_casted_to_string_based_THEN_nullvalue_operand_is_created()
//        {
//            var operandBuilder = new OperandBuilder();
//            Expression<Func<SPItem, bool>> expr = x => (DataTypes.Text)x["Foo"] == (DataTypes.Text)null;
//            var operand = operandBuilder.CreateValueOperandForNativeSyntax(((BinaryExpression)expr.Body).Right);
//
//            Assert.That(operand, Is.InstanceOf<NullValueOperand>());
//        }

        [Test]
        public void test_WHEN_variable_is_null_THEN_nullvalue_operand_is_created()
        {
            object o = null;
            Expression<Func<SPItem, bool>> expr = x => x["Foo"] == o;

            var operandBuilder = new OperandBuilder();
            var operand = operandBuilder.CreateValueOperandForNativeSyntax(((BinaryExpression)expr.Body).Right);

            Assert.That(operand, Is.InstanceOf<NullValueOperand>());
        }

        [Test]
        public void test_WHEN_variable_is_null_casted_to_string_based_THEN_nullvalue_operand_is_created()
        {
            object o = null;
            Expression<Func<SPItem, bool>> expr = x => x["Foo"] == (DataTypes.Integer)o;

            var operandBuilder = new OperandBuilder();
            var operand = operandBuilder.CreateValueOperandForNativeSyntax(((BinaryExpression)expr.Body).Right);

            Assert.That(operand, Is.InstanceOf<NullValueOperand>());
        }

        [Test]
        public void test_WHEN_method_call_result_is_null_THEN_nullvalue_operand_is_created()
        {
            Expression<Func<SPItem, bool>> expr = x => x["Foo"] == val1();

            var operandBuilder = new OperandBuilder();
            var operand = operandBuilder.CreateValueOperandForNativeSyntax(((BinaryExpression)expr.Body).Right);

            Assert.That(operand, Is.InstanceOf<NullValueOperand>());
        }

        [Test]
        public void test_WHEN_method_call_result_is_null_and_casted_to_string_based_THEN_nullvalue_operand_is_created()
        {
            Expression<Func<SPItem, bool>> expr = x => x["Foo"] == (DataTypes.Integer)val1();

            var operandBuilder = new OperandBuilder();
            var operand = operandBuilder.CreateValueOperandForNativeSyntax(((BinaryExpression)expr.Body).Right);

            Assert.That(operand, Is.InstanceOf<NullValueOperand>());
        }

        object val1()
        {
            return null;
        }

        [Test]
        public void test_WHEN_nested_method_call_result_is_null_THEN_nullvalue_operand_is_created()
        {
            Func<object> f = () => null;
            Expression<Func<SPItem, bool>> expr = x => x["Foo"] == f();

            var operandBuilder = new OperandBuilder();
            var operand = operandBuilder.CreateValueOperandForNativeSyntax(((BinaryExpression)expr.Body).Right);

            Assert.That(operand, Is.InstanceOf<NullValueOperand>());
        }

        [Test]
        public void test_WHEN_nested_method_call_result_is_null_and_casted_to_string_based_THEN_nullvalue_operand_is_created()
        {
            Func<object> f = () => null;
            Expression<Func<SPItem, bool>> expr = x => x["Foo"] == (DataTypes.Text)f();

            var operandBuilder = new OperandBuilder();
            var operand = operandBuilder.CreateValueOperandForNativeSyntax(((BinaryExpression)expr.Body).Right);

            Assert.That(operand, Is.InstanceOf<NullValueOperand>());
        }

        [Test]
        public void test_WHEN_value_is_string_based_and_has_no_native_representation_THEN_generic_string_based_operand_is_created()
        {
            var operandBuilder = new OperandBuilder();
            Expression<Func<SPItem, bool>> expr = x => x["User"] == (DataTypes.User)"John Smith";
            var operand = operandBuilder.CreateValueOperandForStringBasedSyntax(((BinaryExpression)expr.Body).Right);

            Assert.That(operand, Is.InstanceOf<GenericStringBasedValueOperand>());

            var valueOperand = operand as GenericStringBasedValueOperand;
            Assert.That(valueOperand.Type, Is.EqualTo(typeof(DataTypes.User)));
            Assert.That(valueOperand.Value, Is.EqualTo("John Smith"));
        }
    }
}


