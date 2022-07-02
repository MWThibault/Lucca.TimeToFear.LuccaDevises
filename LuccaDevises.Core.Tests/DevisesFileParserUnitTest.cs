using LuccaDevises.Core.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace LuccaDevises.Core.Tests
{
    [TestClass]
    public class DevisesFileParserUnitTest
    {
        [TestMethod]
        public void TestFileParser()
        {
            DevisesFileParser parser = new DevisesFileParser();
            parser.Parse("datas.txt");
            Assert.AreEqual(parser.DeviseStart, "EUR");
            Assert.AreEqual(parser.DeviseStop, "JPY");
            Assert.AreEqual(parser.DeviseConvertValue, 550);
            Assert.AreEqual(parser.DevisesRepo?.Count, 6);
        }
    }
}
