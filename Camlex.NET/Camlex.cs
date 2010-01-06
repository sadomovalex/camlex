using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;
using Camlex.NET.Impl;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;

namespace Camlex.NET
{
    public class Camlex
    {
        #region OrderBy functionality
        /// <summary>Marker class representing ASC order direction for "OrderBy" functionality</summary>
        public class OrderDirection
        {
            public static OrderDirection Default { get { return new None(); } }
            public static OrderDirection Convert(Type type)
            {
                if (type == typeof(Asc)) return new Asc();
                if (type == typeof(Desc)) return new Desc();
                return Default;
            }
            public bool IsDefault()
            {
                return this.GetType() == Default.GetType();
            }
        }
        /// <summary>Marker class representing absence of order direction for "OrderBy" functionality</summary>
        public class None : OrderDirection { public override string ToString() { return string.Empty; } }
        /// <summary>Marker class representing ASC order direction for "OrderBy" functionality</summary>
        public class Asc : OrderDirection { public override string ToString() { return true.ToString(); } }
        /// <summary>Marker class representing DESC order direction for "OrderBy" functionality</summary>
        public class Desc : OrderDirection { public override string ToString() { return false.ToString(); } }
        #endregion

        private static ITranslatorFactory translatorFactory;

        static Camlex()
        {
            // factories setup
            var operandBuilder = new OperandBuilder();
            var operationResultBuilder = new OperationResultBuilder();
            var analyzerFactory = new AnalyzerFactory(operandBuilder, operationResultBuilder);
            translatorFactory = new TranslatorFactory(analyzerFactory);
        }

        public static IQuery Query()
        {
            return new Query(translatorFactory);
        }
    }
}
