// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet.Drawing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A grid of <see cref="DrawingElement"/>
    /// </summary>
    public class DrawingBoard
    {
        private readonly IList<IList<DrawingElement>> _board = new List<IList<DrawingElement>>();

        /// <summary>
        /// Gets or sets the <see cref="DrawingElement" /> with the specified position.
        /// </summary>
        /// <value>
        /// The <see cref="DrawingElement" />.
        /// </value>
        /// <param name="column">The column.</param>
        /// <param name="row">The row.</param>
        /// <returns></returns>
        public DrawingElement this[int column, int row]
        {
            get
            {
                // assuming the lines are always adjusted
                var line = _board[row];
                // however rows may not
                if (column < line.Count)
                    return line[column];
                return null;
            }
            set
            {
                // assuming the lines are always adjusted
                var line = _board[row];
                // however rows may not
                // (feel like you've read the same comments in the getter?)
                while (line.Count <= column)
                    line.Add(null);
                line[column] = value;
            }
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width => _board.Aggregate(0, (w, l) => Math.Max(w, l.Count));

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height => _board.Count;

        /// <summary>
        /// Inserts an empty line.
        /// </summary>
        /// <param name="rowIndex">Index of the line.</param>
        public void InsertEmptyRow(int rowIndex)
        {
            _board.Insert(rowIndex, new List<DrawingElement>());
        }

        /// <summary>
        /// Renders the board as a list of lines.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Render()
        {
            foreach (var line in _board)
            {
                var renderedLine = new StringBuilder();
                foreach (var drawingElement in line)
                {
                    if (drawingElement != null)
                        renderedLine.Append(drawingElement.Glyph);
                    else
                        renderedLine.Append(' ');
                }

                while (renderedLine.Length < Width)
                    renderedLine.Append(' ');

                yield return renderedLine.ToString();
            }
        }
    }
}
