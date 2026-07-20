using Blaze3SDK;
using BlazeCommon;
using NLog;
using ZamboniCommonComponents.Requests;
using ZamboniCommonComponents.Responses;

namespace ZamboniCommonComponents.Bases;

public static class OSDKSeasonalPlayComponentBase
{
    public enum OSDKSeasonalPlayComponentCommand : ushort
    {
        getSeasonConfiguration = 1,
        getSeasonDetails = 3,
    }

    public enum OSDKSeasonalPlayComponentNotification : ushort
    {
    }

    public const ushort Id = 2257;
    public const string Name = "OSDKSeasonalPlayComponent";

    public static Type GetCommandRequestType(OSDKSeasonalPlayComponentCommand componentCommand)
    {
        return componentCommand switch
        {
            OSDKSeasonalPlayComponentCommand.getSeasonConfiguration => typeof(NullStruct),
            OSDKSeasonalPlayComponentCommand.getSeasonDetails => typeof(SeasonDetailsRequest),
            _ => typeof(NullStruct)
        };
    }

    public static Type GetCommandResponseType(OSDKSeasonalPlayComponentCommand componentCommand)
    {
        return componentCommand switch
        {
            OSDKSeasonalPlayComponentCommand.getSeasonConfiguration => typeof(GetSeasonConfigurationResponse),
            OSDKSeasonalPlayComponentCommand.getSeasonDetails => typeof(SeasonDetails),
            _ => typeof(NullStruct)
        };
    }

    public static Type GetCommandErrorResponseType(OSDKSeasonalPlayComponentCommand componentCommand)
    {
        return componentCommand switch
        {
            OSDKSeasonalPlayComponentCommand.getSeasonConfiguration => typeof(NullStruct),
            _ => typeof(NullStruct)
        };
    }

    public static Type GetNotificationType(OSDKSeasonalPlayComponentNotification notification)
    {
        return notification switch
        {
            _ => typeof(NullStruct)
        };
    }

    public class Server : BlazeServerComponent<OSDKSeasonalPlayComponentCommand, OSDKSeasonalPlayComponentNotification, Blaze3RpcError>
    {
        public Server() : base(OSDKSeasonalPlayComponentBase.Id, OSDKSeasonalPlayComponentBase.Name)
        {
        }

        [BlazeCommand((ushort)OSDKSeasonalPlayComponentCommand.getSeasonConfiguration)]
        public virtual Task<GetSeasonConfigurationResponse> SeasonConfigurationRequestAsync(NullStruct request, BlazeRpcContext context)
        {
            throw new BlazeRpcException(Blaze3RpcError.ERR_COMMAND_NOT_FOUND);
        }
        
        [BlazeCommand((ushort)OSDKSeasonalPlayComponentCommand.getSeasonDetails)]
        public virtual Task<SeasonDetails> SeasonDetailsRequestAsync(SeasonDetailsRequest request, BlazeRpcContext context)
        {
            throw new BlazeRpcException(Blaze3RpcError.ERR_COMMAND_NOT_FOUND);
        }


        public override Type GetCommandRequestType(OSDKSeasonalPlayComponentCommand componentCommand)
        {
            return OSDKSeasonalPlayComponentBase.GetCommandRequestType(componentCommand);
        }

        public override Type GetCommandResponseType(OSDKSeasonalPlayComponentCommand componentCommand)
        {
            return OSDKSeasonalPlayComponentBase.GetCommandResponseType(componentCommand);
        }

        public override Type GetCommandErrorResponseType(OSDKSeasonalPlayComponentCommand componentCommand)
        {
            return OSDKSeasonalPlayComponentBase.GetCommandErrorResponseType(componentCommand);
        }

        public override Type GetNotificationType(OSDKSeasonalPlayComponentNotification notification)
        {
            return OSDKSeasonalPlayComponentBase.GetNotificationType(notification);
        }
    }

    public class Client : BlazeClientComponent<OSDKSeasonalPlayComponentCommand, OSDKSeasonalPlayComponentNotification, Blaze3RpcError>
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public Client(BlazeClientConnection connection) : base(OSDKSeasonalPlayComponentBase.Id, OSDKSeasonalPlayComponentBase.Name)
        {
            Connection = connection;
            if (!Connection.Config.AddComponent(this))
                throw new InvalidOperationException($"A component with Id({Id}) has already been created for the connection.");
        }

        private BlazeClientConnection Connection { get; }

        public GetSeasonConfigurationResponse SeasonConfigurationRequest(NullStruct request)
        {
            return Connection.SendRequest<NullStruct, GetSeasonConfigurationResponse, NullStruct>(this, (ushort)OSDKSeasonalPlayComponentCommand.getSeasonConfiguration, request);
        }

        public Task<GetSeasonConfigurationResponse> SeasonConfigurationAsync(NullStruct request)
        {
            return Connection.SendRequestAsync<NullStruct, GetSeasonConfigurationResponse, NullStruct>(this, (ushort)OSDKSeasonalPlayComponentCommand.getSeasonConfiguration, request);
        }
        
        public SeasonDetails SeasonDetailsRequest(SeasonDetailsRequest request)
        {
            return Connection.SendRequest<SeasonDetailsRequest, SeasonDetails, NullStruct>(this, (ushort)OSDKSeasonalPlayComponentCommand.getSeasonDetails, request);
        }

        public Task<SeasonDetails> SeasonDetailsRequestAsync(SeasonDetailsRequest request)
        {
            return Connection.SendRequestAsync<SeasonDetailsRequest, SeasonDetails, NullStruct>(this, (ushort)OSDKSeasonalPlayComponentCommand.getSeasonDetails, request);
        }

        public override Type GetCommandRequestType(OSDKSeasonalPlayComponentCommand componentCommand)
        {
            return OSDKSeasonalPlayComponentBase.GetCommandRequestType(componentCommand);
        }

        public override Type GetCommandResponseType(OSDKSeasonalPlayComponentCommand componentCommand)
        {
            return OSDKSeasonalPlayComponentBase.GetCommandResponseType(componentCommand);
        }

        public override Type GetCommandErrorResponseType(OSDKSeasonalPlayComponentCommand componentCommand)
        {
            return OSDKSeasonalPlayComponentBase.GetCommandErrorResponseType(componentCommand);
        }

        public override Type GetNotificationType(OSDKSeasonalPlayComponentNotification notification)
        {
            return OSDKSeasonalPlayComponentBase.GetNotificationType(notification);
        }
    }
}