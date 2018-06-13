using System.Collections.Generic;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.In;
using CamlexNET.Interfaces;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Operations
{
    [TestFixture]
    public class InOperationTests
    {
        [Test]
        public void test_THAT_in_operation_IS_transformed_to_expression_properly()
        {
            var fieldRef = new FieldRefOperand("test");

            var values = new List<IOperand>();
            values.Add(new IntegerValueOperand(1));
            values.Add(new IntegerValueOperand(2));
            values.Add(new IntegerValueOperand(3));
            var valuesOperand = new ValuesValueOperand(values);
            var operation = new InOperation(null, fieldRef, valuesOperand);
            var expr = operation.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("new [] {1, 2, 3}.Contains(Convert(x.get_Item(\"test\")))"));
        }

        [Test]
        [ExpectedException(typeof(CantDetermineValueTypeException))]
        public void test_WHEN_different_types_are_provided_THEN_exception_is_thrown()
        {
            var fieldRef = new FieldRefOperand("test");

            var values = new List<IOperand>();
            values.Add(new IntegerValueOperand(1));
            values.Add(new TextValueOperand("2"));
            var valuesOperand = new ValuesValueOperand(values);
            var operation = new InOperation(null, fieldRef, valuesOperand);
            var expr = operation.ToExpression();
        }
    }
}
