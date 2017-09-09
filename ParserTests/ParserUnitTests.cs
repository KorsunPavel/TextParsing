using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;
using System.Collections.Generic;
using FluentAssertions;
using System.Text;

namespace ParserTests {
    [TestClass]
    public class ParserUnitTests {
        [TestMethod]
        public void ShouldParseDashes() {
            string text = $"te\u2011st te\u2010st te-st -test -cat-cat-cat- are--results -- - test-";
            ParserClass parser = new ParserClass();
            Dictionary<string, int> wordsAmount = parser.ParseTest(text);
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
            ParserClass parser = new ParserClass();
            Dictionary<string, int> wordsAmount = parser.ParseTest(text);
            wordsAmount.Should().NotBeNull();
            wordsAmount.Should().NotBeEmpty();
            wordsAmount.Should().HaveCount(1);
            wordsAmount.Should().Contain("test", 3);
        }


        [TestMethod]
        public void ShouldParsePunctuation() {
            string text = "test, test: test! test? test.";
            ParserClass parser = new ParserClass();
            Dictionary<string, int> wordsAmount = parser.ParseTest(text);
            wordsAmount.Should().NotBeNull();
            wordsAmount.Should().NotBeEmpty();
            wordsAmount.Should().HaveCount(1);
            wordsAmount.Should().Contain("test", 5);
        }
        [TestMethod]
        public void ShouldParseLessThanThreeChars() {
            string text = "t te tes test tes te t";
            ParserClass parser = new ParserClass();
            Dictionary<string, int> wordsAmount = parser.ParseTest(text);
            wordsAmount.Should().NotBeNull();
            wordsAmount.Should().NotBeEmpty();
            wordsAmount.Should().HaveCount(2);
            wordsAmount.Should().Contain("tes", 2);
            wordsAmount.Should().Contain("test", 1);
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

            ParserClass parser = new ParserClass();
            Dictionary<string, int> wordsAmount = parser.ParseTest(text);
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

            ParserClass parser = new ParserClass();
            Dictionary<string, int> wordsAmount = parser.ParseTest(text);
            wordsAmount.Should().NotBeNull();
            wordsAmount.Should().NotBeEmpty();
            wordsAmount.Should().HaveCount(1);
            wordsAmount.Should().Contain("cat", 3);
            wordsAmount.Should().Contain("CAT", 3);
        }

    }
}
