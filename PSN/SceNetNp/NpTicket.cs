using SceNetNp.Tickets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceNetNp;

public abstract class NpTicket
{
    private ReadOnlyMemory<byte> serialId;
    private ReadOnlyMemory<byte> serviceId;
    private ReadOnlyMemory<byte> keyId;
    private ReadOnlyMemory<byte> signature;

    /// <summary>
    /// The version of the ticket.
    /// </summary>
    public abstract NpTicketVersion Version { get; }
    /// <summary>
    /// The raw ticket that this ticket was created from.
    /// </summary>
    public NpRawTicket RawTicket { get; }

    /// <summary>
    /// Serial Id
    /// </summary>
    public ReadOnlySpan<byte> SerialId => serialId.Span;

    /// <summary>
    /// Issuer Id
    /// </summary>
    public uint IssuerId { get; }

    /// <summary>
    /// The time that this ticket was created at.
    /// </summary>
    public DateTime IssuedTime { get; }

    /// <summary>
    /// The time after which the ticket is considered expired.
    /// </summary>
    public DateTime Expiration { get; }

    /// <summary>
    /// Subject Id (also known as User Id)
    /// </summary>
    public ulong SubjectId { get; }

    /// <summary>
    /// Subject Handle (name)
    /// </summary>
    public string SubjectHandle { get; }

    /// <summary>
    /// Subject Age
    /// </summary>
    public abstract ushort SubjectAge { get; }

    /// <summary>
    /// Subject Status
    /// </summary>
    public abstract ushort SubjectStatus { get; }

    /// <summary>
    /// Subject date of birth. Field is present only in ticket versions 3.0 and 4.0.
    /// </summary>
    public abstract DateOnly? SubjectDob { get; }

    /// <summary>
    /// Region
    /// </summary>
    public string Region { get; }

    /// <summary>
    /// Subject preffered language
    /// </summary>
    public LanguageCode LanguageCode { get; }

    /// <summary>
    /// Domain
    /// </summary>
    public string Domain { get; }

    /// <summary>
    /// Service Id / Game Id
    /// </summary>
    public ReadOnlySpan<byte> ServiceId => serviceId.Span;

    /// <summary>
    /// An identifier for the key that was used to sign the ticket.
    /// </summary>
    public ReadOnlySpan<byte> KeyId => keyId.Span;

    /// <summary>
    /// Ticket signature.
    /// </summary>
    public ReadOnlySpan<byte> Signature => signature.Span;

    /// <summary>
    /// Ticket issuer (based on IssuerId).
    /// </summary>
    public NpTicketIssuer Issuer
    {
        get
        {
            if (IssuerId == 0)
                return NpTicketIssuer.ProdQa;

            if (IssuerId <= 8)
                return NpTicketIssuer.SpInt;

            if (IssuerId <= 256)
                return NpTicketIssuer.Np;

            return NpTicketIssuer.Unknown;
        }
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public NpTicket(NpRawTicket ticket, out bool ok)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        ok = false;
        if (ticket.TicketVersion != Version)
            return;

        RawTicket = ticket;

        if (!ticket.Body.TryGetBinary(0, out serialId))
            return;

        if (!ticket.Body.TryGetUInt(1, out uint issuerId))
            return;
        IssuerId = issuerId;

        if (!ticket.Body.TryGetTime(2, out DateTime issuedTime))
            return;
        IssuedTime = issuedTime;

        if (!ticket.Body.TryGetTime(3, out DateTime expiration))
            return;
        Expiration = expiration;

        if (!ticket.Body.TryGetULong(4, out ulong subjectId))
            return;
        SubjectId = subjectId;

        if (!ticket.Body.TryGetString(5, out string? subjectHandle))
            return;
        SubjectHandle = subjectHandle;

        if (!ticket.Body.TryGetBinary(6, out ReadOnlyMemory<byte> bytes) || bytes.Length < 4)
            return;

        ReadOnlySpan<byte> span = bytes.Span;
        Region = new string([(char)span[0], (char)span[1]]);
        LanguageCode = (LanguageCode)(span[2] << 8 | span[3]);

        if (!ticket.Body.TryGetString(7, out string? domain))
            return;
        Domain = domain;

        if (!ticket.Body.TryGetBinary(8, out serviceId))
            return;

        if (!ticket.Footer.TryGetBinary(0, out keyId))
            return;

        if (!ticket.Footer.TryGetBinary(1, out signature))
            return;

        ok = true;
    }

    public static bool TryParse(byte[] ticketBytes, [NotNullWhen(true)] out NpTicket? ticket)
    {
        if (NpRawTicket.TryParse(ticketBytes, out NpRawTicket? rawTicket))
            return TryParse(rawTicket, out ticket);

        ticket = null;
        return false;
    }

    public static bool TryParse(NpRawTicket rawTicket, [NotNullWhen(true)] out NpTicket? ticket)
    {
        bool ok = false;

        ticket = rawTicket.TicketVersion switch
        {
            NpTicketVersion.Version20 => new NpTicket20(rawTicket, out ok),
            NpTicketVersion.Version21 => new NpTicket21(rawTicket, out ok),
            NpTicketVersion.Version30 => new NpTicket30(rawTicket, out ok),
            NpTicketVersion.Version40 => new NpTicket40(rawTicket, out ok),
            _ => null,
        };

        if(!ok)
        {
            ticket = null;
            return false;
        }

        return ticket != null;
    }
}
