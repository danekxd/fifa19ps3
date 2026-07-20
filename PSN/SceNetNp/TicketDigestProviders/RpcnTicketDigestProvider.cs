using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using SceNetNp.Ciphers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceNetNp.TicketDigestProviders;

public class RpcnTicketDigestProvider : ITicketDigestProvider
{
    public bool TryComputeTicketDigest(NpRawTicket ticket, [NotNullWhen(true)] out byte[]? hash)
    {
        if (!ticket.Root.TryGetBody(0, out NpTicketFieldValue.Body? bodyValue))
        {
            hash = null;
            return false;
        }

        IDigest digest = new Sha224Digest();

        // digest is body data (including field type and size)
        int length = bodyValue.EndPosition - bodyValue.FieldPosition;

        ReadOnlySpan<byte> bytes = ticket.TicketBytes.Slice(bodyValue.FieldPosition, length);
        digest.BlockUpdate(bytes);

        hash = new byte[digest.GetDigestSize()];
        digest.DoFinal(hash);
        return true;
    }
}
