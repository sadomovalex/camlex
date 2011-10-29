#region Copyright(c) Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
// -----------------------------------------------------------------------------
// Copyright(c) 2010 Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
//   1. No Trademark License - Microsoft Public License (Ms-PL) does not grant you rights to use
//      authors names, logos, or trademarks.
//   2. If you distribute any portion of the software, you must retain all copyright,
//      patent, trademark, and attribution notices that are present in the software.
//   3. If you distribute any portion of the software in source code form, you may do
//      so only under this license by including a complete copy of Microsoft Public License (Ms-PL)
//      with your distribution. If you distribute any portion of the software in compiled
//      or object code form, you may only do so under a license that complies with
//      Microsoft Public License (Ms-PL).
//   4. The names of the authors may not be used to endorse or promote products
//      derived from this software without specific prior written permission.
//
// The software is licensed "as-is." You bear the risk of using it. The authors
// give no express warranties, guarantees or conditions. You may have additional consumer
// rights under your local laws which this license cannot change. To the extent permitted
// under your local laws, the authors exclude the implied warranties of merchantability,
// fitness for a particular purpose and non-infringement.
// -----------------------------------------------------------------------------
#endregion

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
            if (!base.IsValid()) return false;
            if (el.Attributes().Count() > 0) return false;

            // check presence of FieldRef tag with ID or Name attribute
            if (!this.hasValidFieldRefElement()) return false;

            // check presence of Value tag with Type attribute
            if (!hasValidValueElement()) return false;

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
            return doesOperationSupportValueType(typeAttribute.Value, valueElement.Value);
        }

        protected virtual bool doesOperationSupportValueType(string valueType, string value)
        {
            try
            {
                if (valueType == typeof(DataTypes.Boolean).Name) new BooleanValueOperand(value);
                else if (valueType == typeof(DataTypes.DateTime).Name) new DateTimeValueOperand(value, false);
                else if (valueType == typeof(DataTypes.Guid).Name) new GuidValueOperand(value);
                else if (valueType == typeof(DataTypes.Integer).Name) new IntegerValueOperand(value);
                else if (valueType == typeof(DataTypes.Lookup).Name) new LookupValueValueOperand(value);
                // string based types
                else if (valueType == typeof(DataTypes.AllDayEvent).Name ||
                    valueType == typeof(DataTypes.Attachments).Name ||
                    valueType == typeof(DataTypes.Calculated).Name ||
                    valueType == typeof(DataTypes.Choice).Name ||
                    valueType == typeof(DataTypes.Computed).Name ||
                    valueType == typeof(DataTypes.ContentTypeId).Name ||
                    valueType == typeof(DataTypes.Counter).Name ||
                    valueType == typeof(DataTypes.CrossProjectLink).Name ||
                    valueType == typeof(DataTypes.Currency).Name ||
                    valueType == typeof(DataTypes.Error).Name ||
                    valueType == typeof(DataTypes.File).Name ||
                    valueType == typeof(DataTypes.GridChoice).Name ||
                    valueType == typeof(DataTypes.Invalid).Name ||
                    valueType == typeof(DataTypes.MaxItems).Name ||
                    valueType == typeof(DataTypes.ModStat).Name ||
                    valueType == typeof(DataTypes.MultiChoice).Name ||
                    valueType == typeof(DataTypes.Note).Name ||
                    valueType == typeof(DataTypes.Number).Name ||
                    valueType == typeof(DataTypes.PageSeparator).Name ||
                    valueType == typeof(DataTypes.Recurrence).Name ||
                    valueType == typeof(DataTypes.Text).Name ||
                    valueType == typeof(DataTypes.ThreadIndex).Name ||
                    valueType == typeof(DataTypes.Threading).Name ||
                    valueType == typeof(DataTypes.URL).Name ||
                    valueType == typeof(DataTypes.User).Name ||
                    valueType == typeof(DataTypes.WorkflowEventType).Name ||
                    valueType == typeof(DataTypes.WorkflowStatus).Name
                    )
                {
                    var type = typeof (DataTypes).Assembly.GetType(string.Format("CamlexNET.DataTypes.{0}", valueType));
                    new GenericStringBasedValueOperand(type, value);
                }
                else return false;
            }
            catch (InvalidValueForOperandTypeException) { return false; }
            return true;
        }

        protected IOperation getOperation<T>(Func<IOperand, IOperand, T> constructor)
            where T : IOperation
        {
            if (!IsValid())
                throw new CamlAnalysisException(string.Format(
                    "Can't create {0} from the following xml: {1}", typeof(T).Name, el));

            var fieldRefElement = el.Elements(Tags.FieldRef).First();
            var fieldRefOperand = operandBuilder.CreateFieldRefOperand(fieldRefElement);
            var valueOperand = operandBuilder.CreateValueOperand(el);
            return constructor(fieldRefOperand, valueOperand);
        }
    }
}