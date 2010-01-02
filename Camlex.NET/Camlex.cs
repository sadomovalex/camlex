using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Impl;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;


namespace Camlex.NET
{
    public static class Camlex
    {
        private static ITranslatorFactory translatorFactory;
        static Camlex()
        {
            // factories setup
            var operandBuilder = new OperandBuilder();
            var analyzerFactory = new AnalyzerFactory(operandBuilder);
            translatorFactory = new TranslatorFactory(analyzerFactory);
        }

        public static string Where(Expression<Func<SPItem, bool>> expr)
        {
            var translator = translatorFactory.Create(expr.Body.NodeType);
            return translator.Translate(expr);
        }
    }
}
