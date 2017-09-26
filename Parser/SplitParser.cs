using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser {
    public class SplitParser {
        public Dictionary<string, int> ParseText(string text) {
            var words = text
                .Split(new char[] { '\n', '\r', '\u2013', '\u2014', ' ', ':', ',', '!', '?', '.' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(s => s.Length > 2)
                .Select(s => s.Replace('\u2011', '-'))
                .Select(s => s.Replace('\u2010', '-'))
                .Select(s => s.Trim('-'));
            Dictionary<string, int> pairs = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var word in words)
            {
                if (pairs.ContainsKey(word))
                {
                    pairs[word]++;
                }
                else
                {
                    pairs.Add(word, 1);
                }
            }
            return pairs;
        }
        private bool CheckForDash(int i, string text, StringBuilder sb) {
            if (i > 0 && i < text.Length - 1 && text[i].IsDash())
            {
                if (Char.IsLetter(text[i - 1]) && Char.IsLetter(text[i + 1]))
                {
                    sb.Append('-');
                    return true;
                }
            }
            return false;
        }
        private void AddWordToDictioanary(StringBuilder sb, Dictionary<string, int> pairs) {
            string shell = sb.ToString();
            sb.Clear();
            if (shell.Length > 2)
            {
                if (pairs.ContainsKey(shell))
                {
                    pairs[shell]++;
                }
                else
                {
                    pairs.Add(shell, 1);
                }
            }
        }
    }
}
