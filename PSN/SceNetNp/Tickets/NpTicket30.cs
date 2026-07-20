using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceNetNp.Tickets;

/*
    // From https://www.psdevwiki.com/ps3/X-I-5-Ticket
    NpRawTicket(ver: 3.0) {
            [0] (Body) = {
                    [0] (Binary) = 02D65A9AD81D093427424B9B67293EF29755BC78
                    [1] (UInt) = 256
                    [2] (Time) = 13.10.2012 15:51:12
                    [3] (Time) = 14.10.2012 15:51:12
                    [4] (ULong) = 6426198810416408665
                    [5] (String) = "XXXXXXXX"
                    [6] (Binary) = 66720002
                    [7] (String) = "c9"
                    [8] (Binary) = 4956303030312D4E50585330313030315F30300000000000
                    [9] (Date) = 29.02.1988
                    [10] (UInt) = 402653696
                    [11] (0x3010)
                    [12] (Empty)
            }
            [1] (Footer) = {
                    [0] (Binary) = 34CD3CA9
                    [1] (Binary) = DC3B1A15ECC8155585EA9A07E21FDDBE3FA8569C
            }
    }
 */


class NpTicket30 : NpTicket
{
    public override NpTicketVersion Version => NpTicketVersion.Version30;
    public override ushort SubjectAge { get; }
    public override ushort SubjectStatus { get; }
    public override DateOnly? SubjectDob { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public NpTicket30(NpRawTicket ticket, out bool ok) : base(ticket, out ok)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        if (!ok)
            return;

        ok = false;

        if (!ticket.Body.TryGetDate(9, out DateOnly subjectDob))
            return;
        SubjectDob = subjectDob;

        if (!ticket.Body.TryGetUInt(10, out uint ageAndStatus))
            return;

        SubjectAge = (ushort)(ageAndStatus >> 16);
        SubjectAge = (ushort)((SubjectAge >> 8) | (SubjectAge << 8));

        SubjectStatus = (ushort)(ageAndStatus & 0xFFFF);
        SubjectStatus = (ushort)((SubjectStatus >> 8) | (SubjectStatus << 8));

        ok = true;
    }
}

