using System.Web;
using System.Web.Mvc;
using System;
using System.Collections.Generic;

namespace DomeApp.Code
{
    public static class TextExtensions
    {
        public static string GetExcerpt(this string source)
        {
            //TODO Implement a smarter way to get excerpt. for now this only take a third of the text
            // Ideally pass a list of characters to find a delimit the excerpt version of the input
            return source.Substring(0, source.Length / 3);
        }

        public static IHtmlString ToSafeMarkdown(this string source)
        {
            var markdowner = new MarkdownDeep.Markdown();
            var codeTransformers = new MarkdownCodeTransformers();

            var markdown = source;
            markdown = Transform(markdowner.Transform, markdown, codeTransformers.GetTransformers());

            return new MvcHtmlString(markdown);
        }

        private static string Transform(this Func<string,string> markdowner, string input, IEnumerable<Func<string, string>> transformers)
        {
            foreach (var preTransformation in transformers)
                input = preTransformation.Invoke(input);

            return markdowner(input);
        }
    }
}