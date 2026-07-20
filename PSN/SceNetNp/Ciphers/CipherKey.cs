using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceNetNp.Ciphers;

public abstract class CipherKey
{
    public static readonly ECDsaCipherKey Rpcn = ECDsaCipherKey.CreateForRpcn("5250434e",
        "b07bc0f0addb97657e9f389039e8d2b9c97dc2a31d3042e7d0479b93",
        "d81c42b0abdf6c42191a31e31f93342f8f033bd529c2c57fdb5a0a7d");


    private byte[] keyBytes;
    public ReadOnlySpan<byte> KeyId => keyBytes;

    public CipherKey(string keyId)
    {
        keyBytes = Convert.FromHexString(keyId);
    }

    public abstract bool VerifySignature(byte[] messageHash, byte[] signature);
}
