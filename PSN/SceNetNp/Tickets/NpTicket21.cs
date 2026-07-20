using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceNetNp.Tickets;


/*
    // From https://www.psdevwiki.com/ps3/X-I-5-Ticket
    NpRawTicket(ver: 2.1) {
            [0] (Body) = {
                    [0] (Binary) = 4C47563B81394A22D86BC157716EFDB8AB63CC51
                    [1] (UInt) = 256
                    [2] (Time) = 13.10.2012 19:22:01
                    [3] (Time) = 14.10.2012 19:22:01
                    [4] (ULong) = 6426198810416408665
                    [5] (String) = "XXXXXXXX"
                    [6] (Binary) = 66720002
                    [7] (String) = "c9"
                    [8] (Binary) = 4956303030312D4E50585330313030315F30300000000000
                    [9] (UInt) = 402653696
                    [10] (Empty)
                    [11] (Empty)
            }
            [1] (Footer) = {
                    [0] (Binary) = 34CD3CA9
                    [1] (Binary) = 3A4B426692DA6B7CB74CE8D94F2B771591B8A4A9
            }
    }
 */

class NpTicket21 : NpTicket
{
    public override NpTicketVersion Version => NpTicketVersion.Version21;
    public override ushort SubjectAge { get; }
    public override ushort SubjectStatus { get; }

    /// <summary>
    /// Not available in Version 2.1
    /// </summary>
    public override DateOnly? SubjectDob => null;


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public NpTicket21(NpRawTicket ticket, out bool ok) : base(ticket, out ok)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        if (!ok)
            return;

        ok = false;

        if (!ticket.Body.TryGetUInt(9, out uint ageAndStatus))
            return;

        SubjectAge = (ushort)(ageAndStatus >> 16);
        SubjectAge = (ushort)((SubjectAge >> 8) | (SubjectAge << 8));

        SubjectStatus = (ushort)(ageAndStatus & 0xFFFF);
        SubjectStatus = (ushort)((SubjectStatus >> 8) | (SubjectStatus << 8));

        ok = true;
    }
}

