using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;
using System.Collections.Generic;
using FluentAssertions;
using System.Text;
using System.Diagnostics;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
using System.Text.RegularExpressions;
using System.Linq;

namespace ParserTests {
    [TestClass]
    public class ParserUnitTests {
        [TestMethod]
        public void ShouldParseDashes() {
            string text = $"te\u2011st te\u2010st te-st -test -cat-cat-cat- are\u2014results -- - test-";
            Parser.TextParser parser = new Parser.TextParser();
            Dictionary<string, int> wordsAmount = parser.ParseText(text);
            wordsAmount.Should().NotBeNull();
            wordsAmount.Should().NotBeEmpty();
            wordsAmount.Should().HaveCount(5);
            wordsAmount.Should().Contain("test", 2);
            wordsAmount.Should().Contain("te-st", 3);
            wordsAmount.Should().Contain("cat-cat-cat", 1);
            wordsAmount.Should().Contain("are", 1);
            wordsAmount.Should().Contain("results", 1);
        }

        [TestMethod]
        public void ShouldParseSpaces() {
            string text = " test test    test    ";
            Parser.TextParser parser = new Parser.TextParser();
            Dictionary<string, int> wordsAmount = parser.ParseText(text);
            wordsAmount.Should().NotBeNull();
            wordsAmount.Should().NotBeEmpty();
            wordsAmount.Should().HaveCount(1);
            wordsAmount.Should().Contain("test", 3);
        }


        [TestMethod]
        public void ShouldParsePunctuation() {
            string text = "test, test: test! test? test.";
            Parser.TextParser parser = new Parser.TextParser();
            Dictionary<string, int> wordsAmount = parser.ParseText(text);
            wordsAmount.Should().NotBeNull();
            wordsAmount.Should().NotBeEmpty();
            wordsAmount.Should().HaveCount(1);
            wordsAmount.Should().Contain("test", 5);
        }
        [TestMethod]
        public void ShouldParseLessThanThreeChars() {
            string text = "t te tes test tes te t";
            Parser.TextParser parser = new Parser.TextParser();
            Dictionary<string, int> wordsAmount = parser.ParseText(text);
            wordsAmount.Should().NotBeNull();
            wordsAmount.Should().NotBeEmpty();
            wordsAmount.Should().HaveCount(2);
            wordsAmount.Should().Contain("tes", 2);
            wordsAmount.Should().Contain("test", 1);
        }


        [TestMethod]
        public void ReadAndProcessFiles() {
            // Our thread-safe collection used for the handover.
            var lines = new BlockingCollection<string>();

            // Build the pipeline.
            var stage1 = Task.Run(() =>
            {
                try
                {
                    using (var reader = new StreamReader(@"D:\text.txt"))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            // Hand over to stage 2 and continue reading.
                            lines.Add(line);
                        }
                    }
                }
                finally
                {
                    lines.CompleteAdding();
                }
            });
            Dictionary<string, int> pairs = new Dictionary<string, int>();
            var stage2 = Task.Run(() =>
            {
                // Process lines on a ThreadPool thread
                // as soon as they become available.
                foreach (var line in lines.GetConsumingEnumerable())
                {
                    var words = line.Split(new char[] { '\n', '\r', '\u2013', '\u2014', ' ', ':', ',', '!', '?', '.' }, StringSplitOptions.RemoveEmptyEntries)
               .Where(s => s.Length > 2)
               .Select(s => s.Replace('\u2011', '-'))
               .Select(s => s.Replace('\u2010', '-'))
               .Select(s => s.Trim('-'));

                    foreach (String shell in words)
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
            });

            // Block until both tasks have completed.
            // This makes this method prone to deadlocking.
            // Consider using 'await Task.WhenAll' instead.
            Task.WaitAll(stage1, stage2);

        }
        [TestMethod]
        public void ShouldParseTextFile() {
            //Temp t = new Temp();
            //t.MainDo();


            //
            var words = File.ReadAllLines(@"D:\text.txt");
            StringBuilder sb = new StringBuilder();
            foreach (var word in words)
            {
                sb.Append(word).AppendLine();
            }
            TextParser parser = new TextParser();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Dictionary<string, int> wordsAmount = parser.ParseTextCh(sb.ToString().ToCharArray());
            stopWatch.Stop();
            double tsParalell = stopWatch.Elapsed.TotalSeconds;
        }

        [TestMethod]
        public void ShouldParseTextFileStream() {

            //
            ConcurrentDictionary<string, int> dictionary = new ConcurrentDictionary<string, int>();
            //
            int theshHoldInBytes = ConvertMegabyteToBytes(1);
            string path = @"D:\text.txt";
            long sizeOfFile = new FileInfo(path).Length;
            if (sizeOfFile > theshHoldInBytes)
            {

            }
            List<Task<Dictionary<string, int>>> list = new List<Task<Dictionary<string, int>>>();
            SplitParser parser = new SplitParser();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (StreamReader reader = new StreamReader(path))
            {
                while (reader.Peek() >= 0)
                {
                    char[] arr = new char[theshHoldInBytes];
                    reader.Read(arr, 0, theshHoldInBytes);
                    list.Add(Task.Run(() => AddToProducerQueue(arr)));
                }
            }
            var diacr = Task.WhenAll(list.ToArray()).Result;
            //int sum = 0;
            //foreach (var pair in diacr)
            //{
            //    foreach (var item in pair.Values)
            //    {
            //        sum += item;
            //    }
            //}

            //  Dictionary<string, int> wordsAmount = parser.ParseText(word);
            stopWatch.Stop();
            double tsParalell = stopWatch.Elapsed.TotalSeconds;
        }
        [TestMethod]
        public void ShouldParseTextFileStreamConcurrent2() {
            ConcurrentQueueService cs = new ConcurrentQueueService();
            var tokenSource2 = new CancellationTokenSource();
            CancellationToken ct = tokenSource2.Token;
            int theshHoldInBytes = ConvertMegabyteToBytes(1);
            string path = @"D:\text.txt";
            long sizeOfFile = new FileInfo(path).Length;
            if (sizeOfFile > theshHoldInBytes)
            {

            }
            List<Task> list = new List<Task>();
            SplitParser parser = new SplitParser();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            //Task.Run(() => cs.ParseTextCh(new char[] { 't','e','s','t',' ','m','y','o'}));
            using (StreamReader reader = new StreamReader(path))
            {
                while (reader.Peek() >= 0)
                {
                    char[] arr = new char[theshHoldInBytes];
                    reader.Read(arr, 0, theshHoldInBytes);
                    list.Add(Task.Run(() => cs.ParseTextCh(arr)));
                }
            }
            var t = Task.Run(() => ConcurrentQueueService.Dequeue(), tokenSource2.Token);
            Task.WaitAll(list.ToArray());
            t.Wait();
            //tokenSource2.Cancel();
            var ss = ConcurrentQueueService.dic;

        }

        [TestMethod]
        public void ShouldParseTextFileStreamConcurrent() {

            ConcurrentDictionary<string, int> dictionary = new ConcurrentDictionary<string, int>();
            int theshHoldInBytes = ConvertMegabyteToBytes(1);
            string path = @"D:\text.txt";
            long sizeOfFile = new FileInfo(path).Length;
            if (sizeOfFile > theshHoldInBytes)
            {

            }
            List<Task<ConcurrentDictionary<string, int>>> list = new List<Task<ConcurrentDictionary<string, int>>>();
            SplitParser parser = new SplitParser();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (StreamReader reader = new StreamReader(path))
            {
                while (reader.Peek() >= 0)
                {
                    char[] arr = new char[theshHoldInBytes];
                    reader.Read(arr, 0, theshHoldInBytes);
                    list.Add(Task.Run(async () => await AddToConcurrentDictioanary(dictionary, arr)));
                }
            }
            var diacr = Task.WhenAll(list.ToArray()).Result;
            //int sum = 0;
            //foreach (var pair in diacr)
            //{
            //    foreach (var item in pair.Values)
            //    {
            //        sum += item;
            //    }
            //}

            //  Dictionary<string, int> wordsAmount = parser.ParseText(word);
            stopWatch.Stop();
            double tsParalell = stopWatch.Elapsed.TotalSeconds;
        }

        private async Task<ConcurrentDictionary<string, int>> AddToConcurrentDictioanary(ConcurrentDictionary<string, int> pairs, char[] arr) {
            TextParser parser = new TextParser();
            return await Task.Factory.StartNew(() => parser.ParseTextParallel(pairs, arr));
        }

        private async Task<Dictionary<string, int>> AddToProducerQueue(char[] arr) {
            TextParser parser = new TextParser();
            return await Task.Factory.StartNew(() => parser.ParseTextCh(arr));
        }


        private int ConvertMegabyteToBytes(int megaBites) {
            return megaBites * 1024 * 1024;
        }

        [TestMethod]
        public void ShouldParseNewLines() {
            string[] items = { "Cat", "Dog", "Cat" };
            StringBuilder builder = new StringBuilder(
                "Required:").AppendLine();
            foreach (string item in items)
            {
                builder.Append(item).AppendLine();
            }
            string text = builder.ToString();

            TextParser parser = new Parser.TextParser();
            Dictionary<string, int> wordsAmount = parser.ParseText(text);
            wordsAmount.Should().NotBeNull();
            wordsAmount.Should().NotBeEmpty();
            wordsAmount.Should().HaveCount(3);
            wordsAmount.Should().Contain("Required", 1);
            wordsAmount.Should().Contain("Cat", 2);
            wordsAmount.Should().Contain("Dog", 1);
        }

        [TestMethod]
        public void ShouldIgnoreCase() {
            string text = "cat cAt CAT";

            Parser.TextParser parser = new Parser.TextParser();
            Dictionary<string, int> wordsAmount = parser.ParseText(text);
            wordsAmount.Should().NotBeNull();
            wordsAmount.Should().NotBeEmpty();
            wordsAmount.Should().HaveCount(1);
            wordsAmount.Should().Contain("cat", 3);
            wordsAmount.Should().Contain("CAT", 3);
        }

    }
}
