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
using System.Linq.Expressions;
using System.Text;
using Microsoft.SharePoint;

namespace CamlexNET
{
    internal class GenericException : Exception
    {
        public GenericException(string message, params object[] args) :
            base(string.Format(message, args))
        {}        
    }

    internal class NonSupportedExpressionException : GenericException
    {
        public NonSupportedExpressionException(Expression expr) :
            base(ErrorMessages.NON_SUPPORTED_EXPRESSION, expr)
        {
        }
    }

    internal class NonSupportedExpressionTypeException : GenericException
    {
        public NonSupportedExpressionTypeException(ExpressionType exprType) :
            base(ErrorMessages.NON_SUPPORTED_EXPRESSION_TYPE, exprType)
        {
        }
    }

    internal class NonSupportedOperandTypeException : GenericException
    {
        public NonSupportedOperandTypeException(Type type) :
            base(ErrorMessages.NON_SUPPORTED_OPERAND_TYPE, type)
        {
        }
    }

    internal class NullValueOperandCannotBeTranslatedToCamlException : GenericException
    {
        public NullValueOperandCannotBeTranslatedToCamlException() :
            base(ErrorMessages.NULL_VALUE_OPERAND_CAN_NOT_BE_TRANSLATED_TO_CAML)
        {
        }
    }

    internal class InvalidValueForOperandTypeException : GenericException
    {
        public InvalidValueForOperandTypeException(object value, Type operandType) :
            base(ErrorMessages.INVALID_VALUE_FOR_OPERAND_TYPE, value, operandType)
        {
        }
    }

    internal class InvalidFieldNameForFieldRefException : GenericException
    {
        public InvalidFieldNameForFieldRefException(object value) :
            base(ErrorMessages.INVALID_FIELD_NAME_FOR_FIELD_REF_OPERAND, value)
        {
        }
    }
}
