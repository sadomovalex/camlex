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

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CamlexNET.Impl.Operations.ProjectedField;
using CamlexNET.Interfaces;
using CamlexNET.Interfaces.ReverseEngeneering;

namespace CamlexNET.Impl.ReverseEngeneering.Caml.Analyzers
{
    internal class ReProjectedFieldsAnalyzer : ReBaseAnalyzer
    {
        public ReProjectedFieldsAnalyzer(XElement el, IReOperandBuilder operandBuilder) :
            base(el, operandBuilder)
        {
        }

        public override bool IsValid()
        {
            if (!base.IsValid()) return false;
            if (el.Name != Tags.ProjectedFields) return false;
            foreach (var child in el.Elements())
            {
                if (!this.isValid(child))
                {
                    return false;
                }
            }
            return true;
        }

        private bool isValid(XElement el)
        {
            if (el == null)
            {
                return false;
            }
            if (el.Name != Tags.Field)
            {
                return false;
            }
            if (!el.Attributes().Any(a => a.Name == Attributes.Name) || string.IsNullOrEmpty(el.Attributes().First(a => a.Name == Attributes.Name).Value))
            {
                return false;
            }
            if (!el.Attributes().Any(a => a.Name == Attributes.Type) || string.IsNullOrEmpty(el.Attributes().First(a => a.Name == Attributes.Type).Value))
            {
                return false;
            }
            if (!el.Attributes().Any(a => a.Name == Attributes.List) || string.IsNullOrEmpty(el.Attributes().First(a => a.Name == Attributes.List).Value))
            {
                return false;
            }
            if (!el.Attributes().Any(a => a.Name == Attributes.ShowField) || string.IsNullOrEmpty(el.Attributes().First(a => a.Name == Attributes.ShowField).Value))
            {
                return false;
            }

            return true;
        }

        public override IOperation GetOperation()
        {
            return null;
        }

        public override List<IOperation> GetOperations()
        {
            if (!this.IsValid())
            {
                throw new CamlAnalysisException(string.Format(
                    "Can't create {0} from the following xml: {1}", typeof(ProjectedFieldOperation).Name, el));
            }
            
            var operands = this.getOperands();
            if (operands == null)
            {
                return new List<IOperation>();
            }

            var result = new List<IOperation>();
            foreach (var operand in operands)
            {
                result.Add(new ProjectedFieldOperation(null, operand));
            }
            return result;
        }

        private List<IOperand> getOperands()
        {
            var operands = new List<IOperand>();
            el.Elements(Tags.Field).ToList().ForEach(
                e =>
                {
                    var operand = this.operandBuilder.CreateFieldRefOperandForProjectedField(e);
                    operands.Add(operand);
                });
            return operands;
        }
    }
}