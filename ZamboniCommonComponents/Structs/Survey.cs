using Tdf;

namespace ZamboniCommonComponents.Structs;

[TdfStruct]
public struct Survey
{
    [TdfMember("DESC")] 
    public string mDescription;

    [TdfMember("SUID")] 
    public int mSurveyIdOrScriptId;
    
    [TdfMember("TITL")] 
    public string mTitle;
    
}