using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser;
using System.Collections.Concurrent;

namespace Parser {
    public class TextParser {

        public ConcurrentDictionary<string, int> ParseTextParallel(ConcurrentDictionary<string,int> pairs, Char[] text) {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                if (Char.IsLetter(text[i]))
                {
                    sb.Append(text[i]);
                    if (i == text.Length - 1)
                    {
                        AddWordToDictioanaryConcurrent(sb, pairs);
                    }
                }
                else if (sb.Length == 0) continue;
                else if (CheckForDashCh(i, text, sb)) continue;
                else AddWordToDictioanaryConcurrent(sb, pairs);
            }
            return pairs;
        }

        private void AddWordToDictioanaryConcurrent(StringBuilder sb, ConcurrentDictionary<string, int> pairs) {
            string shell = sb.ToString();
            sb.Clear();
            if (shell.Length > 2)
            {
                pairs.AddOrUpdate(shell, 1, (word, amount) => amount++);
            }
        }

        public Dictionary<string, int> ParseText(String text) {
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
                else if (CheckForDash(i, text, sb)) continue;
                else AddWordToDictioanary(sb, pairs);
            }
            return pairs;
        }
        public Dictionary<string, int> ParseTextCh(char [] text) {
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
                else if (CheckForDashCh(i, text, sb)) continue;
                else AddWordToDictioanary(sb, pairs);
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
        private bool CheckForDashCh(int i, char [] text, StringBuilder sb) {
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
