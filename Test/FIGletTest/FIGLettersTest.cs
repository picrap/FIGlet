// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGletTest
{
    using System.IO;
    using FIGlet;
    using Fonts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FIGLettersTest
    {
        private Stream OpenRead(string name)
        {
            var s = GetType().Assembly.GetManifestResourceStream(typeof(FontsRoot), name + ".flf");
            return s;
        }

        private StreamReader OpenReader(string name)
        {
            return new StreamReader(OpenRead(name));
        }

        [TestMethod]
        public void LoadFont()
        {
            using (var r = OpenReader("small"))
            {
                var f = new FIGletters();
                f.Read(r);
            }
            // it doesn't fail and that's a good start
        }
    }
}