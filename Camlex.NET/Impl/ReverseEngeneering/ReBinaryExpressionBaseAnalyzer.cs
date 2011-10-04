using System;
using System.Linq;
using System.Xml.Linq;
using CamlexNET.Impl.Operands;
using CamlexNET.Interfaces;
using CamlexNET.Interfaces.ReverseEngeneering;

namespace CamlexNET.Impl.ReverseEngeneering
{
    internal abstract class ReBinaryExpressionBaseAnalyzer : ReBaseAnalyzer
    {
        protected ReBinaryExpressionBaseAnalyzer(XElement el, IReOperandBuilder operandBuilder) 
            : base(el, operandBuilder)
        {
        }

        public override bool IsValid()
        {
            if (!base.IsValid())
            {
                return false;
            }
            if (el.Attributes().Count() > 0)
            {
                return false;
            }

            // check presence of FieldRef tag with ID or Name attribute
            if (!this.hasValidFieldRefElement())
            {
                return false;
            }

            // check presence of Value tag with Type attribute
            if (!hasValidValueElement())
            {
                return false;
            }

            return true;
        }

        protected bool hasValidValueElement()
        {
            if (el.Elements(Tags.Value).Count() != 1) return false;
            var valueElement = el.Elements(Tags.Value).First();
            var typeAttribute = valueElement.Attributes()
                .Where(a => a.Name == Attributes.Type).FirstOrDefault();
            if (typeAttribute == null) return false;

            // check whether we support this value type
            if (typeAttribute.Value != typeof(DataTypes.Text).Name &&
                string.IsNullOrEmpty(valueElement.Value)) return false;
            var value = valueElement.Value;
            try
            {
                if (typeAttribute.Value == typeof(DataTypes.Boolean).Name) new BooleanValueOperand(value);
                else if (typeAttribute.Value == typeof(DataTypes.DateTime).Name) new DateTimeValueOperand(value, false);
                else if (typeAttribute.Value == typeof(DataTypes.Guid).Name) new GuidValueOperand(value);
                else if (typeAttribute.Value == typeof(DataTypes.Integer).Name) new IntegerValueOperand(value);
                else if (typeAttribute.Value == typeof(DataTypes.Lookup).Name) new LookupValueValueOperand(value);
                else if (typeAttribute.Value == typeof(DataTypes.Text).Name) { }
                else throw new InvalidValueForOperandTypeException(null, null);
            }
            catch (InvalidValueForOperandTypeException) { return false; }
            return true;
        }

        protected bool hasValidFieldRefElement()
        {
            if (el.Elements(Tags.FieldRef).Count() != 1) return false;
            var fieldRefElement = el.Elements(Tags.FieldRef).First();
            var isIdOrNamePresent = fieldRefElement.Attributes()
                .Any(a => a.Name == Attributes.ID || a.Name == Attributes.Name);
            if (!isIdOrNamePresent) return false;
            return true;
        }

        protected IOperation getOperation<T>(Func<IOperand, IOperand, T> constructor)
            where T : IOperation
        {
            if (!IsValid())
                throw new CamlAnalysisException(string.Format(
                    "Can't create {0} from the following xml: {1}", typeof(T).Name, el));

            var fieldRefElement = this.el.Elements(Tags.FieldRef).First();
            var valueElement = this.el.Elements(Tags.Value).First();

            var fieldRefOperand = this.operandBuilder.CreateFieldRefOperand(fieldRefElement);
            var valueOperand = this.operandBuilder.CreateValueOperand(valueElement, fieldRefElement);
            return constructor(fieldRefOperand, valueOperand);
        }
    }
}