using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceNetNp;

public abstract class NpTicketFieldValue
{
    public enum Type
    {
        Empty = 0x0000,
        UInt = 0x0001,
        ULong = 0x0002,
        String = 0x0004,
        Time = 0x0007,
        Binary = 0x0008,
        Body = 0x3000,
        Footer = 0x3002,
        Date = 0x3011
    }

    public abstract Type ValueType { get; }
    public required int FieldPosition { init; get; }
    public required int ValuePosition { init; get; }
    public required int EndPosition { init; get; }

    public sealed class Empty : NpTicketFieldValue
    {
        public override Type ValueType => Type.Empty;

        public override string ToString()
        {
            return string.Empty;
        }
    }

    public sealed class UInt : NpTicketFieldValue
    {
        public override Type ValueType => Type.UInt;
        public required uint Value { init; get; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public sealed class ULong : NpTicketFieldValue
    {
        public override Type ValueType => Type.ULong;
        public required ulong Value { init; get; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public sealed class String : NpTicketFieldValue
    {
        public override Type ValueType => Type.String;
        public required string Value { init; get; }

        public override string ToString()
        {
            return $"\"{Value}\"";
        }
    }

    public sealed class Time : NpTicketFieldValue
    {
        public override Type ValueType => Type.Time;
        public required DateTime Value { init; get; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public sealed class Binary : NpTicketFieldValue
    {
        public override Type ValueType => Type.Binary;
        public required ReadOnlyMemory<byte> Value { init; get; }

        public override string ToString()
        {
            return Convert.ToHexString(Value.ToArray());
        }
    }

    public sealed class Body : NpTicketFieldValue
    {
        public override Type ValueType => Type.Body;
        public required NpTicketSection Value { init; get; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public sealed class Footer : NpTicketFieldValue
    {
        public override Type ValueType => Type.Footer;
        public required NpTicketSection Value { init; get; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public sealed class Date : NpTicketFieldValue
    {
        public override Type ValueType => Type.Date;
        public required DateOnly Value { init; get; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public sealed class Unknown : NpTicketFieldValue
    {
        public override Type ValueType { get; }
        public required ReadOnlyMemory<byte> Value { init; get; }
        public Unknown(Type type)
        {
            if(Enum.IsDefined(type))
                throw new ArgumentException("Unknown type must not be a defined enum of NpTicketFieldValue.Type", nameof(type));
            
            ValueType = type;
        }

        public override string ToString()
        {
            return Convert.ToHexString(Value.ToArray());
        }
    }
}
