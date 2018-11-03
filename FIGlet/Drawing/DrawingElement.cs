// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet.Drawing
{
    public class DrawingElement
    {
        public char Glyph { get; }

        public bool IsBlank => Glyph == ' ' || Glyph == FIGdriver.HardBlank;

        public DrawingElement(char glyph)
        {
            Glyph = glyph;
        }

        public virtual DrawingElement ReplaceGlyph(char newGlyph)
        {
            return new DrawingElement(newGlyph);
        }
    }
}