#region Copyright(c) Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
// -----------------------------------------------------------------------------
// Copyright(c) 2007 Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
//   1.  No Trademark License - Microsoft Public License (Ms-PL) does not grant you rights to use
//       authors names, logos, or trademarks.
//   2.  If you distribute any portion of the software, you must retain all copyright,
//       patent, trademark, and attribution notices that are present in the software.
//   3.  If you distribute any portion of the software in source code form, you may do
//       so only under this license by including a complete copy of Microsoft Public License (Ms-PL)
//       with your distribution. If you distribute any portion of the software in compiled
//       or object code form, you may only do so under a license that complies with
//       Microsoft Public License (Ms-PL).
//
// The software is licensed "as-is." You bear the risk of using it. The authors
// give no express warranties, guarantees or conditions. You may have additional consumer
// rights under your local laws which this license cannot change. To the extent permitted
// under your local laws, the authors exclude the implied warranties of merchantability,
// fitness for a particular purpose and non-infringement.
// -----------------------------------------------------------------------------
#endregion
using System;
using System.Linq.Expressions;
using CamlexNET.Impl.Factories;
using CamlexNET.Impl.Operands;
using CamlexNET.Interfaces;
using Microsoft.SharePoint;
using NUnit.Framework;

namespace CamlexNET.UnitTests.Factories
{
    [TestFixture]
    public class OperandBuilderTests
    {
        [Test]
        public void test_THAT_field_ref_operand_from_indexer_with_constant_param_IS_created_successfully()
        {
            Expression<Func<SPItem, bool>> expr = x => (string) x["Email"] == "test@example.com";

            var operandBuilder = new OperandBuilder();
            var operand = operandBuilder.CreateFieldRefOperand(((BinaryExpression)expr.Body).Left);

            Assert.That(operand, Is.InstanceOf<FieldRefOperand>());
            Assert.That(((FieldRefOperand)operand).FieldName, Is.EqualTo("Email"));
        }

        [Test]
        public void test_THAT_field_ref_operand_from_indexer_with_variable_param_IS_created_successfully()
        {
            string val = "Email";
            Expression<Func<SPItem, bool>> expr = x => (string)x[val] == "test@example.com";

            var operandBuilder = new OperandBuilder();
            var operand = operandBuilder.CreateFieldRefOperand(((BinaryExpression)expr.Body).Left);

            Assert.That(operand, Is.InstanceOf<FieldRefOperand>());
            Assert.That(((FieldRefOperand)operand).FieldName, Is.EqualTo("Email"));
        }

        [Test]
        public void test_THAT_field_ref_operand_from_indexer_with_ternary_operator_IS_created_successfully()
        {
            bool b = true;
            Expression<Func<SPItem, bool>> expr = x => (string)x[b ? "val1" : "val2"] == "test@example.com";

            var operandBuilder = new OperandBuilder();
            var operand = operandBuilder.CreateFieldRefOperand(((BinaryExpression)expr.Body).Left);

            Assert.That(operand, Is.InstanceOf<FieldRefOperand>());
            Assert.That(((FieldRefOperand)operand).FieldName, Is.EqualTo("val1"));
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
        public void test_WHEN_native_value_is_datetime_THEN_datetime_operand_is_created()
        {
            var operandBuilder = new OperandBuilder();
            Expression<Func<SPItem, bool>> expr = x => (DateTime)x["Foo"] == new DateTime(2010, 1, 2, 3, 4, 5);
            var operand = operandBuilder.CreateValueOperandForNativeSyntax(((BinaryExpression)expr.Body).Right);

            Assert.That(operand, Is.InstanceOf<DateTimeValueOperand>());

            var valueOperand = operand as DateTimeValueOperand;
            Assert.That(valueOperand.Type, Is.EqualTo(typeof(DataTypes.DateTime)));
            Assert.That(valueOperand.Value, Is.EqualTo(new DateTime(2010, 1, 2, 3, 4, 5)));
            Assert.That(valueOperand.IncludeTimeValue, Is.False);
        }

        [Test]
        public void test_WHEN_native_value_is_datetime_with_includetimevalue_THEN_datetime_operand_is_created()
        {
            var operandBuilder = new OperandBuilder();
            Expression<Func<SPItem, bool>> expr = x => (DateTime)x["Foo"] == new DateTime(2010, 1, 2, 3, 4, 5).IncludeTimeValue();
            var operand = operandBuilder.CreateValueOperandForNativeSyntax(((BinaryExpression)expr.Body).Right);

            Assert.That(operand, Is.InstanceOf<DateTimeValueOperand>());

            var valueOperand = operand as DateTimeValueOperand;
            Assert.That(valueOperand.Type, Is.EqualTo(typeof(DataTypes.DateTime)));
            Assert.That(valueOperand.Value, Is.EqualTo(new DateTime(2010, 1, 2, 3, 4, 5)));
            Assert.That(valueOperand.IncludeTimeValue, Is.True);
        }

        [Test]
        public void test_WHEN_string_based_value_is_datetime_THEN_datetime_operand_is_created()
        {
            var operandBuilder = new OperandBuilder();
            Expression<Func<SPItem, bool>> expr = x => x["Modified"] == ((DataTypes.DateTime)"02.01.2010 03:04:05");
            var operand = operandBuilder.CreateValueOperandForStringBasedSyntax(((BinaryExpression)expr.Body).Right);

            Assert.That(operand, Is.InstanceOf<DateTimeValueOperand>());

            var valueOperand = operand as DateTimeValueOperand;
            Assert.That(valueOperand.Type, Is.EqualTo(typeof(DataTypes.DateTime)));
            Assert.That(valueOperand.Value, Is.EqualTo(new DateTime(2010, 1, 2, 3, 4, 5)));
            Assert.That(valueOperand.IncludeTimeValue, Is.False);
        }

        [Test]
        public void test_WHEN_string_based_value_is_datetime_with_includedatetime_THEN_datetime_operand_is_created()
        {
            var operandBuilder = new OperandBuilder();
            Expression<Func<SPItem, bool>> expr = x => x["Modified"] == ((DataTypes.DateTime)"02.01.2010 03:04:05").IncludeTimeValue();
            var operand = operandBuilder.CreateValueOperandForStringBasedSyntax(((BinaryExpression)expr.Body).Right);

            Assert.That(operand, Is.InstanceOf<DateTimeValueOperand>());

            var valueOperand = operand as DateTimeValueOperand;
            Assert.That(valueOperand.Type, Is.EqualTo(typeof(DataTypes.DateTime)));
            Assert.That(valueOperand.Value, Is.EqualTo(new DateTime(2010, 1, 2, 3, 4, 5)));
            Assert.That(valueOperand.IncludeTimeValue, Is.True);
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


