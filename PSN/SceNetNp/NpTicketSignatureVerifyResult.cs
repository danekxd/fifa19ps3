using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceNetNp;

public enum NpTicketSignatureVerifyResult
{
    Success,
    UnknownCipherKeyId,
    InvalidTicket,
    InvalidSignature
}
