namespace ZamboniCommonComponents.Structs;

public enum GrantMethod : int
{
    GRANT_METHOD_ALL = 0,
    GRANT_METHOD_ONLINE_PASS = 1,
    GRANT_METHOD_AT_SUBSCRIPTION_START = 2,
    GRANT_METHOD_ON_CONTENT_PUBLISH = 3,
    GRANT_METHOD_AT_LOGIN = 4,
}