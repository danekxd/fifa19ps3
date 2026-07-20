using Blaze3SDK.Blaze.Redirector;
using Blaze3SDK.Components;
using BlazeCommon;
using NLog;

namespace Zamboni14Legacy.Components.Blaze;

internal class RedirectorComponent : RedirectorComponentBase.Server
{
    private static readonly Logger Logger =
        LogManager.GetCurrentClassLogger();

    public override Task<ServerInstanceInfo> GetServerInstanceAsync(
        ServerInstanceRequest request,
        BlazeRpcContext context)
    {
        Logger.Warn(
            $"FIFA redirector request received. " +
            $"Returning {Program.GameServerIp}:" +
            $"{Program.ZamboniConfig.GameServerPort}"
        );

        var responseData = new ServerInstanceInfo
        {
            mAddress = new ServerAddress
            {
                IpAddress = new IpAddress
                {
                    mHostname = Program.GameServerIp,
                    mIp = Util.GetIPAddressAsUInt(
                        Program.GameServerIp
                    ),
                    mPort = Program.ZamboniConfig.GameServerPort
                }
            },

            mSecure = false,
            mDefaultDnsAddress = 0
        };

        Logger.Warn(
            $"Redirector response ready: " +
            $"{Program.GameServerIp}:" +
            $"{Program.ZamboniConfig.GameServerPort}, " +
            $"secure={responseData.mSecure}"
        );

        return Task.FromResult(responseData);
    }
}