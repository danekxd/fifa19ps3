using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceNetNp.TicketDigestProviders;

public interface ITicketDigestProvider
{
    bool TryComputeTicketDigest(NpRawTicket ticket, [NotNullWhen(true)] out byte[]? hash);
}
