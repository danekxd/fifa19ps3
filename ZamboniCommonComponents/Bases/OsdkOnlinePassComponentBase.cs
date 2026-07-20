using Blaze3SDK;
using BlazeCommon;
using NLog;
using ZamboniCommonComponents.Responses;

namespace ZamboniCommonComponents.Bases;

public static class OsdkOnlinePassComponentBase
{
    public enum OsdkOnlinePassComponentCommand : ushort
    {
        fetchGates = 3
    }

    public enum OsdkOnlinePassComponentNotification : ushort
    {
    }

    public const ushort Id = 2268;
    public const string Name = "OsdkOnlinePassComponent";

    public static Type GetCommandRequestType(OsdkOnlinePassComponentCommand componentCommand)
    {
        return componentCommand switch
        {
            OsdkOnlinePassComponentCommand.fetchGates => typeof(NullStruct),
            _ => typeof(NullStruct)
        };
    }

    public static Type GetCommandResponseType(OsdkOnlinePassComponentCommand componentCommand)
    {
        return componentCommand switch
        {
            OsdkOnlinePassComponentCommand.fetchGates => typeof(FetchGatesResponse),
            _ => typeof(NullStruct)
        };
    }

    public static Type GetCommandErrorResponseType(OsdkOnlinePassComponentCommand componentCommand)
    {
        return componentCommand switch
        {
            OsdkOnlinePassComponentCommand.fetchGates => typeof(NullStruct),
            _ => typeof(NullStruct)
        };
    }

    public static Type GetNotificationType(OsdkOnlinePassComponentNotification componentNotification)
    {
        return componentNotification switch
        {
            _ => typeof(NullStruct)
        };
    }

    public class Server : BlazeServerComponent<OsdkOnlinePassComponentCommand, OsdkOnlinePassComponentNotification, Blaze3RpcError>
    {
        public Server() : base(OsdkOnlinePassComponentBase.Id, OsdkOnlinePassComponentBase.Name)
        {
        }

        [BlazeCommand((ushort)OsdkOnlinePassComponentCommand.fetchGates)]
        public virtual Task<FetchGatesResponse> FetchGatesAsync(NullStruct request, BlazeRpcContext context)
        {
            throw new BlazeRpcException(Blaze3RpcError.ERR_COMMAND_NOT_FOUND);
        }

        public override Type GetCommandRequestType(OsdkOnlinePassComponentCommand componentCommand)
        {
            return OsdkOnlinePassComponentBase.GetCommandRequestType(componentCommand);
        }

        public override Type GetCommandResponseType(OsdkOnlinePassComponentCommand componentCommand)
        {
            return OsdkOnlinePassComponentBase.GetCommandResponseType(componentCommand);
        }

        public override Type GetCommandErrorResponseType(OsdkOnlinePassComponentCommand componentCommand)
        {
            return OsdkOnlinePassComponentBase.GetCommandErrorResponseType(componentCommand);
        }

        public override Type GetNotificationType(OsdkOnlinePassComponentNotification componentNotification)
        {
            return OsdkOnlinePassComponentBase.GetNotificationType(componentNotification);
        }
    }

    public class Client : BlazeClientComponent<OsdkOnlinePassComponentCommand, OsdkOnlinePassComponentNotification, Blaze3RpcError>
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public Client(BlazeClientConnection connection) : base(OsdkOnlinePassComponentBase.Id, OsdkOnlinePassComponentBase.Name)
        {
            Connection = connection;
            if (!Connection.Config.AddComponent(this))
                throw new InvalidOperationException($"A component with Id({Id}) has already been created for the connection.");
        }

        private BlazeClientConnection Connection { get; }


        public FetchGatesResponse FetchGates(NullStruct request)
        {
            return Connection.SendRequest<NullStruct, FetchGatesResponse, NullStruct>(this, (ushort)OsdkOnlinePassComponentCommand.fetchGates, request);
        }

        public Task<FetchGatesResponse> FetchGatesAsync(NullStruct request)
        {
            return Connection.SendRequestAsync<NullStruct, FetchGatesResponse, NullStruct>(this, (ushort)OsdkOnlinePassComponentCommand.fetchGates, request);
        }


        public override Type GetCommandRequestType(OsdkOnlinePassComponentCommand componentCommand)
        {
            return OsdkOnlinePassComponentBase.GetCommandRequestType(componentCommand);
        }

        public override Type GetCommandResponseType(OsdkOnlinePassComponentCommand componentCommand)
        {
            return OsdkOnlinePassComponentBase.GetCommandResponseType(componentCommand);
        }

        public override Type GetCommandErrorResponseType(OsdkOnlinePassComponentCommand componentCommand)
        {
            return OsdkOnlinePassComponentBase.GetCommandErrorResponseType(componentCommand);
        }

        public override Type GetNotificationType(OsdkOnlinePassComponentNotification componentNotification)
        {
            return OsdkOnlinePassComponentBase.GetNotificationType(componentNotification);
        }
    }
    
}