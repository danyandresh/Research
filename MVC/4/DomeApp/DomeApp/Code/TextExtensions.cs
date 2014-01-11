using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}