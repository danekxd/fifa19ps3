using System.Reflection;
using Tdf;

namespace BlazeCommon
{
    /// <summary>
    /// Wire framing used by a ProtoFire connection.
    ///
    /// LegacyFireFrame is the 12-byte format used by the original Zamboni
    /// implementation. Fire2 is the 16-byte format used by FIFA 19
    /// (Blaze SDK 15.1.1.0.0).
    /// </summary>
    public enum ProtoFireFraming
    {
        LegacyFireFrame = 0,
        Fire2 = 1
    }

    public class ProtoFirePacket
    {
        private const int Fire2HeaderSize = 16;

        public FireFrame Frame { get; set; }
        public byte[] Data { get; set; }

        /// <summary>
        /// Fire2 metadata appears between the 16-byte header and the TDF body.
        /// FIFA's first preAuth packet uses an empty metadata section.
        /// </summary>
        public byte[] Metadata { get; set; }

        public ProtoFireFraming Framing { get; set; }

        /// <summary>
        /// Raw Fire2 options byte (header byte 14).
        /// </summary>
        public byte Fire2Options { get; set; }

        /// <summary>
        /// Raw Fire2 reserved byte (header byte 15).
        /// </summary>
        public byte Fire2Reserved { get; set; }

        public ProtoFirePacket(FireFrame frame, byte[] data)
            : this(
                frame,
                data,
                ProtoFireFraming.LegacyFireFrame,
                Array.Empty<byte>(),
                0,
                0)
        {
        }

        public ProtoFirePacket(
            FireFrame frame,
            byte[] data,
            ProtoFireFraming framing,
            byte[]? metadata = null,
            byte fire2Options = 0,
            byte fire2Reserved = 0)
        {
            Frame = frame ?? throw new ArgumentNullException(nameof(frame));
            Data = data ?? throw new ArgumentNullException(nameof(data));
            Framing = framing;
            Metadata = metadata ?? Array.Empty<byte>();
            Fire2Options = fire2Options;
            Fire2Reserved = fire2Reserved;
        }

        public MemoryStream GetDataStream()
        {
            return new MemoryStream(Data, writable: false);
        }

        public ProtoFirePacket CreateResponsePacket(int errorCode = 0)
        {
            return CreateResponsePacket(Array.Empty<byte>(), errorCode);
        }

        public ProtoFirePacket CreateResponsePacket(
            byte[] data,
            int errorCode = 0)
        {
            return new ProtoFirePacket(
                Frame.CreateResponseFrame(errorCode),
                data,
                Framing,
                Array.Empty<byte>(),
                0,
                0);
        }

        public void WriteTo(Stream stream)
        {
            WriteTo(stream, Framing);
        }

        public void WriteTo(
            Stream stream,
            ProtoFireFraming framing)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (framing == ProtoFireFraming.Fire2)
            {
                WriteFire2To(stream);
                return;
            }

            Frame.Size = checked((uint)Data.Length);
            Frame.WriteTo(stream);

            if (Data.Length != 0)
                stream.Write(Data, 0, Data.Length);
        }

        public Task WriteToAsync(Stream stream)
        {
            return WriteToAsync(stream, Framing);
        }

        public async Task WriteToAsync(
            Stream stream,
            ProtoFireFraming framing)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (framing == ProtoFireFraming.Fire2)
            {
                await WriteFire2ToAsync(stream).ConfigureAwait(false);
                return;
            }

            Frame.Size = checked((uint)Data.Length);
            await Frame.WriteToAsync(stream).ConfigureAwait(false);

            if (Data.Length != 0)
            {
                await stream.WriteAsync(
                        Data,
                        0,
                        Data.Length)
                    .ConfigureAwait(false);
            }
        }

        private void WriteFire2To(Stream stream)
        {
            byte[] header = CreateFire2Header();
            stream.Write(header, 0, header.Length);

            if (Metadata.Length != 0)
                stream.Write(Metadata, 0, Metadata.Length);

            if (Data.Length != 0)
                stream.Write(Data, 0, Data.Length);
        }

        private async Task WriteFire2ToAsync(Stream stream)
        {
            byte[] header = CreateFire2Header();

            await stream.WriteAsync(
                    header,
                    0,
                    header.Length)
                .ConfigureAwait(false);

            if (Metadata.Length != 0)
            {
                await stream.WriteAsync(
                        Metadata,
                        0,
                        Metadata.Length)
                    .ConfigureAwait(false);
            }

            if (Data.Length != 0)
            {
                await stream.WriteAsync(
                        Data,
                        0,
                        Data.Length)
                    .ConfigureAwait(false);
            }
        }

        private byte[] CreateFire2Header()
        {
            if (Metadata.Length > ushort.MaxValue)
            {
                throw new InvalidOperationException(
                    "Fire2 metadata exceeds 65535 bytes.");
            }

            uint payloadSize = checked((uint)Data.Length);
            ushort metadataSize = checked((ushort)Metadata.Length);
            uint messageNumber = Frame.MsgNum & 0x00FF_FFFF;

            byte[] header = new byte[Fire2HeaderSize];

            WriteUInt32BigEndian(header, 0, payloadSize);
            WriteUInt16BigEndian(header, 4, metadataSize);
            WriteUInt16BigEndian(header, 6, Frame.Component);
            WriteUInt16BigEndian(header, 8, Frame.Command);

            header[10] = (byte)(messageNumber >> 16);
            header[11] = (byte)(messageNumber >> 8);
            header[12] = (byte)messageNumber;

            // Fire2 stores the message type in the high three bits and
            // the user index in the low five bits.
            header[13] = (byte)(
                (((byte)Frame.MsgType & 0x07) << 5) |
                (Frame.UserIndex & 0x1F));

            header[14] = Fire2Options;
            header[15] = Fire2Reserved;

            return header;
        }

        private static void WriteUInt16BigEndian(
            byte[] buffer,
            int offset,
            ushort value)
        {
            buffer[offset] = (byte)(value >> 8);
            buffer[offset + 1] = (byte)value;
        }

        private static void WriteUInt32BigEndian(
            byte[] buffer,
            int offset,
            uint value)
        {
            buffer[offset] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)value;
        }

        private static readonly MethodInfo DecodeMethod =
            typeof(ITdfDecoder).GetMethod(
                nameof(ITdfDecoder.Decode),
                new[] { typeof(Type), typeof(Stream) })!;

        public IBlazePacket Decode(
            Type type,
            ITdfDecoder decoder)
        {
            Type blazePacketType =
                typeof(BlazePacket<>).MakeGenericType(type);

            object decoded = DecodeMethod.Invoke(
                decoder,
                new object?[] { type, GetDataStream() })!;

            return (IBlazePacket)Activator.CreateInstance(
                blazePacketType,
                Frame,
                decoded)!;
        }
    }
}
