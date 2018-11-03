// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet
{
    using System;

    /// <summary>
    /// The main instance.
    /// Please note that it requires to be initialized with a <see cref="FIGfont"/>
    /// </summary>
    public class FIGdriver
    {
        public DrawingBoard DrawingBoard { get; } = new DrawingBoard();

        /// <summary>
        /// Gets or sets the caret (the place at which the next character is going to be inserted.
        /// </summary>
        /// <value>
        /// The caret.
        /// </value>
        public int Caret { get; set; }

        /// <summary>
        /// Gets or sets the baseline.
        /// </summary>
        /// <value>
        /// The baseline.
        /// </value>
        public int Baseline { get; set; }

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        /// <value>
        /// The font.
        /// </value>
        public FIGfont Font { get; set; }

        /// <summary>
        /// Gets or sets the character spacing.
        /// </summary>
        /// <value>
        /// The character spacing.
        /// </value>
        public CharacterSpacing CharacterSpacing { get; set; } = CharacterSpacing.FullSize;

        /// <summary>
        /// An actual hard blank, to be used for rendering
        /// </summary>
        public const char HardBlank = '\u00A0';

        /// <summary>
        /// Returns a simple render (which is enough for most of us)
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Join(Environment.NewLine, DrawingBoard.Render());
        }

        /// <summary>
        /// Writes the specified char.
        /// This is the base for rendering
        /// </summary>
        /// <param name="c">The c.</param>
        /// <exception cref="System.InvalidOperationException">A font is needed!</exception>
        public void Write(char c)
        {
            if (Font == null)
                throw new InvalidOperationException("A font is needed!");
            if (!Font.Characters.TryGetValue(c, out var character))
                return;
            AdjustDrawingBoardHeight();
            AdjustPreDrawCaret(character, CharacterSpacing);
            Draw(character, DrawingBoard);
            Caret += character.Width;
        }

        /// <summary>
        /// Writes a string.
        /// </summary>
        /// <param name="s">The s.</param>
        public void Write(string s)
        {
            foreach (var c in s)
                Write(c);
        }

        /// <summary>
        /// Draws the specified <see cref="FIGcharacter"/> to <see cref="DrawingBoard"/>.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="drawingBoard">The drawing board.</param>
        private void Draw(FIGcharacter character, DrawingBoard drawingBoard)
        {
            for (int row = 0; row < character.Height; row++)
                for (int column = 0; column < character.Width; column++)
                {
                    var glyph = character[column, row];
                    if (glyph == Font.HardBlank)
                        glyph = HardBlank;
                    var drawingElement = CreateDrawingElement(character, glyph);
                    if (drawingElement is null)
                        continue;
                    drawingBoard[Caret + column, row + Baseline - Font.Baseline] = drawingElement;
                }
        }

        private void AdjustPreDrawCaret(FIGcharacter character, CharacterSpacing characterSpacing)
        {
            switch (characterSpacing)
            {
                case CharacterSpacing.FullSize:
                    return;
                case CharacterSpacing.Fitting:
                    AdjustCaretFitting(character);
                    break;
                case CharacterSpacing.Smushing:
                    throw new NotImplementedException();
            }
        }

        private void AdjustCaretFitting(FIGcharacter character)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adjusts the height of the drawing board.
        /// Checks that there are enough lines to render
        /// </summary>
        private void AdjustDrawingBoardHeight()
        {
            var above = Font.Baseline;
            while (Baseline < above)
            {
                DrawingBoard.InsertLine(0);
                Baseline++;
            }

            var below = Font.Height - Font.Baseline;
            while (Baseline + below > DrawingBoard.Height)
                DrawingBoard.InsertLine(DrawingBoard.Height - 1);
        }

        /// <summary>
        /// Creates a drawing element.
        /// This may be overriden in inherited classes, in order to add your own metadata
        /// </summary>
        /// <param name="character"></param>
        /// <param name="c">The character.</param>
        /// <returns></returns>
        protected virtual DrawingElement CreateDrawingElement(FIGcharacter character, char c)
        {
            if (c == '\0' || c == ' ')
                return null;
            return new DrawingElement(c);
        }
    }
}
