using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Parser {
   public  class Temp {

        static Regex _wordRegex = new Regex(@"\W+", RegexOptions.Compiled);

        public  void MainDo() {
            var words = File.ReadAllText(@"D:\text3.txt");
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            string[] c2 = words.Split(' ');
            var t = stopWatch.Elapsed.TotalSeconds;
            stopWatch.Restart();
            string[] c = _wordRegex.Split(words);
            stopWatch.Stop();
            var regexTime = stopWatch.Elapsed.TotalSeconds;
        }
    }
}
