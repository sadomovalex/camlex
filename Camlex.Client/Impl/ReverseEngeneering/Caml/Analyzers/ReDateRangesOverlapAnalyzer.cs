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

using System.Linq;
using System.Xml.Linq;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.DateRangesOverlap;
using CamlexNET.Interfaces;
using CamlexNET.Interfaces.ReverseEngeneering;

namespace CamlexNET.Impl.ReverseEngeneering.Caml.Analyzers
{
    internal class ReDateRangesOverlapAnalyzer : ReBaseAnalyzer
    {
        public ReDateRangesOverlapAnalyzer(XElement el, IReOperandBuilder operandBuilder) 
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
            if (!this.hasValidFieldRefElements())
            {
                return false;
            }
            if (!this.hasValidValueElement())
            {
                return false;
            }
            return true;
        }

        protected bool hasValidFieldRefElements()
        {
            if (el.Elements(Tags.FieldRef).Count() != 3) return false;
            var result = true;
            el.Elements(Tags.FieldRef).ToList().ToList().ForEach(fieldRefElement =>
            {
                var isIdOrNamePresent = fieldRefElement.Attributes()
                    .Any(a => a.Name == Attributes.ID || a.Name == Attributes.Name);
                if (!isIdOrNamePresent) result = false;
            });
            return result;
        }

        protected bool hasValidValueElement()
        {
            if (el.Elements(Tags.Value).Count() != 1)
            {
                return false;
            }
            var valueElement = el.Elements(Tags.Value).First();
            var typeAttribute = valueElement.Attributes()
                .Where(a => a.Name == Attributes.Type).FirstOrDefault();
            if (typeAttribute == null)
            {
                return false;
            }
            if (typeAttribute.Value != typeof (DataTypes.DateTime).Name)
            {
                return false;
            }
            if (string.IsNullOrEmpty(valueElement.Value) && valueElement.Elements().Count() != 1)
            {
                return false;
            }
            try
            {
                if (!string.IsNullOrEmpty(valueElement.Value))
                {
                    new DateTimeValueOperand(valueElement.Value, false);
                }
                else if (valueElement.Elements().Count() == 1)
                {
                    new DateTimeValueOperand(valueElement.Elements().First().Name.ToString(), false);
                }
            }
            catch (InvalidValueForOperandTypeException)
            {
                return false;
            }
            return true;
        }

        public override IOperation GetOperation()
        {
            if (!this.IsValid())
            {
                throw new CamlAnalysisException(string.Format(
                    "Can't create {0} from the following xml: {1}", typeof (DateRangesOverlapOperation).Name, el));
            }

            var startFieldRefElement = el.Elements(Tags.FieldRef).First();
            var stopFieldRefElement = el.Elements(Tags.FieldRef).Skip(1).First();
            var recurrenceFieldRefElement = el.Elements(Tags.FieldRef).Skip(2).First();

            var startFieldRefOperand = operandBuilder.CreateFieldRefOperand(startFieldRefElement);
            var stopFieldRefOperand = operandBuilder.CreateFieldRefOperand(stopFieldRefElement);
            var recurrenceFieldRefOperand = operandBuilder.CreateFieldRefOperand(recurrenceFieldRefElement);

            var valueOperand = operandBuilder.CreateValueOperand(el, false);

            return new DateRangesOverlapOperation(null, 
                startFieldRefOperand, stopFieldRefOperand, recurrenceFieldRefOperand, valueOperand);
        }
    }
}