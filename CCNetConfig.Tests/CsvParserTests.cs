using System;
using System.Collections.Generic;
using CCNetConfig.Core;
using NUnit.Framework;

namespace CCNetConfig.Tests
{
    [TestFixture]
    public class CsvParserTests
    {
        #region Single item on single line tests
        [Test]
        public void ParseSingleItemOnSingleLine()
        {
            var input = "item";
            var output = CsvParser.ParseData(input);
            var expected = new List<string[]>();
            expected.Add(new string[] { "item" });
            AssertData(output, expected);
        }

        [Test]
        public void ParseSingleQuotedItemOnSingleLine_SingleQuote()
        {
            var input = "'item'";
            var output = CsvParser.ParseData(input);
            var expected = new List<string[]>();
            expected.Add(new string[] { "item" });
            AssertData(output, expected);
        }

        [Test]
        public void ParseSingleQuotedItemOnSingleLine_DoubleQuote()
        {
            var input = "\"item\"";
            var output = CsvParser.ParseData(input);
            var expected = new List<string[]>();
            expected.Add(new string[] { "item" });
            AssertData(output, expected);
        }

        [Test]
        public void ParseSingleQuotedItemAndLiteralQuoteOnSingleLine_SingleQuote()
        {
            var input = "'item'''";
            var output = CsvParser.ParseData(input);
            var expected = new List<string[]>();
            expected.Add(new string[] { "item'" });
            AssertData(output, expected);
        }

        [Test]
        public void ParseSingleQuotedItemAndLiteralQuoteOnSingleLine_DoubleQuote()
        {
            var input = "\"item\"\"\"";
            var output = CsvParser.ParseData(input);
            var expected = new List<string[]>();
            expected.Add(new string[] { "item\"" });
            AssertData(output, expected);
        }
        #endregion

        #region Multiple items on single line tests
        [Test]
        public void ParseMultipleItemOnSingleLine()
        {
            var input = "item,item 1, item 2, \"test\" , 'test2' ,'test3'''";
            var output = CsvParser.ParseData(input);
            var expected = new List<string[]>();
            expected.Add(new string[] { "item", "item 1", " item 2", "test", "test2", "test3'" });
            AssertData(output, expected);
        }
        #endregion

        #region Multiple items on multiple line tests
        [Test]
        public void ParseMultipleItemOnMultipleLine()
        {
            var input = "item,item 1, item 2, \"test\" , 'test2' ,'test3'''" + Environment.NewLine + 
                "first,'second',\"third\"";
            var output = CsvParser.ParseData(input);
            var expected = new List<string[]>();
            expected.Add(new string[] { "item", "item 1", " item 2", "test", "test2", "test3'" });
            expected.Add(new string[] { "first", "second", "third" });
            AssertData(output, expected);
        }

        [Test]
        public void ParseMultipleItemOnMultipleLineWithItemAcrossLine()
        {
            var input = "item,item 1, item 2, \"test\" , 'test2' ,'test3'''" + Environment.NewLine +
                "first,'second',\"third\"" + Environment.NewLine +
                "\"split" + Environment.NewLine + "line\"";
            var output = CsvParser.ParseData(input);
            var expected = new List<string[]>();
            expected.Add(new string[] { "item", "item 1", " item 2", "test", "test2", "test3'" });
            expected.Add(new string[] { "first", "second", "third" });
            expected.Add(new string[] { "split" + Environment.NewLine + "line" });
            AssertData(output, expected);
        }
        #endregion

        private void AssertData(List<string[]> actualData, List<string[]> expectedData)
        {
            Assert.AreEqual(expectedData.Count, actualData.Count, "Number of lines doesn't match");
            for (var loop = 0; loop < expectedData.Count; loop++)
            {
                Assert.AreEqual(expectedData[loop].Length, actualData[loop].Length,
                    string.Format("Number of fields in line {0} doesn't match", loop));
                for (var fieldLoop = 0; fieldLoop < expectedData[loop].Length; fieldLoop++)
                {
                    Assert.AreEqual(expectedData[loop][fieldLoop], actualData[loop][fieldLoop],
                        string.Format("Value for field {1} in line {0} doesn't match", loop, fieldLoop));
                }
            }
        }
    }
}
