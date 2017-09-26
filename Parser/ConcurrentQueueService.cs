using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Parser {
    public class ConcurrentQueueService {
        static ConcurrentQueue<string> _queue;
        public static Dictionary<string, int> dic = new Dictionary<string, int>();

        public void ParseTextCh(char[] text) {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {

                if (Char.IsLetter(text[i]))
                {
                    sb.Append(text[i]);
                    if (i == text.Length - 1)
                    {
                        string res = GetWord(sb);
                        if (res.Length > 2) Enqueue(res);
                    }
                }
                else if (sb.Length == 0) continue;
                //else if (CheckForDashCh(i, text, sb)) continue;
                else {
                    string res = GetWord(sb);
                    if (res.Length > 2) Enqueue(res);
                }

            }
        }



        private bool CheckForDashCh(int i, char[] text, StringBuilder sb) {
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

        private static string GetWord(StringBuilder sb) {
            string shell = sb.ToString();
            sb.Clear();
            return shell;
        }

        static ConcurrentQueueService() {
            _queue = new ConcurrentQueue<string>();
        }

        public static void Enqueue(string word, CancellationToken cancelToken = default(CancellationToken)) {
            _queue.Enqueue(word);
        }

        public static void Dequeue() {
           // while (!_queue.IsEmpty)
            {
                try
                {
                    string word;
                    while (_queue.TryDequeue(out word)) { AddToDictionary(word); }
                }
                catch (NullReferenceException ex)
                {
                    string w = ex.Message;
                    Debug.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        private static void AddToDictionary(string word) {
            if (dic.ContainsKey(word))
            {
                dic[word]++;
            }
            else
            {
                dic.Add(word, 1);
            }
        }
    }
}
