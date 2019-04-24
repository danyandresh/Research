using System.Web;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DomeApp.Code
{
    public static class TextExtensions
    {
        public static IHtmlString ToSafeMarkdown(this string source)
        {
            var markdowner = new MarkdownDeep.Markdown();
            var codeTransformers = new MarkdownCodeTransformers();

            var markdown = source;
            markdown = Transform(markdowner.Transform, markdown, codeTransformers.GetTransformers());

            return new MvcHtmlString(markdown);
        }

        private static string Transform(this Func<string, string> markdowner, string input, IEnumerable<Func<string, string>> transformers)
        {
            foreach (var preTransformation in transformers)
                input = preTransformation.Invoke(input);

            return markdowner(input);
        }
    }
}