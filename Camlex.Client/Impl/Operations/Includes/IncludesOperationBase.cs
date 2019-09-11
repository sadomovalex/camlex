using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CamlexNET.Impl.Operands;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operations.Includes
{
    internal class IncludesOperationBase : BinaryOperationBase
    {
        public IncludesOperationBase(IOperationResultBuilder operationResultBuilder,
            IOperand fieldRefOperand, IOperand valueOperand)
            : base(operationResultBuilder, fieldRefOperand, valueOperand)
        {
        }

        public override IOperationResult ToResult()
        {
            var result = new XElement(Tags.Includes,
                             fieldRefOperand.ToCaml(),
                             valueOperand.ToCaml());
            return operationResultBuilder.CreateResult(result);
        }

        public override Expression ToExpression()
        {
            // in the field ref operand we don't know what type of the value it has. So perform
            // conversion here
            var fieldRef = this.getFieldRefOperandExpression();
            var value = this.getValueOperandExpression();

            if (fieldRef.Type != typeof(object))
            {
                fieldRef = Expression.Convert(fieldRef, typeof(object));
            }

            if (value.Type != typeof(object))
            {
                value = Expression.Convert(value, typeof(object));
            }

            bool hasLookupId = false;
            List<KeyValuePair<string, string>> attrs = null;
            if (this.fieldRefOperand is FieldRefOperand)
            {
                attrs = (this.fieldRefOperand as FieldRefOperand).Attributes;
                if (attrs != null)
                {
                    hasLookupId = attrs.Any(a => a.Key == Attributes.LookupId);
                }
            }

            MethodInfo mi = null;
            if (hasLookupId)
            {
                mi = typeof(ExtensionMethods).GetMethod(ReflectionHelper.IncludesMethodName,
                    new[]
                    {
                        typeof(object), typeof(object), typeof(bool)
                    });
                return Expression.Call(mi, fieldRef, value, Expression.Constant(true));
            }
            else
            {
                mi = typeof(ExtensionMethods).GetMethod(ReflectionHelper.IncludesMethodName,
                    new[]
                    {
                        typeof(object), typeof(object)
                    });
                return Expression.Call(mi, fieldRef, value);
            }
        }
    }
}
