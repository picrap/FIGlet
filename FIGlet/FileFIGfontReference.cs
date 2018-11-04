// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet
{
    /// <summary>
    ///     Implementation of <see cref="FIGfontReference" /> with ability to load from file
    /// </summary>
    /// <seealso cref="FIGlet.FIGfontReference" />
    public class FileFIGfontReference : FIGfontReference
    {
        private readonly string _path;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FileFIGfontReference" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="name">The name.</param>
        public FileFIGfontReference(string path, string name)
            : base(name)
        {
            _path = path;
        }

        /// <summary>
        ///     Loads the font.
        /// </summary>
        /// <returns></returns>
        public override FIGfont LoadFont()
        {
            return FIGfont.FromFile(_path);
        }
    }
}