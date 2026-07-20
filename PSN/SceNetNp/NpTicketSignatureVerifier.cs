using SceNetNp.Ciphers;
using SceNetNp.TicketDigestProviders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceNetNp;

public class NpTicketSignatureVerifier(ITicketDigestProvider digestProvider)
{
    public IList<CipherKey> CipherKeys { get; set; } = [];

    public NpTicketSignatureVerifyResult VerifySignature(NpTicket ticket)
    {
        return VerifySignature(ticket.RawTicket);
    }

    public NpTicketSignatureVerifyResult VerifySignature(NpRawTicket ticket)
    {
        // all ticket versions have the same footer structure
        if (!ticket.Footer.TryGetBinary(0, out NpTicketFieldValue.Binary? keyID) ||
           !ticket.Footer.TryGetBinary(1, out NpTicketFieldValue.Binary? signature))
        {
            return NpTicketSignatureVerifyResult.InvalidTicket;
        }

        CipherKey? cipherKey = CipherKeys.FirstOrDefault(key => key.KeyId.SequenceEqual(keyID.Value.Span));
        if (cipherKey is null)
            return NpTicketSignatureVerifyResult.UnknownCipherKeyId;

        if(!digestProvider.TryComputeTicketDigest(ticket, out byte[]? hash))
            return NpTicketSignatureVerifyResult.InvalidTicket;

        if(!cipherKey.VerifySignature(hash, signature.Value.ToArray()))
            return NpTicketSignatureVerifyResult.InvalidSignature;

        return NpTicketSignatureVerifyResult.Success;
    }
}
