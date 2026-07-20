using Tdf;

namespace ZamboniCommonComponents.Structs.TdfTagged
{
    [TdfStruct(0x7C56BF5B)]
    public struct Report
    {

        [TdfMember("CGRT")]
        public object? mClubReport;
        
        [TdfMember("GAMR")]
        public GameInfoReport mGameInfoReport;
        
        [TdfMember("IFPR")]
        public object? mIFPR;
        
        [TdfMember("PLYR")]
        public SortedDictionary<ulong, PlayerReport> mPlayerReports;
        
        [TdfMember("TAMR")]
        public object? mTAMR;
        
        
    }
}