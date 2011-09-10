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
using CamlexNET.Impl.Operands;
using Microsoft.SharePoint;

namespace CamlexNET
{
    // All exceptions are internal cause we don't want users to implement logic
    // based on exceptions. Is there a case when exceptions from Camlex should be
    // known on client side?

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

    internal class DateTimeOperandModeNotSupportedException : GenericException
    {
        public DateTimeOperandModeNotSupportedException(DateTimeValueOperand.DateTimeValueMode mode) :
            base(ErrorMessages.DATETIME_OPERAND_MODE_NOT_SUPPORTED, mode)
        {
        }
    }

    internal class InvalidValueForFieldRefException : GenericException
    {
        public InvalidValueForFieldRefException(object value) :
            base(ErrorMessages.INVALID_VALUE_FOR_FIELD_REF_OPERAND, value)
        {
        }
    }

    internal class OnlyOrAndBinaryExpressionsAllowedForJoinsExceptions : GenericException
    {
        public OnlyOrAndBinaryExpressionsAllowedForJoinsExceptions() :
            base(ErrorMessages.ONLY_OR_AND_BINARY_EXPRESSIONS_ALLOWED_FOR_JOINS)
        {
        }
    }

    internal class EmptyExpressionsListException : GenericException
    {
        public EmptyExpressionsListException() :
            base(ErrorMessages.EMPTY_EXPRESSIONS_LIST)
        {
        }
    }

    internal class FieldRefOperandShouldContainNameOrIdException : GenericException
    {
        public FieldRefOperandShouldContainNameOrIdException() :
            base(ErrorMessages.EMPTY_EXPRESSIONS_LIST)
        {
        }
    }

    internal class ArrayOperationShouldContainOnlyFieldRefOperandsException : GenericException
    {
        public ArrayOperationShouldContainOnlyFieldRefOperandsException() :
            base(ErrorMessages.ARRAY_OPERATION_SHOULD_CONTAIN_ONLY_FIELD_REF_OPERANDS_EXCEPTION)
        {
        }
    }

    internal class OperationShouldContainFieldRefOperandException : GenericException
    {
        public OperationShouldContainFieldRefOperandException() :
            base(ErrorMessages.OPERATION_SHOULD_CONTAIN_FIELD_REF_OPERAND_EXCEPTION)
        {
        }
    }

    internal class OperationShouldContainTextValueOperandException : GenericException
    {
        public OperationShouldContainTextValueOperandException() :
            base(ErrorMessages.OPERATION_SHOULD_CONTAIN_TEXT_VALUE_OPERAND_EXCEPTION)
        {
        }
    }

    internal class DateTimeValueOperandExpectedException : GenericException
    {
        public DateTimeValueOperandExpectedException() :
            base(ErrorMessages.DATE_TIME_VALUE_OPERAND_EXPECTED_EXCEPTION)
        {
        }
    }

    internal class XmlNotWellFormedException : GenericException
    {
        public XmlNotWellFormedException() :
            base(ErrorMessages.XML_NOT_WELL_FORMED_EXCEPTION)
        {
        }
    }

    internal class IncorrectCamlException : Exception
    {
        public IncorrectCamlException(string tag)
            : base(string.Format("Caml specified for tag '{0}' can not be translated to code", tag))
        { }
    }

    internal class AtLeastOneCamlPartShouldNotBeEmptyException : Exception
    {
        public AtLeastOneCamlPartShouldNotBeEmptyException()
            : base(string.Format("At least one part for the CAML should not be empty: {0}, {1}, {2}, {3}",
                Tags.Where, Tags.OrderBy, Tags.GroupBy, Tags.ViewFields))
        {}
    }

    internal class LinkerFromCamlRequiresTranslatorFromCamlException : Exception
    {
        public LinkerFromCamlRequiresTranslatorFromCamlException(Type type)
            : base(string.Format("Incorrect translator type was passed to the linker: '{0}'. Linker from CAML requires translator from CAML", type))
        {}
    }

    internal class CantParseBooleanAttributeException : Exception
    {
        public CantParseBooleanAttributeException(string attr) :
            base(string.Format("Can't parse boolean attribute '{0}'", attr))
        {
        }
    }

    internal class CantParseIntegerAttributeException : Exception
    {
        public CantParseIntegerAttributeException(string attr) :
            base(string.Format("Can't parse integer attribute '{0}'", attr))
        {
        }
    }
}
