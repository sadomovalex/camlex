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
using System.Xml.Linq;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operands
{
    internal class FieldRefOperand : IOperand
    {
        protected readonly string fieldName;
        private readonly Guid? id;
        private List<KeyValuePair<string, string>> attributes;

        public string FieldName
        {
            get
            {
                return this.fieldName;
            }
        }

//        public Guid? FieldId
//        {
//            get
//            {
//                return id;
//            }
//        }

        protected FieldRefOperand(List<KeyValuePair<string, string>> attributes)
        {
            this.attributes = attributes;
        }

        public FieldRefOperand(string fieldName)
        {
            this.fieldName = fieldName;
        }

        public FieldRefOperand(string fieldName, List<KeyValuePair<string, string>> attributes) :
            this (attributes)
        {
            this.fieldName = fieldName;
        }

        public FieldRefOperand(Guid id)
        {
            this.id = id;
        }

        public FieldRefOperand(Guid id, List<KeyValuePair<string, string>> attributes) :
            this(attributes)
        {
            this.id = id;
        }

        public virtual XElement ToCaml()
        {
            XElement element;
            if (this.id != null)
            {
                element = new XElement(Tags.FieldRef, new XAttribute(Attributes.ID, this.id.Value));
            }
            else
            {
                element = new XElement(Tags.FieldRef, new XAttribute(Attributes.Name, this.fieldName));
            }

            if (this.attributes != null)
            {
                foreach (var attr in this.attributes)
                {
                    // should not specify id or name twice
                    if (string.Compare(attr.Key, Attributes.ID, true) == 0 ||
                        string.Compare(attr.Key, Attributes.Name, true) == 0)
                    {
                        continue;
                    }
                    element.Add(new XAttribute(attr.Key, attr.Value));
                }
            }
            return element;
        }
    }
}


