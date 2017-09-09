using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser{
    public class ParserClass {
        public Dictionary<string, int> ParseTest(string text) {
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
                else
                {
                    if (CheckForDash(i, text, sb)) continue;
                    AddWordToDictioanary(sb, pairs);
                }
            }
            return pairs;
        }

        private bool CheckForDash(int i, string text, StringBuilder sb) {
            if (i > 0 && i < text.Length - 1 && IsDash(text[i]))
            {
                if (Char.IsLetter(text[i - 1]) && Char.IsLetter(text[i + 1]))
                {
                    sb.Append('-');
                    return true;
                }
            }
            return false;
        }

        private bool IsDash(char ch) {
            if (ch == '-' || ch == '\u2011' || ch == '\u2010') {
                return true;
            }
            return false;
        }

        private  void AddWordToDictioanary(StringBuilder sb, Dictionary<string, int> pairs) {

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
