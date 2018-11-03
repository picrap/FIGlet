// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet
{
    using System;
    using System.Collections.Generic;
    using Blend;
    using Drawing;

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

        private CharacterSpacing _characterSpacing;

        /// <summary>
        /// Gets or sets the character spacing.
        /// </summary>
        /// <value>
        /// The character spacing.
        /// </value>
        public CharacterSpacing CharacterSpacing
        {
            get { return _characterSpacing; }
            set
            {
                switch (value)
                {
                    case CharacterSpacing.FullSize:
                        Blender = FullSizeBlender;
                        break;
                    case CharacterSpacing.Fitting:
                        Blender = FittingBlender;
                        break;
                    case CharacterSpacing.Smushing:
                        break;
                }
                _characterSpacing = value;
            }
        }

        private IDrawingElementBlender _blender;

        public IDrawingElementBlender Blender
        {
            get { return _blender; }
            set
            {
                _blender = value;
                _characterSpacing = CharacterSpacing.Custom;
            }
        }

        /// <summary>
        /// An actual hard blank, to be used for rendering
        /// </summary>
        public const char HardBlank = '\u00A0';

        /// <summary>
        /// Gets the fixed size blender.
        /// </summary>
        /// <value>
        /// The fixed size blender.
        /// </value>
        public static IDrawingElementBlender FullSizeBlender { get; }

        /// <summary>
        /// Gets the fitted blender.
        /// </summary>
        /// <value>
        /// The fitted blender.
        /// </value>
        public static IDrawingElementBlender FittingBlender { get; }

        static FIGdriver()
        {
            FullSizeBlender = DrawingElementBlender.WriteOnEmptyOnly;
            FittingBlender = DrawingElementBlender.WriteOnSpace.WithoutPriority();
        }

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
        public void Write(UnicodeChar c)
        {
            if (Font == null)
                throw new InvalidOperationException("A font is needed!");
            if (!Font.Characters.TryGetValue(c, out var character))
                return;
            AdjustDrawingBoardHeight();
            //AdjustPreDrawCaret(character, CharacterSpacing);
            var rowOffset = Baseline - Font.Baseline;
            var columnOffset = Caret;
            columnOffset = AdjustCaret(character, DrawingBoard, columnOffset, rowOffset, Blender);
            Draw(character, DrawingBoard, columnOffset, rowOffset, Blender);
            Caret = columnOffset + character.Width;
        }

        /// <summary>
        /// Writes one <see cref="char"/>.
        /// </summary>
        /// <param name="c">The c.</param>
        public void Write(char c)
        {
            Write((UnicodeChar)c);
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
        /// Draws the specified <see cref="FIGcharacter" /> to <see cref="DrawingBoard" />.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="drawingBoard">The drawing board.</param>
        /// <param name="columnOffset">The column offset.</param>
        /// <param name="rowOffset">The row offset.</param>
        /// <param name="blender">The blender.</param>
        private void Draw(FIGcharacter character, DrawingBoard drawingBoard, int columnOffset, int rowOffset, IDrawingElementBlender blender)
        {
            foreach (var element in GetCharacterDrawingElements(character))
            {
                var column = element.Column + columnOffset;
                var row = element.Row + rowOffset;
                drawingBoard[column, row] = blender.TryBlend(drawingBoard[column, row], element.DrawingElement);
            }
        }

        private int AdjustCaret(FIGcharacter character, DrawingBoard drawingBoard, int columnOffset, int rowOffset, IDrawingElementBlender blender)
        {
            for (; ; columnOffset--)
            {
                if (!CanDraw(character, drawingBoard, columnOffset - 1, rowOffset, blender))
                    return columnOffset;
            }
        }

        /// <summary>
        /// Determines whether this instance can draw the specified character.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="drawingBoard">The drawing board.</param>
        /// <param name="columnOffset">The column offset.</param>
        /// <param name="rowOffset">The row offset.</param>
        /// <param name="blender">The blender.</param>
        private bool CanDraw(FIGcharacter character, DrawingBoard drawingBoard, int columnOffset, int rowOffset, IDrawingElementBlender blender)
        {
            foreach (var element in GetCharacterDrawingElements(character))
            {
                var column = element.Column + columnOffset;
                var row = element.Row + rowOffset;
                if (column < 0 || row < 0)
                    return false;
                // this relies on the fact that element.DrawingElement is never null
                if (blender.TryBlend(drawingBoard[column, row], element.DrawingElement) is null)
                    return false;
            }

            return true;
        }

        private IEnumerable<CharacterDrawingElement> GetCharacterDrawingElements(FIGcharacter character)
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
                    yield return new CharacterDrawingElement(column, row, drawingElement);
                }
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
        /// <param name="glyph">The character.</param>
        /// <returns></returns>
        protected virtual DrawingElement CreateDrawingElement(FIGcharacter character, char glyph)
        {
            if (glyph == '\0')
                return null;
            return new DrawingElement(glyph);
        }
    }
}
