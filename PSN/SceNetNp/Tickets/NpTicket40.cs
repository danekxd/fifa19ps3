using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceNetNp.Tickets;

/*
    // From https://www.psdevwiki.com/ps3/X-I-5-Ticket
    NpRawTicket(ver: 4.0) {
            [0] (Body) = {
                    [0] (Binary) = AF6748E7272A3B13629EDFFCC5AB059B7B92FA02
                    [1] (UInt) = 256
                    [2] (Time) = 13.10.2012 19:09:48
                    [3] (Time) = 14.10.2012 19:09:48
                    [4] (ULong) = 6426198810416408665
                    [5] (String) = "XXXXXXXX"
                    [6] (Binary) = 66720002
                    [7] (String) = "c9"
                    [8] (Binary) = 4956303030312D4E50585330313030315F30300000000000
                    [9] (Date) = 29.02.1988
                    [10] (UInt) = 402653696
                    [11] (0x3010)
                    [12] (Empty)
                    [13] (Empty)
                    [14] (Binary) = 5053335F43000000
                    [15] (Binary) = 01000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
            }
            [1] (Footer) = {
                    [0] (Binary) = 34CD3CA9
                    [1] (Binary) = C4EE7580ECC1D7635400399884CF6834948FE92FB467A32E20CC88CD100E8B41
            }
    }
 */


class NpTicket40 : NpTicket
{
    public override NpTicketVersion Version => NpTicketVersion.Version40;
    public override ushort SubjectAge { get; }
    public override ushort SubjectStatus { get; }
    public override DateOnly? SubjectDob { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public NpTicket40(NpRawTicket ticket, out bool ok) : base(ticket, out ok)
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