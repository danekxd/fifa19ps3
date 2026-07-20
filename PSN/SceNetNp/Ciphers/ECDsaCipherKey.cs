using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Asn1;

namespace SceNetNp.Ciphers;

public class ECDsaCipherKey : CipherKey
{
    private ECDsaSigner signer;
    public ECPublicKeyParameters Parameters { get; }

    public ECDsaCipherKey(string keyId, string curveName, string pubKeyX, string pubKeyY) : base(keyId)
    {
        X9ECParameters xParams = ECNamedCurveTable.GetByName(curveName);
        ECDomainParameters domainParams = new ECDomainParameters(xParams.Curve, xParams.G, xParams.N, xParams.H, xParams.GetSeed());
        ECPoint ecp = domainParams.Curve.CreatePoint(new BigInteger(pubKeyX, 16), new BigInteger(pubKeyY, 16));
        Parameters = new ECPublicKeyParameters(ecp, domainParams);
        signer = new ECDsaSigner();
        signer.Init(false, Parameters);
    }

    public override bool VerifySignature(byte[] messageHash, byte[] signature)
    {
        try
        {
            Asn1InputStream decoder = new Asn1InputStream(signature);

            DerSequence? seq = decoder.ReadObject() as DerSequence;
            if (seq == null || seq.Count < 2)
                return false;

            if (seq[0] is not DerInteger rValue)
                return false;

            if (seq[1] is not DerInteger sValue)
                return false;

            return signer.VerifySignature(messageHash, rValue.Value, sValue.Value);
        }
        catch { return false; }
    }

    public static ECDsaCipherKey CreateForPsn(string keyId, string pubKeyX, string pubKeyY)
    {
        return new ECDsaCipherKey(keyId, "secp192r1", pubKeyX, pubKeyY);
    }

    public static ECDsaCipherKey CreateForRpcn(string keyId, string pubKeyX, string pubKeyY)
    {
        return new ECDsaCipherKey(keyId, "secp224k1", pubKeyX, pubKeyY);
    }
}
