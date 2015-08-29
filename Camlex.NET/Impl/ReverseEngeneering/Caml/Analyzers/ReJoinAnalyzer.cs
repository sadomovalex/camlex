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
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CamlexNET.Impl.Operations.Eq;
using CamlexNET.Impl.Operations.Join;
using CamlexNET.Interfaces;
using CamlexNET.Interfaces.ReverseEngeneering;

namespace CamlexNET.Impl.ReverseEngeneering.Caml.Analyzers
{
    internal class ReJoinAnalyzer : ReBaseAnalyzer
    {
        public ReJoinAnalyzer(XElement el, IReOperandBuilder operandBuilder) :
            base(el, operandBuilder)
        {
        }

        public override bool IsValid()
        {
            if (!base.IsValid()) return false;
            if (el.Name != Tags.Joins) return false;
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
            if (el.Name != Tags.Join)
            {
                return false;
            }
            if (!el.Attributes().Any(a => a.Name == Attributes.Type) || string.IsNullOrEmpty(el.Attributes().First(a => a.Name == Attributes.Type).Value))
            {
                return false;
            }
            var typeStr = el.Attributes().First(a => a.Name == Attributes.Type).Value;
            //if (!Enum.IsDefined(typeof (JoinType), typeStr))
            if (!Enum.GetNames(typeof(JoinType)).Any(t => string.Compare(t.ToString(), typeStr, true) == 0))
            {
                return false;
            }
            if (!el.Attributes().Any(a => a.Name == Attributes.ListAlias) || string.IsNullOrEmpty(el.Attributes().First(a => a.Name == Attributes.ListAlias).Value))
            {
                return false;
            }

            var eq = el.Elements();
            if (eq.Count() != 1 || eq.ElementAt(0).Name != Tags.Eq)
            {
                return false;
            }

            var childs = eq.Elements();
            if (childs.Count() != 2)
            {
                return false;
            }

            if (!this.isChildElemValid(childs.ElementAt(0), Attributes.Name, Attributes.RefType))
            {
                return false;
            }

            if (!this.isChildElemValid(childs.ElementAt(1), Attributes.Name, Attributes.List))
            {
                return false;
            }
            return true;
        }

        private bool isChildElemValid(XElement el, string attr1, string attr2)
        {
            if (el == null)
            {
                return false;
            }
            if (el.Name != Tags.FieldRef)
            {
                return false;
            }
            if (!el.HasAttributes)
            {
                return false;
            }
            if (!el.Attributes().Any(a => a.Name == attr1) || string.IsNullOrEmpty(el.Attributes().First(a => a.Name == attr1).Value))
            {
                return false;
            }
            if (!el.Attributes().Any(a => a.Name == attr2) || string.IsNullOrEmpty(el.Attributes().First(a => a.Name == attr2).Value))
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
            var result = new List<IOperation>();
            foreach (var child in el.Elements())
            {
                var typeStr = child.Attributes().First(a => a.Name == Attributes.Type).Value;
                var operands = this.getFieldRefOperands(child);
                result.Add(new JoinOperation(null, operands[0], operands[1], (JoinType)Enum.Parse(typeof(JoinType), typeStr, true)));
            }
            return result;
        }

        private IOperand[] getFieldRefOperands(XElement el)
        {
            var operands = new List<IOperand>();
            var eq = el.Element(Tags.Eq);
            eq.Elements(Tags.FieldRef).ToList().ForEach(
                e =>
                {
                    var operand = this.operandBuilder.CreateFieldRefOperand(e);
                    if (operand != null)
                    {
                        operands.Add(operand);
                    }
                });
            return operands.ToArray();
        }
    }
}