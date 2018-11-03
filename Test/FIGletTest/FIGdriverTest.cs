// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGletTest
{
    using System;
    using System.IO;
    using FIGlet;
    using FIGlet.Blend;
    using Fonts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FIGdriverTest
    {
        [TestMethod]
        public void LoadFont()
        {
            var f = LoadSmallFIGfont();
            Assert.IsNotNull(f);
            // it doesn't fail and that's a good start
        }

        private static FIGfont LoadSmallFIGfont()
        {
            return FIGfont.FromEmbeddedResource("small.flf", typeof(FontsRoot));
        }

        private static string P(string s)
        {
            return s.Trim().Replace('\u00A0', ' ');
        }

        private static bool Matches(string a, string b)
        {
            var pa = P(a);
            var pb = P(b);
            var aLines = pa.Split(Environment.NewLine);
            var bLines = pb.Split(Environment.NewLine);
            if (aLines.Length != bLines.Length)
                return false;
            for (int index = 0; index < aLines.Length; index++)
            {
                var aLine = aLines[index].TrimEnd();
                var bLine = bLines[index].TrimEnd();
                if (aLine != bLine)
                    return false;
            }

            return true;
        }

        private static void CheckRenderAsSmall(string literal, string expectedRendering, IDrawingElementBlender blender)
        {
            var d = new FIGdriver { Font = LoadSmallFIGfont(), Blender = blender };
            d.Write(literal);
            var r = d.ToString();
            Assert.IsTrue(Matches(expectedRendering, r));
        }

        [TestMethod]
        public void FittingTest()
        {
            CheckRenderAsSmall("That one fits?", @"
  _____  _           _                       __  _  _        ___
 |_   _|| |_   __ _ | |_   ___  _ _   ___   / _|(_)| |_  ___|__ \
   | |  | ' \ / _` ||  _| / _ \| ' \ / -_) |  _|| ||  _|(_-<  /_/
   |_|  |_||_|\__,_| \__| \___/|_||_|\___| |_|  |_| \__|/__/ (_) "
                    , FIGdriver.FittingBlender);
        }

        [TestMethod]
        public void SmushingTest()
        {
            CheckRenderAsSmall("This smushes!", @"
  _____ _    _                      _           _
 |_   _| |_ (_)___  ____ __ _  _ __| |_  ___ __| |
   | | | ' \| (_-< (_-< '  \ || (_-< ' \/ -_|_-<_|
   |_| |_||_|_/__/ /__/_|_|_\_,_/__/_||_\___/__(_)"
                    , FIGdriver.SmushingBlender);
        }
    }
}