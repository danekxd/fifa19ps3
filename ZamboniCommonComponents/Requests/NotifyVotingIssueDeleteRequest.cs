using Tdf;

namespace ZamboniCommonComponents.Requests;

[TdfStruct]
public struct NotifyVotingIssueDeleteRequest
{
    [TdfMember("ISID")] 
    public int mISID;

}