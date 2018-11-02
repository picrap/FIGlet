// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FIGletTest
{
    using FIGlet;

    [TestClass]
    public class IntParserTest
    {
        [TestMethod]
        public void Parse1234()
        {
            Assert.AreEqual(1234, IntParser.TryParse("1234"));
        }

        [TestMethod]
        public void Parse010()
        {
            Assert.AreEqual(8, IntParser.TryParse("010"));
        }

        [TestMethod]
        public void Parse0xFF()
        {
            Assert.AreEqual(255, IntParser.TryParse("0xFF"));
        }

        [TestMethod]
        public void Parse0Xff()
        {
            Assert.AreEqual(255, IntParser.TryParse("0Xff"));
        }

        [TestMethod]
        public void ParseA()
        {
            Assert.AreEqual(null, IntParser.TryParse("A"));
        }

        [TestMethod]
        public void Parse09()
        {
            Assert.AreEqual(null, IntParser.TryParse("09"));
        }
    }
}