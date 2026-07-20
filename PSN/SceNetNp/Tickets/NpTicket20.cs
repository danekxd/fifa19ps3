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
    NpRawTicket(ver: 2.0) {
            [0] (Body) = {
                    [0] (Binary) = 793CF6FB7AAE34CC2D4716DCA798FDCF66EEE010
                    [1] (UInt) = 256
                    [2] (Time) = 13.10.2012 19:27:16
                    [3] (Time) = 14.10.2012 19:27:16
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
                    [1] (Binary) = E9E7104BC5D609177CF29B91DDC78D6CDEB906E1
            }
    }
 */

class NpTicket20 : NpTicket
{
    public override NpTicketVersion Version => NpTicketVersion.Version20;
    
    public override ushort SubjectAge { get; }
    public override ushort SubjectStatus { get; }

    /// <summary>
    /// Not available in Version 2.0
    /// </summary>
    public override DateOnly? SubjectDob => null;


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public NpTicket20(NpRawTicket ticket, out bool ok) : base(ticket, out ok)
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
