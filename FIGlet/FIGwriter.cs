// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet
{
    using System;

    public class FIGwriter
    {
        public DrawingBoard DrawingBoard { get; }

        public int Caret { get; set; }

        public int Baseline { get; set; }

        public FIGfont Font { get; set; }

        public CharacterSpacing CharacterSpacing { get; set; }

        public FIGwriter()
        {
            DrawingBoard = new DrawingBoard();
        }

        public void Write(char c)
        {
            if (!Font.Characters.TryGetValue(c, out var character))
                return;
            AdjustDrawingBoardHeight();
            AdjustPreDrawCaret(character, CharacterSpacing);
            Draw(character, DrawingBoard);
            Caret += character.Width;
        }

        public void Write(string s)
        {
            foreach (var c in s)
                Write(c);
        }

        private void Draw(FIGcharacter character, DrawingBoard drawingBoard)
        {
            for (int row = 0; row < character.Height; row++)
                for (int column = 0; column < character.Width; column++)
                {
                    var glyph = character[column, row];
                    var drawingElement = CreateDrawingElement(glyph);
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

        private void AdjustCaretFitting(FIGcharacter c)
        {
        }

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

        protected virtual DrawingElement CreateDrawingElement(char character)
        {
            if (character == '\0' || character == ' ')
                return null;
            return new DrawingElement(character);
        }
    }
}