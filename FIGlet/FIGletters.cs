// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet
{
    using System;
    using System.IO;

    public class FIGletters
    {
        public void Read(TextReader textReader)
        {
            if (!IsValid(textReader))
                throw new NotSupportedException("The format is unknown");
            var informations = textReader.ReadLine();
            var parts = informations.Split(' ');
            if(parts.Length!=9)
                throw new NotSupportedException("Invalid headers");
        }

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
