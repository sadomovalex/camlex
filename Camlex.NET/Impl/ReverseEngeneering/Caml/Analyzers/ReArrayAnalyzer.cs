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
using CamlexNET.Impl.Operations.Array;
using CamlexNET.Interfaces;
using CamlexNET.Interfaces.ReverseEngeneering;

namespace CamlexNET.Impl.ReverseEngeneering.Caml.Analyzers
{
    internal class ReArrayAnalyzer : ReBaseAnalyzer
    {
        public ReArrayAnalyzer(XElement el, IReOperandBuilder operandBuilder) :
            base(el, operandBuilder)
        {
        }

        public override bool IsValid()
        {
            if (!base.IsValid())
            {
                return false;
            }
            if (this.el.Descendants(Tags.FieldRef).Count() == 0)
            {
                return false;
            }

            var elements = this.el.Descendants().Where(e => e.Name == Tags.FieldRef &&
                                             (e.Attributes().Any(
                                                 a => a.Name == Attributes.ID || a.Name == Attributes.Name)));

            return elements.Count() > 0;
        }

        public override IOperation GetOperation()
        {
            if (!IsValid())
            {
                throw new CamlAnalysisException(string.Format("Xml element '{0}' is not supported", el));
            }
            var operands = getFieldRefOperands(this.el);
            return new ArrayOperation(null, operands);
        }

        private IOperand[] getFieldRefOperands(XElement el)
        {
            var operands = new List<IOperand>();
            el.Descendants(Tags.FieldRef).ToList().ForEach(
                e =>
                    {
                        IOperand operand = null;
                        var attr = e.Attributes().FirstOrDefault(a => a.Name == Attributes.Ascending);
                        if (attr != null && (attr.Value == new Camlex.Asc().ToString() || attr.Value == new Camlex.Desc().ToString()))
                        {
                            var direction = (attr.Value == new Camlex.Asc().ToString()
                                                ? (Camlex.OrderDirection)new Camlex.Asc()
                                                : new Camlex.Desc());
                            operand = this.operandBuilder.CreateFieldRefOperandWithOrdering(e, direction);
                        }
                        else
                        {
                            operand = this.operandBuilder.CreateFieldRefOperand(e);
                        }

                        if (operand != null)
                        {
                            operands.Add(operand);
                        }
                    });
            return operands.ToArray();
        }
    }
}


