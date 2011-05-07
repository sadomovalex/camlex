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
using System.Text;

namespace CamlexNET
{
    internal static class ErrorMessages
    {
        public const string NON_SUPPORTED_EXPRESSION =  "Expression '{0}' can not be translated into CAML";
        public const string NON_SUPPORTED_EXPRESSION_TYPE = "Expression type '{0}' is not supported";
        public const string NON_SUPPORTED_OPERAND_TYPE = "Operand type '{0}' is not supported";
        public const string NULL_VALUE_OPERAND_CAN_NOT_BE_TRANSLATED_TO_CAML =
            "Value is null. Null rvalue is allowed only with '==' (IsNull) and '!=' (IsNotNull) operations. " +
            "Also null rvalue should not be casted to DataTypes.*";
        public const string INVALID_VALUE_FOR_OPERAND_TYPE = "Value '{0}' is not valid for operand type '{1}'";
        //public const string INVALID_FIELD_NAME_FOR_FIELD_REF_OPERAND = "Value '{0}' is not valid field name for FieldRef operand";
        public const string INVALID_VALUE_FOR_FIELD_REF_OPERAND = "Value '{0}' is not valid field name or field id for FieldRef operand";
        public const string ONLY_OR_AND_BINARY_EXPRESSIONS_ALLOWED_FOR_JOINS = "Only 'OrElse' and 'AnsAlso' binary expressions are allowed for logical joins";
        public const string EMPTY_EXPRESSIONS_LIST = "Can not join list of expressions because it is empty. You should specify at least one expression in list";
        //public const string DIFFERENT_ARGUMENTS_NAMES = "Expressions have different arguments names. All expressions should have the same argument name";
        //public const string INVALID_LOOKUP_ID = "Value '{0}' is not valid for lookup id. Lookup id should be integer";
        public const string FIELD_REF_SHOULD_CONTAIN_NAME_OR_ID = "Field ref element should contain at least one attribute: Name or ID";
        public const string XML_NOT_WELL_FORMED_EXCEPTION = "Xml not well formed";
        public const string DATETIME_OPERAND_MODE_NOT_SUPPORTED = "Mode '{0}' is not supported for DateTime operand";
        public const string ARRAY_OPERATION_SHOULD_CONTAIN_ONLY_FIELD_REF_OPERANDS_EXCEPTION = "Array operation should contain only field ref operands";
        public const string OPERATION_SHOULD_CONTAIN_FIELD_REF_OPERAND_EXCEPTION = "Operation should contain FieldRefOperand";
        public const string OPERATION_SHOULD_CONTAIN_TEXT_VALUE_OPERAND_EXCEPTION = "hOperation should contain TextValueOperand";
        public const string DATE_TIME_VALUE_OPERAND_EXPECTED_EXCEPTION = "DateRangesOverlap operation should have DateTimeValueOperand";
    }
}
