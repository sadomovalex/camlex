#region Copyright(c) Alexey Sadomov, Vladimir Timashkov, Stef Heyenrath. All Rights Reserved.
// -----------------------------------------------------------------------------
// Copyright(c) 2010 Alexey Sadomov, Vladimir Timashkov, Stef Heyenrath. All Rights Reserved.
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
using CamlexNET.Impl.Operations.BeginsWith;
using CamlexNET.Interfaces;
using CamlexNET.Interfaces.ReverseEngeneering;

namespace CamlexNET.Impl.ReverseEngeneering.Caml.Analyzers
{
    internal class ReConstantAnalyzer : ReBaseAnalyzer
    {
        private string tag;
        private Type type;

        public ReConstantAnalyzer(XElement el, IReOperandBuilder operandBuilder, string tag, Type type) :
            base(el, operandBuilder)
        {
            this.tag = tag;
            this.type = type;
        }

        public override bool IsValid()
        {
			if (!base.IsValid())
			{
				return false;
			}

			if (el.Attributes().Any())
			{
				return false;
			}

			if (el.Elements().Any())
			{
				return false;
			}

            if (el.Name != this.tag)
            {
	            return false;
            }

//	        int rowLimit;
//			if (!int.TryParse(el.Value, out rowLimit))
//			{
//				return false;
//			}
            try
            {
                Convert.ChangeType(el.Value, this.type);
            }
            catch
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
                    "Can't create {0} from the following xml: {1}", typeof(ConstantOperation).Name, el));
			}

			var operand = this.operandBuilder.CreateConstantOperand(el, type);
			return new ConstantOperation(null, operand);
        }
    }
}