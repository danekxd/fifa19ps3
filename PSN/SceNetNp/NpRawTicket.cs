using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceNetNp;

public class NpRawTicket
{
    private byte[] bytes;

    public ReadOnlySpan<byte> TicketBytes => bytes;
    public NpTicketVersion TicketVersion { get; }
    public NpTicketSection Root { get; }
    public NpTicketSection Body { get; }
    public NpTicketSection Footer { get; }

    private NpRawTicket(byte[] ticketBytes, NpTicketVersion ticketVersion, NpTicketSection section)
    {
        bytes = ticketBytes;
        TicketVersion = ticketVersion;
        Root = section;

        NpTicketSection? body = null;
        foreach (var value in section)
        {
            if (value is not NpTicketFieldValue.Body bodyValue)
                continue;
            body = bodyValue.Value;
            break;
        }
        Body = body ?? new NpTicketSection();

        NpTicketSection? footer = null;
        foreach (var value in section)
        {
            if (value is not NpTicketFieldValue.Footer footerValue)
                continue;
            footer = footerValue.Value;
            break;
        }
        Footer = footer ?? new NpTicketSection();
    }

    public static bool TryParse(byte[] ticketBytes, [NotNullWhen(true)] out NpRawTicket? ticket)
    {
        ticket = null;

        if (ticketBytes == null || ticketBytes.Length < 8)
            return false;
        
        using MemoryStream stream = new MemoryStream(ticketBytes);

        if(!stream.TryReadUInt(out uint ver))
            return false;

        NpTicketVersion ticketVersion = ToTicketVersionEnum(ver);
        if (ticketVersion == NpTicketVersion.Unknown)
            return false;

        // always expecting it to be 0
        if (!stream.TryReadUShort(out ushort unk1) || unk1 != 0)
            return false;

        uint ticketSize;

        if(ticketVersion == NpTicketVersion.Version40)
        {
            if (!stream.TryReadUInt(out ticketSize))
                return false;
        }
        else
        {
            if (!stream.TryReadUShort(out ushort size))
                return false;

            ticketSize = size;
        }

        if(!TryReadTicketSection(stream, ticketVersion, ticketSize, out NpTicketSection? section))
            return false;

        byte[] ticketCopy = new byte[ticketBytes.Length];
        Array.Copy(ticketBytes, ticketCopy, ticketBytes.Length);

        ticket = new NpRawTicket(ticketCopy, ticketVersion, section);
        return true;
    }


    private static NpTicketVersion ToTicketVersionEnum(uint ticketVersion)
    {
        const uint VER_2_0 = 0x21000000;
        const uint VER_2_1 = 0x21010000;
        const uint VER_3_0 = 0x31000000;
        const uint VER_4_0 = 0x41000000;

        switch (ticketVersion)
        {
            case VER_2_0:
                return NpTicketVersion.Version20;
            case VER_2_1:
                return NpTicketVersion.Version21;
            case VER_3_0:
                return NpTicketVersion.Version30;
            case VER_4_0:
                return NpTicketVersion.Version40;
            default:
                return NpTicketVersion.Unknown;
        }
    }

    private static bool TryReadTicketSection(Stream stream, NpTicketVersion ticketVersion, uint ticketSize, [NotNullWhen(true)] out NpTicketSection? section)
    {
        section = null;
        long sectionEnd = stream.Position + ticketSize;

        if(sectionEnd > stream.Length)
        {
            section = null;
            return false;
        }

        List<NpTicketFieldValue> fields = new List<NpTicketFieldValue>();
        while(stream.Position < sectionEnd)
        {
            if(!TryReadField(stream, ticketVersion, out NpTicketFieldValue? value))
                return false;

            if (stream.Position > sectionEnd)
                return false;

            fields.Add(value);
        }

        section = new NpTicketSection(fields);
        return true;
    }

    private static bool TryReadField(Stream stream, NpTicketVersion ticketVersion, [NotNullWhen(true)] out NpTicketFieldValue? value)
    {
        value = null;

        int fieldPosition = (int)stream.Position;

        if (!stream.TryReadUShort(out ushort type))
            return false;

        NpTicketFieldValue.Type fieldType = (NpTicketFieldValue.Type)type;

        if (!stream.TryReadUShort(out ushort fieldSize))
            return false;

        int valuePosition = (int)stream.Position;

        switch (fieldType)
        {
            case NpTicketFieldValue.Type.Empty:
                {
                    if (fieldSize != 0)
                        return false;

                    value = new NpTicketFieldValue.Empty
                    {
                        FieldPosition = fieldPosition,
                        ValuePosition = valuePosition,
                        EndPosition = (int)stream.Position
                    };
                    return true;
                }
            case NpTicketFieldValue.Type.UInt:
                {
                    if(fieldSize != sizeof(uint))
                        return false;

                    Span<byte> buffer = stackalloc byte[sizeof(uint)];
                    if (!stream.ReadAll(buffer))
                        return false;

                    if (!BinaryPrimitives.TryReadUInt32BigEndian(buffer, out uint uintValue))
                        return false;

                    value = new NpTicketFieldValue.UInt
                    {
                        FieldPosition = fieldPosition,
                        ValuePosition = valuePosition,
                        EndPosition = (int)stream.Position,
                        Value = uintValue
                    };

                    return true;
                }
            case NpTicketFieldValue.Type.ULong:
                {
                    if (fieldSize != sizeof(ulong))
                        return false;

                    Span<byte> buffer = stackalloc byte[sizeof(ulong)];
                    if (!stream.ReadAll(buffer))
                        return false;

                    if (!BinaryPrimitives.TryReadUInt64BigEndian(buffer, out ulong ulongValue))
                        return false;

                    value = new NpTicketFieldValue.ULong
                    {
                        FieldPosition = fieldPosition,
                        ValuePosition = valuePosition,
                        EndPosition = (int)stream.Position,
                        Value = ulongValue
                    };

                    return true;
                }
            case NpTicketFieldValue.Type.String:
                {
                    Span<byte> buffer = fieldSize <= 512 ? stackalloc byte[fieldSize] : new byte[fieldSize];
                    if (!stream.ReadAll(buffer))
                        return false;

                    int inx = buffer.IndexOf<byte>(0);
                    if (inx >= 0)
                        buffer = buffer.Slice(0, inx);

                    string strValue = Encoding.ASCII.GetString(buffer);

                    value = new NpTicketFieldValue.String
                    {
                        FieldPosition = fieldPosition,
                        ValuePosition = valuePosition,
                        EndPosition = (int)stream.Position,
                        Value = strValue
                    };

                    return true;
                }
            case NpTicketFieldValue.Type.Time:
                {
                    if (fieldSize != sizeof(ulong))
                        return false;

                    Span<byte> buffer = stackalloc byte[sizeof(ulong)];
                    if (!stream.ReadAll(buffer))
                        return false;

                    if (!BinaryPrimitives.TryReadUInt64BigEndian(buffer, out ulong ulongValue))
                        return false;

                    DateTime timeValue = DateTimeOffset.FromUnixTimeMilliseconds((long)ulongValue).UtcDateTime;

                    value = new NpTicketFieldValue.Time
                    {
                        FieldPosition = fieldPosition,
                        ValuePosition = valuePosition,
                        EndPosition = (int)stream.Position,
                        Value = timeValue
                    };

                    return true;
                }
            case NpTicketFieldValue.Type.Binary:
                {
                    byte[] buffer = new byte[fieldSize];
                    if (!stream.ReadAll(buffer))
                        return false;

                    value = new NpTicketFieldValue.Binary
                    {
                        FieldPosition = fieldPosition,
                        ValuePosition = valuePosition,
                        EndPosition = (int)stream.Position,
                        Value = buffer
                    };

                    return true;
                }
            case NpTicketFieldValue.Type.Body:
                {
                    uint bodySize = fieldSize;

                    if (ticketVersion == NpTicketVersion.Version40)
                    {
                        if (!stream.TryReadUShort(out ushort extendedSize))
                            return false;

                        bodySize = bodySize << 16 | extendedSize;

                        valuePosition = (int)stream.Position;
                    }

                    if(!TryReadTicketSection(stream, ticketVersion, bodySize, out NpTicketSection? bodySection))
                        return false;

                    value = new NpTicketFieldValue.Body
                    {
                        FieldPosition = fieldPosition,
                        ValuePosition = valuePosition,
                        EndPosition = (int)stream.Position,
                        Value = bodySection
                    };

                    return true;
                }
            case NpTicketFieldValue.Type.Footer:
                {
                    if (!TryReadTicketSection(stream, ticketVersion, fieldSize, out NpTicketSection? bodySection))
                        return false;

                    value = new NpTicketFieldValue.Footer
                    {
                        FieldPosition = fieldPosition,
                        ValuePosition = valuePosition,
                        EndPosition = (int)stream.Position,
                        Value = bodySection
                    };

                    return true;
                }
            case NpTicketFieldValue.Type.Date:
                {
                    if (fieldSize != 4)
                        return false;

                    Span<byte> buffer = stackalloc byte[4];
                    if (!stream.ReadAll(buffer))
                        return false;

                    int year = (buffer[0] << 8) | buffer[1];
                    int month = buffer[2];
                    int day = buffer[3];

                    DateOnly date = new DateOnly(year, month, day);

                    value = new NpTicketFieldValue.Date
                    {
                        FieldPosition = fieldPosition,
                        ValuePosition = valuePosition,
                        EndPosition = (int)stream.Position,
                        Value = date
                    };

                    return true;
                }
            default:
                {
                    byte[] buffer = new byte[fieldSize];
                    if (!stream.ReadAll(buffer))
                        return false;

                    value = new NpTicketFieldValue.Unknown(fieldType)
                    {
                        FieldPosition = fieldPosition,
                        ValuePosition = valuePosition,
                        EndPosition = (int)stream.Position,
                        Value = buffer
                    };

                    return true;
                }
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        string version = TicketVersion switch
        {
            NpTicketVersion.Version20 => "2.0",
            NpTicketVersion.Version21 => "2.1",
            NpTicketVersion.Version30 => "3.0",
            NpTicketVersion.Version40 => "4.0",
            _ => "unknown"
        };

        sb.Append($"NpRawTicket(ver: {version}) ");
        sb.AppendLine(Root.ToString());
        return sb.ToString();
    }
}
