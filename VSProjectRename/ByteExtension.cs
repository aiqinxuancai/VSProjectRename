﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSProjectRename
{
    public static class ByteExtension
    {
        public static IEnumerable<byte> ByteReplace(IEnumerable<byte> source, params (byte[] search, byte[] replace)[] replacements)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (replacements == null)
                throw new ArgumentNullException(nameof(replacements));
            if (replacements.Any(r => r.search == null || r.search.Length == 0))
                throw new ArgumentOutOfRangeException(nameof(replacements), "Search parameter cannot be null or empty");
            if (replacements.Any(r => r.replace == null))
                throw new ArgumentOutOfRangeException(nameof(replacements), "Replace parameter cannot be null");

            var maxMatchSize = replacements.Select(r => r.search.Length).Max();
            var bufferSize = maxMatchSize * 2;
            var buffer = new byte[bufferSize];
            int bufferStart = 0;
            int bufferPosition = 0;

            byte[] nextBytes()
            {
                foreach ((byte[] search, byte[] replace) in replacements)
                {
                    if (ByteStartsWith(buffer, bufferStart, bufferPosition - bufferStart, search))
                    {
                        bufferStart += search.Length;
                        return replace;
                    }
                }

                var returnBytes = new byte[] { buffer[bufferStart] };
                bufferStart++;
                return returnBytes;
            }

            foreach (var dataByte in source)
            {
                buffer[bufferPosition] = dataByte;
                bufferPosition++;

                if (bufferPosition - bufferStart >= maxMatchSize)
                {
                    foreach (var resultByte in nextBytes())
                        yield return resultByte;
                }

                if (bufferPosition == bufferSize - 1)
                {
                    Buffer.BlockCopy(buffer, bufferStart, buffer, 0, bufferPosition - bufferStart);
                    bufferPosition -= bufferStart;
                    bufferStart = 0;
                }
            }

            while (bufferStart < bufferPosition)
            {
                foreach (var resultByte in nextBytes())
                    yield return resultByte;
            }
        }
        public static bool ByteStartsWith(byte[] data, int dataOffset, int dataLength, byte[] startsWith)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (startsWith == null)
                throw new ArgumentNullException(nameof(startsWith));

            if (dataLength < startsWith.Length)
                return false;

            for (int i = 0; i < startsWith.Length; i++)
            {
                if (data[i + dataOffset] != startsWith[i])
                    return false;
            }

            return true;
        }


    }
}
