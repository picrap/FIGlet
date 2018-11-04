// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet.Utility
{
    using System.IO;

    /// <summary>
    /// Extensions to <see cref="Stream"/>
    /// </summary>
    public static class StreamExtensions
    {
        private static int MakeInt(int b0, int b1, int b2 = 0, int b3 = 0)
        {
            return (b3 << 24) + (b2 << 16) + (b1 << 8) + b0;
        }

        /// <summary>
        /// Reads a short.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static short ReadShort(this Stream stream)
        {
            var b = new byte[2];
            stream.Read(b, 0, b.Length);
            return (short)MakeInt(b[0], b[1]);
        }

        /// <summary>
        /// Reads an int.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static int ReadInt(this Stream stream)
        {
            var b = new byte[4];
            stream.Read(b, 0, b.Length);
            return MakeInt(b[0], b[1], b[2], b[3]);
        }

        /// <summary>
        /// Allocates and reads bytes.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static byte[] ReadBytes(this Stream stream, int length)
        {
            var b = new byte[length];
            stream.Read(b, 0, b.Length);
            return b;
        }

        /// <summary>
        /// Copies from source to destination with length limitS.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="length">The length.</param>
        public static void Copy(this Stream source, Stream destination, long length)
        {
            var buffer = new byte[10 << 10];
            while (length > 0)
            {
                var bytesRead = source.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                    break;
                destination.Write(buffer, 0, bytesRead);
                length -= bytesRead;
            }
        }
    }
}