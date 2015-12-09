using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ColorCode;

namespace DomeApp.Code
{
    /// <summary>
    /// Thank to danielwertheim, whose Kiwi project served as inspiration for these transformers; find more at https://github.com/danielwertheim/Kiwi
    /// </summary>
    public class MarkdownCodeTransformers
    {
        private const string CodeBlockMarker = "```";

        private readonly CodeColorizer syntaxHighlighter;
        private Regex cSharpPretransformation;
        private Regex javascriptPretransformation;
        private Regex htmlPretransformation;
        private Regex cssPretransformation;
        private Regex xmlPretransformation;
        private Regex genericPretransformation;

        public Func<string, string> LineBreaks { get; set; }

        public Func<string, string> HtmlEncoding { get; set; }

        public Func<string, string> GenericCodeBlock { get; set; }

        public Func<string, string> CSharp { get; set; }

        public Func<string, string> JavaScript { get; set; }

        public Func<string, string> Html { get; set; }

        public Func<string, string> Css { get; set; }

        public Func<string, string> Xml { get; set; }

        public MarkdownCodeTransformers()
        {
            syntaxHighlighter = new CodeColorizer();

            InitializeTransformers();
        }

        private void InitializeTransformers()
        {
            OnInitializeTranformerRegExs();
            OnInitializeTranformerFuncs();
        }

        protected virtual void OnInitializeTranformerRegExs()
        {
            const string format = @"^{0}([\s]*){1}(.*?){0}";

            Func<string[], Regex> buildMarkupRegex = (formatParams) => new Regex(string.Format(format, formatParams), RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);


            cSharpPretransformation = buildMarkupRegex(new[] { CodeBlockMarker, "(c#|csharp){1}" });
            javascriptPretransformation = buildMarkupRegex(new[] { CodeBlockMarker, "(js|javascript){1}" });
            htmlPretransformation = buildMarkupRegex(new[] { CodeBlockMarker, "html" });
            cssPretransformation = buildMarkupRegex(new[] { CodeBlockMarker, "css" });
            xmlPretransformation = buildMarkupRegex(new[] { CodeBlockMarker, "xml" });
            genericPretransformation = buildMarkupRegex(new[] { CodeBlockMarker, string.Empty });
        }

        protected virtual void OnInitializeTranformerFuncs()
        {
            LineBreaks = mc => mc.Replace("\r\n", "\n");
            HtmlEncoding = mc => mc.Replace(@"\<", "&lt;").Replace(@"\>", "&gt;");
            CSharp = mc => cSharpPretransformation.Replace(mc, m => FormatAndColorize(m.Value, Languages.CSharp));
            JavaScript = mc => javascriptPretransformation.Replace(mc, m => FormatAndColorize(m.Value, Languages.JavaScript));
            Html = mc => htmlPretransformation.Replace(mc, m => FormatAndColorize(m.Value, Languages.Html));
            Css = mc => cssPretransformation.Replace(mc, m => FormatAndColorize(m.Value, Languages.Css));
            Xml = mc => xmlPretransformation.Replace(mc, m => FormatAndColorize(m.Value, Languages.Xml));
            GenericCodeBlock = mc => genericPretransformation.Replace(mc, m => FormatAndColorize(m.Value));
        }

        protected virtual string FormatAndColorize(string value, ILanguage language = null)
        {
            var output = new StringBuilder();
            foreach (var line in value.Split(new[] { "\n" }, StringSplitOptions.None).Where(s => !s.StartsWith(CodeBlockMarker)))
            {
                if (language == null)
                    output.Append(new string(' ', 4));

                if (line == "\n")
                    output.AppendLine();
                else
                    output.AppendLine(line);
            }

            return language != null
                    ? syntaxHighlighter.Colorize(output.ToString().Trim(), language)
                    : output.ToString();
        }

        public virtual IEnumerable<Func<string, string>> GetTransformers()
        {
            yield return LineBreaks;

            yield return CSharp;

            yield return JavaScript;

            yield return Html;

            yield return Css;

            yield return Xml;

            yield return GenericCodeBlock;

            yield return HtmlEncoding;
        }
    }
}