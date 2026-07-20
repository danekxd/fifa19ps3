using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceNetNp;

public class NpTicketSection : IReadOnlyList<NpTicketFieldValue>
{
    private List<NpTicketFieldValue> fields;

    public NpTicketSection(List<NpTicketFieldValue> fields)
    {
        this.fields = fields;
    }

    public NpTicketSection()
    {
        fields = new List<NpTicketFieldValue>();
    }

    public NpTicketFieldValue this[int index] => fields[index];

    public int Count => fields.Count;

    public IEnumerator<NpTicketFieldValue> GetEnumerator()
    {
        return fields.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return fields.GetEnumerator();
    }

    public bool TryGetField(int index, [NotNullWhen(true)] out NpTicketFieldValue? value)
    {
        if (index < 0 || index >= Count)
        {
            value = null;
            return false;
        }

        value = fields[index];
        return true;
    }

    public bool TryGetEmpty(int index, [NotNullWhen(true)] out NpTicketFieldValue.Empty? value)
    {
        if (TryGetField(index, out NpTicketFieldValue? field)
            && field is NpTicketFieldValue.Empty emptyValue)
        {
            value = emptyValue;
            return true;
        }

        value = null;
        return false;
    }


    public bool TryGetUInt(int index, [NotNullWhen(true)] out NpTicketFieldValue.UInt? value)
    {
        if(TryGetField(index, out NpTicketFieldValue? field) 
            && field is NpTicketFieldValue.UInt fieldValue)
        {
            value = fieldValue;
            return true;
        }

        value = null;
        return false;
    }

    public bool TryGetUInt(int index, out uint value)
    {
        if(!TryGetUInt(index, out NpTicketFieldValue.UInt? fieldValue))
        {
            value = 0;
            return false;
        }

        value = fieldValue.Value;
        return true;
    }

    public bool TryGetULong(int index, [NotNullWhen(true)] out NpTicketFieldValue.ULong? value)
    {
        if (TryGetField(index, out NpTicketFieldValue? field)
            && field is NpTicketFieldValue.ULong fieldValue)
        {
            value = fieldValue;
            return true;
        }

        value = null;
        return false;
    }

    public bool TryGetULong(int index, out ulong value)
    {
        if (!TryGetULong(index, out NpTicketFieldValue.ULong? fieldValue))
        {
            value = 0;
            return false;
        }

        value = fieldValue.Value;
        return true;
    }

    public bool TryGetString(int index, [NotNullWhen(true)] out NpTicketFieldValue.String? value)
    {
        if (TryGetField(index, out NpTicketFieldValue? field)
            && field is NpTicketFieldValue.String fieldValue)
        {
            value = fieldValue;
            return true;
        }

        value = null;
        return false;
    }

    public bool TryGetString(int index, [NotNullWhen(true)] out string? value)
    {
        if (TryGetString(index, out NpTicketFieldValue.String? fieldValue))
        {
            value = fieldValue.Value;
            return true;
        }

        value = null;
        return false;
    }

    public bool TryGetTime(int index, [NotNullWhen(true)] out NpTicketFieldValue.Time? value)
    {
        if (TryGetField(index, out NpTicketFieldValue? field)
            && field is NpTicketFieldValue.Time fieldValue)
        {
            value = fieldValue;
            return true;
        }

        value = null;
        return false;
    }

    public bool TryGetTime(int index, out DateTime value)
    {
        if (TryGetTime(index, out NpTicketFieldValue.Time? fieldValue))
        {
            value = fieldValue.Value;
            return true;
        }

        value = default;
        return false;
    }

    public bool TryGetBinary(int index, [NotNullWhen(true)] out NpTicketFieldValue.Binary? value)
    {
        if (TryGetField(index, out NpTicketFieldValue? field)
            && field is NpTicketFieldValue.Binary fieldValue)
        {
            value = fieldValue;
            return true;
        }

        value = null;
        return false;
    }

    public bool TryGetBinary(int index, out ReadOnlyMemory<byte> value)
    {
        if (TryGetBinary(index, out NpTicketFieldValue.Binary? fieldValue))
        {
            value = fieldValue.Value;
            return true;
        }

        value = default;
        return false;
    }

    public bool TryGetBody(int index, [NotNullWhen(true)] out NpTicketFieldValue.Body? value)
    {
        if (TryGetField(index, out NpTicketFieldValue? field)
            && field is NpTicketFieldValue.Body fieldValue)
        {
            value = fieldValue;
            return true;
        }

        value = null;
        return false;
    }

    public bool TryGetBody(int index, [NotNullWhen(true)] out NpTicketSection? value)
    {
        if (TryGetBody(index, out NpTicketFieldValue.Body? fieldValue))
        {
            value = fieldValue.Value;
            return true;
        }

        value = null;
        return false;
    }

    public bool TryGetFooter(int index, [NotNullWhen(true)] out NpTicketFieldValue.Footer? value)
    {
        if (TryGetField(index, out NpTicketFieldValue? field)
            && field is NpTicketFieldValue.Footer fieldValue)
        {
            value = fieldValue;
            return true;
        }

        value = null;
        return false;
    }

    public bool TryGetFooter(int index, [NotNullWhen(true)] out NpTicketSection? value)
    {
        if (TryGetFooter(index, out NpTicketFieldValue.Footer? fieldValue))
        {
            value = fieldValue.Value;
            return true;
        }

        value = null;
        return false;
    }

    public bool TryGetDate(int index, [NotNullWhen(true)] out NpTicketFieldValue.Date? value)
    {
        if (TryGetField(index, out NpTicketFieldValue? field)
            && field is NpTicketFieldValue.Date fieldValue)
        {
            value = fieldValue;
            return true;
        }

        value = null;
        return false;
    }

    public bool TryGetDate(int index, out DateOnly value)
    {
        if (TryGetDate(index, out NpTicketFieldValue.Date? fieldValue))
        {
            value = fieldValue.Value;
            return true;
        }

        value = default;
        return false;
    }

    public bool TryGetUnknown(int index, [NotNullWhen(true)] out NpTicketFieldValue.Unknown? value)
    {
        if (TryGetField(index, out NpTicketFieldValue? field)
            && field is NpTicketFieldValue.Unknown fieldValue)
        {
            value = fieldValue;
            return true;
        }

        value = null;
        return false;
    }


    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("{");

        for (int i = 0; i < fields.Count ; i++)
        {
            NpTicketFieldValue field = fields[i];

            sb.Append("\t[");
            sb.Append(i);
            sb.Append("] (");

            if(Enum.IsDefined(field.ValueType))
                sb.Append(field.ValueType);
            else //0x0000
            {
                sb.Append("0x");
                sb.Append(((ushort)field.ValueType).ToString("X4"));
            }
            sb.Append(')');    
             
            string str = field.ToString() ?? string.Empty;
            string[] lines = str.Split(["\r\n", "\r"], StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length == 0)
                sb.AppendLine();
            else if (lines.Length > 0)
            {
                sb.Append(" = ");
                sb.AppendLine(lines[0]);

                for (int j = 1; j < lines.Length; j++)
                {
                    sb.Append("\t");
                    sb.AppendLine(lines[j]);
                }
            }


        }

        sb.AppendLine("}");

        return sb.ToString();
    }
}
