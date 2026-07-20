namespace ZamboniUltimateTeam;

public interface IServerProvider
{
    long GetUserIdByConnectionId(long connectionId);
    string GetUserNameByUserId(long userId);
}