using Blaze3SDK.Blaze.Example;
using Tdf;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct LoginResponse
{
    [TdfMember("ABBR")] 
    public string mTeamAbbreviation;

    [TdfMember("BNUS")] 
    public byte mBonusAwarded;
    
    [TdfMember("CVER")]
    public List<int> mCVER;

    [TdfMember("DRRC")] 
    public short mDRRC;
    
    [TdfMember("DRRL")] 
    public short mDRRL;
    
    [TdfMember("DRRO")] 
    public short mDRRO;
    
    [TdfMember("DRRW")] 
    public short mDRRW;
    
    [TdfMember("NAME")] 
    public string mTeamName;

    [TdfMember("RWRD")] 
    public short mRewardType;
    
    [TdfMember("TNOW")] 
    public int mRewardValue;
    
    [TdfMember("TRBS")] 
    public int mTRBS;
    
    [TdfMember("UID")] 
    public ulong mUserId;
}