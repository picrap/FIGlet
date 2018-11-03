// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet
{
    using Drawing;

    internal  class CharacterDrawingElement
  {
      public readonly int Column;
      public readonly int Row;
      public readonly DrawingElement DrawingElement;

      public CharacterDrawingElement(int column, int row, DrawingElement drawingElement)
      {
          Column = column;
          Row = row;
          DrawingElement = drawingElement;
      }
  }
}