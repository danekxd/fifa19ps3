using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace SceNetNp.TicketDigestProviders;

public class PsnTicketDigestProvider : ITicketDigestProvider
{
    public bool TryComputeTicketDigest(NpRawTicket ticket, [NotNullWhen(true)] out byte[]? hash)
    {
        if(!ticket.Footer.TryGetBinary(1, out NpTicketFieldValue.Binary? signatureValue))
        {
            hash = null;
            return false;
        }

        IDigest digest = new Sha1Digest();

        // digest is all ticket bytes right up to the signature value.
        ReadOnlySpan<byte> bytes = ticket.TicketBytes.Slice(0, signatureValue.ValuePosition);
        digest.BlockUpdate(bytes);

        hash = new byte[digest.GetDigestSize()];
        digest.DoFinal(hash);
        return true;
    }
}
