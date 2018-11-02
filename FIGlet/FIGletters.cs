// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Represents the font
    /// </summary>
    public class FIGletters
    {
        public char HardBlank { get; private set; }
        public int Height { get; private set; }
        public int Baseline { get; private set; }
        public int MaxLength { get; private set; }
        public int OldLayout { get; private set; }
        public int CommentLinesCount { get; private set; }

        public int? PrintDirection { get; private set; }
        public int? FullLayout { get; private set; }
        public int? CodetagCount { get; private set; }

        public IList<string> Comment { get; private set; }

        public IDictionary<char, FIGLetter> Letters { get; private set; }

        /// <summary>
        /// Reads from  the specified <see cref="TextReader"/>.
        /// </summary>
        /// <param name="textReader">The text reader.</param>
        /// <exception cref="NotSupportedException">
        /// The format is unknown
        /// or
        /// Invalid headers
        /// </exception>
        public void Read(TextReader textReader)
        {
            if (!IsValid(textReader))
                throw new NotSupportedException("The format is unknown");
            var informations = textReader.ReadLine().TrimEnd();
            var informationParts = informations.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            // 6 parts are mandatory, 3 are optional
            switch (informationParts.Length)
            {
                case 6:
                    ReadMandatoryInformation(informationParts);
                    break;
                case 9:
                    ReadMandatoryInformation(informationParts);
                    ReadOptionalInformation(informationParts);
                    break;
                default:
                    throw new NotSupportedException("Invalid headers");
            }

            // comment is kept (not sure this helps)
            var comment = new List<string>();
            for (int commentLineIndex = 0; commentLineIndex < CommentLinesCount; commentLineIndex++)
                comment.Add(textReader.ReadLine());
            Comment = comment.AsReadOnly();

            // -- now load letters
            Letters = new Dictionary<char, FIGLetter>();
            // all ASCII letters
            for (int i = 32; i <= 126; i++)
                AddLetter(ReadLetter((char)i, textReader));
            // then some extended (but implied) extra letters
            foreach (var s in new[] { 196, 214, 220, 228, 246, 252, 223 })
                AddLetter(ReadLetter((char)s, textReader));

            // then, it's free bar. Any letter can be added
            for (; ; )
            {
                var code = ReadLetterCode(textReader, out var description);
                if (!code.HasValue)
                    break;
                AddLetter(ReadLetter(code.Value, textReader));
            }
        }

        private void AddLetter(FIGLetter letter)
        {
            Letters[letter.Code] = letter;
        }

        /// <summary>
        /// Reads the letter code.
        /// </summary>
        /// <param name="textReader">The text reader.</param>
        /// <param name="description">The char description (if any).</param>
        /// <returns>A <see cref="char"/> representing the code</returns>
        private char? ReadLetterCode(TextReader textReader, out string description)
        {
            var line = textReader.ReadLine();
            if (line == null)
            {
                description = null;
                return null;
            }

            var splitIndex = line.IndexOf(' ');
            if (splitIndex == -1)
            {
                description = null;
                return (char)Parse(line);
            }

            var literalCode = line.Substring(0, splitIndex);
            description = line.Substring(splitIndex + 1).Trim();
            return (char)Parse(literalCode);
        }

        /// <summary>
        /// Reads the letter.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="textReader">The text reader.</param>
        /// <returns></returns>
        private FIGLetter ReadLetter(char code, TextReader textReader)
        {
            var letterCharacters = new List<string>();
            // we guess the trailing character. Maybe it's always @, but I'm not sure now.
            char? trailingCharacter = null;
            for (; ; )
            {
                var letterLine = textReader.ReadLine();
                if (letterLine == null)
                    return null;
                if (!trailingCharacter.HasValue)
                    trailingCharacter = letterLine.Last();
                var trailerCharactersCount = CountTrailing(letterLine, trailingCharacter.Value);
                var letterLineCharacters = letterLine.Substring(0, letterLine.Length - trailerCharactersCount);
                letterCharacters.Add(letterLineCharacters);
                if (trailerCharactersCount == 2)
                    break;
            }
            return new FIGLetter(code, letterCharacters);
        }

        /// <summary>
        /// Counts the trailing character.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        private int CountTrailing(string s, char c)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[s.Length - i - 1] != c)
                    return i;
            }

            return s.Length;
        }

        /// <summary>
        /// Counts the heading character.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        private int CountHeading(string s, char c)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] != c)
                    return i;
            }

            return s.Length;
        }

        private void ReadMandatoryInformation(string[] informationParts)
        {
            HardBlank = informationParts[0][0];
            Height = Parse(informationParts[1]);
            Baseline = Parse(informationParts[2]);
            MaxLength = Parse(informationParts[3]);
            OldLayout = Parse(informationParts[4]);
            CommentLinesCount = Parse(informationParts[5]);
        }

        private void ReadOptionalInformation(string[] informationsParts)
        {
            PrintDirection = Parse(informationsParts[6]);
            FullLayout = Parse(informationsParts[7]);
            CodetagCount = Parse(informationsParts[8]);
        }

        /// <summary>
        /// Parses the specified literal. Throws exceptions when angry
        /// </summary>
        /// <param name="literal">The literal.</param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        private int Parse(string literal)
        {
            var v = IntParser.TryParse(literal);
            if (!v.HasValue)
                throw new FormatException();
            return v.Value;
        }

        /// <summary>
        /// Indicates whether the file is valid.
        /// </summary>
        /// <param name="textReader">The text reader.</param>
        /// <returns>
        ///   <c>true</c> if the specified text reader is valid; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsValid(TextReader textReader)
        {
            var signatureBuffer = new char[5];
            textReader.Read(signatureBuffer, 0, signatureBuffer.Length);
            var signature = new string(signatureBuffer);
            var b = signature == "flf2a";
            return b;
        }
    }
}
