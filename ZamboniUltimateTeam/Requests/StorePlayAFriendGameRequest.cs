using Tdf;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct StorePlayAFriendGameRequest
{
    [TdfMember("DISC")] 
    public byte mDisconnect;
    
    [TdfMember("GMID")] 
    public uint mGameId;
    
    [TdfMember("OT")] 
    public byte mOvertime;
    
    [TdfMember("QUIT")] 
    public byte mQuit;
    
    [TdfMember("SCRA")] 
    public byte mScoreA;
    
    [TdfMember("SCRB")] 
    public byte mScoreB;
    
    [TdfMember("UIDA")] 
    public byte mUserIdA;
    
    [TdfMember("UIDB")] 
    public byte mUserIdB;

}