using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace WrittenQuality.Models
{
    public static class Text
    {
        public static string TitleToCamel(this string name)
        {

            var m = Regex.Match(name, @"[a-zA-Z]"); // first character we run into
            if (m.Success)
            {
                name = name.Substring(0, m.Index) + System.Char.ToLowerInvariant(name[m.Index]) + name.Substring(m.Index + 1);
            }

            return name;
        }

        public static string CamelToTitle(this string name)
        {

            var m = Regex.Match(name, @"^[a-zA-Z]"); // first character we run into
            if (m.Success)
            {
                name = name.Substring(0, m.Index) + System.Char.ToUpperInvariant(name[m.Index]) + name.Substring(m.Index + 1);
            }

            return name;
        }

        public static string Truncate(this string value, int maxChars)
        {
            if (value == null) return value;
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }

    }

    public static class Markdown
    {
        public static string MarkdownToText(this string markdown)
        {
            // separate superscript references from the text before them, so that here^1 turns into here ^1.
            // that way, ToPlanText below doesn't create a new word called here1, but rather creates "here 1"
            // after all the syntax has been stripped away.
            markdown = markdown.Replace("[", " [").Replace("<sup", " <sup");
            var doc = Markdig.Markdown.ToPlainText(markdown);
            return doc;
        }

        public static string FullSentencesOnly(this string allText)
        {
            var regex = new Regex(@"[a-z0-9A-Z][^\n]*[\.\!\?]\s*$", RegexOptions.Multiline);
            var allMatches = regex.Matches(allText).Select(m => m.Value).ToList();
            // remove lines with one word per line
            return String.Join("\n", allMatches);
        }
    }

    public static class Url
    {

        public static string HostName(this string url)
        {
            if (string.IsNullOrEmpty(url)) return null;
            else return new System.Uri(url).Host?.Replace("www.", "");
        }
    }


}
