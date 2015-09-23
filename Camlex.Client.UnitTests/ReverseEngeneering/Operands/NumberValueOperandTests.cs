using CamlexNET.Impl.Operands;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Operands
{
    [TestFixture]
    [SetCulture("ru-RU")]
    public class NumberValueOperandTests
    {
        [Test]
        [TestCase(3245, "3245")]
        [TestCase(32.45, "32,45")]
        public void test_THAT_operand_IS_conveted_to_expression_correctly(double value, string result)
        {
            var op = new NumberValueOperand(value);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo(result));
        }
    }
}