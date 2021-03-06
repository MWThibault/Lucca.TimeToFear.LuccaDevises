using LuccaDevises.Core.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace LuccaDevises.Core.Tests
{
    [TestClass]
    public class DevisesConverterUnitTest
    {
        [TestMethod]
        public void TestConverter()
        {
            List<DevisesConvertItem> devises = new List<DevisesConvertItem>();
            devises.Add(new DevisesConvertItem { DeviseFrom = "AUD", DeviseTo = "CHF", ValueConvertion = 0.9661 });
            devises.Add(new DevisesConvertItem { DeviseFrom = "JPY", DeviseTo = "KWU", ValueConvertion = 13.1151 });
            devises.Add(new DevisesConvertItem { DeviseFrom = "EUR", DeviseTo = "CHF", ValueConvertion = 1.2053 });
            devises.Add(new DevisesConvertItem { DeviseFrom = "AUD", DeviseTo = "JPY", ValueConvertion = 86.0305 });
            devises.Add(new DevisesConvertItem { DeviseFrom = "EUR", DeviseTo = "USD", ValueConvertion = 1.2989 });
            devises.Add(new DevisesConvertItem { DeviseFrom = "JPY", DeviseTo = "INR", ValueConvertion = 0.6571 });


            DevisesConverter devisesConverter = new DevisesConverter(devises);
            Assert.AreEqual(devisesConverter.Convert("EUR", "JPY", 550), 59033);
        }
    }
}
