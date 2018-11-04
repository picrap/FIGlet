// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet.Utility
{
    /// <summary>
    /// Extensions to <see cref="FIGfont"/>
    /// </summary>
    public static class FIGfontExtensions
    {
        /// <summary>
        /// Renders the specified text using the given <see cref="FIGfont"/>.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string Render(this FIGfont font, string text)
        {
            var driver = new FIGdriver(font);
            driver.Write(text);
            return driver.ToString();
        }
    }
}
