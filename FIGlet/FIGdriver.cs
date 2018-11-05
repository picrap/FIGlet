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
        private readonly Func<FIGcharacter, char, DrawingElement> _createDrawingElement;

        /// <summary>
        /// Gets the drawing board.
        /// </summary>
        /// <value>
        /// The drawing board.
        /// </value>
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

        private FIGfont _font;

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        /// <value>
        /// The font.
        /// </value>
        public FIGfont Font
        {
            get { return _font; }
            set
            {
                _font = value;
                if (value != null)
                    LayoutRule = value.DefaultLayoutRule;
            }
        }

        private LayoutRule _layoutRule;

        /// <summary>
        /// Gets or sets the character spacing.
        /// </summary>
        /// <value>
        /// The character spacing.
        /// </value>
        public LayoutRule LayoutRule
        {
            get { return _layoutRule; }
            set
            {
                switch (value)
                {
                    case LayoutRule.FullSize:
                        Blender = FullSizeBlender;
                        break;
                    case LayoutRule.Fitting:
                        Blender = FittingBlender;
                        break;
                    case LayoutRule.Smushing:
                        Blender = SmushingBlender;
                        break;
                }
                _layoutRule = value;
            }
        }

        private IDrawingElementBlender _blender;

        /// <summary>
        /// Gets or sets the blender.
        /// </summary>
        /// <value>
        /// The blender.
        /// </value>
        public IDrawingElementBlender Blender
        {
            get { return _blender; }
            set
            {
                _blender = value;
                _layoutRule = LayoutRule.Custom;
            }
        }

        /// <summary>
        /// An actual hard blank, to be used for rendering
        /// </summary>
        public const char HardBlank = '\u00A0';

        /// <summary>
        /// Gets the full size blender.
        /// </summary>
        /// <value>
        /// The fixed size blender.
        /// </value>
        public static IDrawingElementBlender FullSizeBlender { get; }

        /// <summary>
        /// Gets the fitting blender.
        /// </summary>
        /// <value>
        /// The fitted blender.
        /// </value>
        public static IDrawingElementBlender FittingBlender { get; }

        /// <summary>
        /// Gets the smushing blender.
        /// </summary>
        /// <value>
        /// The fitted blender.
        /// </value>
        public static IDrawingElementBlender SmushingBlender { get; }

        private static FIGfont _defaultFont;

        /// <summary>
        /// Gets the default font.
        /// </summary>
        /// <value>
        /// The default font.
        /// </value>
        public static FIGfont DefaultFont
        {
            get
            {
                if (_defaultFont == null)
                    _defaultFont = FIGfontReference.Integrated[0].LoadFont();
                return _defaultFont;
            }
        }

        static FIGdriver()
        {
            FullSizeBlender = DrawingElementBlender.WriteOnEmptyOnly;
            FittingBlender = DrawingElementBlender.WriteOnSpace.WithoutPriority();
            SmushingBlender = FittingBlender
                .Or(DrawingElementBlender.EqualCharacterSmushing).Or(DrawingElementBlender.UnderscoreSmushing)
                .Or(DrawingElementBlender.HierarchySmushing).Or(DrawingElementBlender.OppositePairSmushing)
                .Or(DrawingElementBlender.BigXSmushing).Or(DrawingElementBlender.HardBlankSmushing);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FIGdriver"/> class.
        /// </summary>
        /// <param name="font">The font (optional, it may be added later).</param>
        /// <param name="createDrawingElement">It allows to create <see cref="DrawingElement"/> from external delegate.
        /// When this parameter is specified, the method <see cref="CreateDrawingElement"/> is not invoked.</param>
        public FIGdriver(FIGfont font = null, Func<FIGcharacter, char, DrawingElement> createDrawingElement = null)
        {
            _createDrawingElement = createDrawingElement;
            Font = font;
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
                drawingBoard[column, row] = blender.TryBlend(drawingBoard[column, row], element.DrawingElement) ?? element.DrawingElement;
            }
        }

        private int AdjustCaret(FIGcharacter character, DrawingBoard drawingBoard, int columnOffset, int rowOffset, IDrawingElementBlender blender)
        {
            // this is the poor man's fix (again)
            // character does not have a visual, so we ignore it
            if (character.Width == 0)
                return columnOffset;

            var previousHasNonBlanks = false;
            for (; ; columnOffset--)
            {
                if (!CanDraw(character, drawingBoard, columnOffset - 1, rowOffset, blender, out var hasNonBlanks) || previousHasNonBlanks)
                    return columnOffset;
                previousHasNonBlanks = hasNonBlanks;
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
        /// <param name="hasNonBlanks">if set to <c>true</c> [has non spaces].</param>
        /// <returns>
        ///   <c>true</c> if this instance can draw the specified character; otherwise, <c>false</c>.
        /// </returns>
        private bool CanDraw(FIGcharacter character, DrawingBoard drawingBoard, int columnOffset, int rowOffset, IDrawingElementBlender blender, out bool hasNonBlanks)
        {
            hasNonBlanks = false;
            foreach (var element in GetCharacterDrawingElements(character))
            {
                var column = element.Column + columnOffset;
                var row = element.Row + rowOffset;
                if (column < 0 || row < 0)
                    return false;
                // this relies on the fact that element.DrawingElement is never null
                var under = drawingBoard[column, row];
                if (blender.TryBlend(under, element.DrawingElement) is null)
                    return false;
                if (!(under is null) && !under.IsBlank && !element.DrawingElement.IsBlank)
                    hasNonBlanks = true;
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
                    var drawingElement = InvokeCreateDrawingElement(character, glyph);
                    if (drawingElement is null)
                        continue;
                    yield return new CharacterDrawingElement(column, row, drawingElement);
                }
        }

        private DrawingElement InvokeCreateDrawingElement(FIGcharacter character, char glyph)
        {
            if (!(_createDrawingElement is null))
                return _createDrawingElement(character, glyph);
            return CreateDrawingElement(glyph, character, this);
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
                DrawingBoard.InsertEmptyRow(0);
                Baseline++;
            }

            var below = Font.Height - Font.Baseline;
            while (Baseline + below > DrawingBoard.Height)
                DrawingBoard.InsertEmptyRow(DrawingBoard.Height - 1);
        }

        /// <summary>
        /// Creates a drawing element.
        /// This may be overriden in inherited classes, in order to add your own metadata.
        /// This method is not invoked if a delegate is given to <see cref="FIGdriver"/> constructor.
        /// </summary>
        /// <param name="glyph">The glyph to be displayed.</param>
        /// <param name="character">The related <see cref="FIGcharacter"/> that generated this glyph</param>
        /// <param name="driver">The related <see cref="FIGdriver"/> that generated this glyph</param>
        /// <returns></returns>
        protected virtual DrawingElement CreateDrawingElement(char glyph, FIGcharacter character, FIGdriver driver)
        {
            if (glyph == '\0')
                return null;
            return new DrawingElement(glyph);
        }
    }
}
