// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a character to be drawn
    /// </summary>
    public class FIGcharacter
    {
        /// <summary>
        /// The Unicode code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public UnicodeChar Code { get; }

        /// <summary>
        /// Gets the rows.
        /// </summary>
        /// <value>
        /// The lines.
        /// </value>
        public IList<string> Rows { get; }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width { get; }
        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height => Rows.Count;

        /// <summary>
        /// Gets the glyph at specified coordinates.
        /// </summary>
        /// <value>
        /// The <see cref="System.Char"/>.
        /// </value>
        /// <param name="column">The x.</param>
        /// <param name="row">The y.</param>
        /// <returns></returns>
        public char this[int column, int row]
        {
            get
            {
                var rowData = Rows[row];
                if (column < rowData.Length)
                    return rowData[column];
                return '\0';
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FIGcharacter"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="rows">The rows.</param>
        public FIGcharacter(UnicodeChar code, IList<string> rows)
        {
            Code = code;
            Rows = rows;
            Width = rows.Aggregate(0, (w, l) => Math.Max(w, l.Length));
        }
    }
}