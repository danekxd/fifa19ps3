using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceNetNp;

static class StreamExtensions
{
    public static bool TryReadUShort(this Stream stream, out ushort value)
    {
        Span<byte> buffer = stackalloc byte[2];
        if (!stream.ReadAll(buffer))
        {
            value = 0;
            return false;
        }
        return BinaryPrimitives.TryReadUInt16BigEndian(buffer, out value);
    }

    public static bool TryReadUInt(this Stream stream, out uint value)
    {
        Span<byte> buffer = stackalloc byte[4];
        if(!stream.ReadAll(buffer))
        {
            value = 0;
            return false;
        }

        return BinaryPrimitives.TryReadUInt32BigEndian(buffer, out value);
    }

    public static bool TryReadULong(this Stream stream, out ulong value)
    {
        Span<byte> buffer = stackalloc byte[8];
        if (!stream.ReadAll(buffer))
        {
            value = 0;
            return false;
        }
        return BinaryPrimitives.TryReadUInt64BigEndian(buffer, out value);
    }


    public static bool ReadAll(this Stream stream, Span<byte> buffer)
    {
        if (stream == null)
            return false;

        if (buffer.Length == 0)
            return true;

        int read;
        while ((read = stream.Read(buffer)) != 0)
        {
            if (read == buffer.Length)
                return true;

            if(read == 0)
                return false;

            buffer = buffer.Slice(read);
        }

        return false;
    }
}
