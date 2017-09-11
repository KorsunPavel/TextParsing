using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser;

namespace Parser {
    public class StringBuilderParser {
        public Dictionary<string, int> ParseWithStringBuilder(StringBuilder text) {
            StringBuilder sb = new StringBuilder();
            Dictionary<string, int> pairs = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
            for (int i = 0; i < text.Length; i++)
            {
                if (Char.IsLetter(text[i]))
                {
                    sb.Append(text[i]);
                    if (i == text.Length - 1)
                    {
                        AddWordToDictioanary(sb, pairs);
                    }
                }
                else if (sb.Length == 0) continue;
                else if (CheckForDashWithStringBuilder(i, text, sb)) continue;
                else AddWordToDictioanary(sb, pairs);
            }
            return pairs;
        }

        private bool CheckForDashWithStringBuilder(int i, StringBuilder text, StringBuilder sb) {
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
