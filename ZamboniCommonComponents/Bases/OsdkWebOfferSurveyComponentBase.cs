using Blaze3SDK;
using BlazeCommon;
using NLog;
using ZamboniCommonComponents.Requests;
using ZamboniCommonComponents.Responses;
using ZamboniCommonComponents.Structs;

namespace ZamboniCommonComponents.Bases;

public static class OsdkWebOfferSurveyComponentBase
{
    public enum OsdkWebOfferSurveyComponentCommand : ushort
    {
        getSurveyList = 112
    }

    public enum OsdkWebOfferSurveyComponentNotification : ushort
    {
        NotifyVotingIssue = 102,
        NotifyVotingIssueDeleteRequest = 103,
        NotifyVotingIssue2 = 104,
        NotifyVoteCastNotificationType = 105
    }

    public const ushort Id = 2251;
    public const string Name = "OsdkWebOfferSurveyComponent";

    public static Type GetCommandRequestType(OsdkWebOfferSurveyComponentCommand command)
    {
        return command switch
        {
            OsdkWebOfferSurveyComponentCommand.getSurveyList => typeof(GetSurveyListArgs),
            _ => typeof(NullStruct)
        };
    }

    public static Type GetCommandResponseType(OsdkWebOfferSurveyComponentCommand command)
    {
        return command switch
        {
            OsdkWebOfferSurveyComponentCommand.getSurveyList => typeof(GetSurveyListResponse),
            _ => typeof(NullStruct)
        };
    }

    public static Type GetCommandErrorResponseType(OsdkWebOfferSurveyComponentCommand command)
    {
        return command switch
        {
            OsdkWebOfferSurveyComponentCommand.getSurveyList => typeof(NullStruct),
            _ => typeof(NullStruct)
        };
    }

    public static Type GetNotificationType(OsdkWebOfferSurveyComponentNotification notification)
    {
        return notification switch
        {
            OsdkWebOfferSurveyComponentNotification.NotifyVotingIssue => typeof(VotingIssue),
            OsdkWebOfferSurveyComponentNotification.NotifyVotingIssueDeleteRequest => typeof(NotifyVotingIssueDeleteRequest),
            OsdkWebOfferSurveyComponentNotification.NotifyVotingIssue2 => typeof(VotingIssue),
            OsdkWebOfferSurveyComponentNotification.NotifyVoteCastNotificationType => typeof(VoteCastNotificationType),
            _ => typeof(NullStruct)
        };
    }

    public class Server : BlazeServerComponent<OsdkWebOfferSurveyComponentCommand, OsdkWebOfferSurveyComponentNotification, Blaze3RpcError>
    {
        public Server() : base(OsdkWebOfferSurveyComponentBase.Id, OsdkWebOfferSurveyComponentBase.Name)
        {
        }

        public static Task NotifyVotingIssue(BlazeServerConnection connection, VotingIssue notification, bool waitUntilFree = false)
        {
            return connection.NotifyAsync(OsdkWebOfferSurveyComponentBase.Id, (ushort)OsdkWebOfferSurveyComponentNotification.NotifyVotingIssue, notification, waitUntilFree);
        }

        public static Task NotifyVotingIssueDeleteRequest(BlazeServerConnection connection, NotifyVotingIssueDeleteRequest notification, bool waitUntilFree = false)
        {
            return connection.NotifyAsync(OsdkWebOfferSurveyComponentBase.Id, (ushort)OsdkWebOfferSurveyComponentNotification.NotifyVotingIssueDeleteRequest, notification, waitUntilFree);
        }

        public static Task NotifyVotingIssue2(BlazeServerConnection connection, VotingIssue notification, bool waitUntilFree = false)
        {
            return connection.NotifyAsync(OsdkWebOfferSurveyComponentBase.Id, (ushort)OsdkWebOfferSurveyComponentNotification.NotifyVotingIssue2, notification, waitUntilFree);
        }

        public static Task NotifyVoteCastNotificationType(BlazeServerConnection connection, VoteCastNotificationType notification, bool waitUntilFree = false)
        {
            return connection.NotifyAsync(OsdkWebOfferSurveyComponentBase.Id, (ushort)OsdkWebOfferSurveyComponentNotification.NotifyVoteCastNotificationType, notification, waitUntilFree);
        }

        [BlazeCommand((ushort)OsdkWebOfferSurveyComponentCommand.getSurveyList)]
        public virtual Task<GetSurveyListResponse> GetSurveyListAsync(GetSurveyListArgs request, BlazeRpcContext context)
        {
            throw new BlazeRpcException(Blaze3RpcError.ERR_COMMAND_NOT_FOUND);
        }

        public override Type GetCommandRequestType(OsdkWebOfferSurveyComponentCommand command)
        {
            return OsdkWebOfferSurveyComponentBase.GetCommandRequestType(command);
        }

        public override Type GetCommandResponseType(OsdkWebOfferSurveyComponentCommand command)
        {
            return OsdkWebOfferSurveyComponentBase.GetCommandResponseType(command);
        }

        public override Type GetCommandErrorResponseType(OsdkWebOfferSurveyComponentCommand command)
        {
            return OsdkWebOfferSurveyComponentBase.GetCommandErrorResponseType(command);
        }

        public override Type GetNotificationType(OsdkWebOfferSurveyComponentNotification notification)
        {
            return OsdkWebOfferSurveyComponentBase.GetNotificationType(notification);
        }
    }

    public class Client : BlazeClientComponent<OsdkWebOfferSurveyComponentCommand, OsdkWebOfferSurveyComponentNotification, Blaze3RpcError>
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public Client(BlazeClientConnection connection) : base(OsdkWebOfferSurveyComponentBase.Id, OsdkWebOfferSurveyComponentBase.Name)
        {
            Connection = connection;
            if (!Connection.Config.AddComponent(this))
                throw new InvalidOperationException($"A component with Id({Id}) has already been created for the connection.");
        }

        private BlazeClientConnection Connection { get; }


        public GetSurveyListResponse GetSurveyList(GetSurveyListArgs request)
        {
            return Connection.SendRequest<GetSurveyListArgs, GetSurveyListResponse, NullStruct>(this, (ushort)OsdkWebOfferSurveyComponentCommand.getSurveyList, request);
        }

        public Task<GetSurveyListResponse> GetSurveyListAsync(GetSurveyListArgs request)
        {
            return Connection.SendRequestAsync<GetSurveyListArgs, GetSurveyListResponse, NullStruct>(this, (ushort)OsdkWebOfferSurveyComponentCommand.getSurveyList, request);
        }


        public override Type GetCommandRequestType(OsdkWebOfferSurveyComponentCommand command)
        {
            return OsdkWebOfferSurveyComponentBase.GetCommandRequestType(command);
        }

        public override Type GetCommandResponseType(OsdkWebOfferSurveyComponentCommand command)
        {
            return OsdkWebOfferSurveyComponentBase.GetCommandResponseType(command);
        }

        public override Type GetCommandErrorResponseType(OsdkWebOfferSurveyComponentCommand command)
        {
            return OsdkWebOfferSurveyComponentBase.GetCommandErrorResponseType(command);
        }

        public override Type GetNotificationType(OsdkWebOfferSurveyComponentNotification notification)
        {
            return OsdkWebOfferSurveyComponentBase.GetNotificationType(notification);
        }
    }
}