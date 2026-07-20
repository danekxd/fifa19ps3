using Blaze2SDK.Blaze.Clubs;
using BlazeCommon;
using NLog;

namespace Blaze2SDK.Components
{
    public static class ClubsComponentBase
    {
        public const ushort Id = 11;
        public const string Name = "ClubsComponent";
        
        public class Server : BlazeServerComponent<ClubsComponentCommand, ClubsComponentNotification, Blaze2RpcError>
        {
            public Server() : base(ClubsComponentBase.Id, ClubsComponentBase.Name)
            {
                
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.createClub)]
            public virtual Task<CreateClubResponse> CreateClubAsync(CreateClubRequest request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.getClubs)]
            public virtual Task<GetClubsResponse> GetClubsAsync(GetClubsRequest request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.findClubs)]
            public virtual Task<FindClubsResponse> FindClubsAsync(FindClubsRequest request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.removeMember)]
            public virtual Task<NullStruct> RemoveMemberAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.sendInvitation)]
            public virtual Task<NullStruct> SendInvitationAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.getInvitations)]
            public virtual Task<NullStruct> GetInvitationsAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.revokeInvitation)]
            public virtual Task<NullStruct> RevokeInvitationAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.acceptInvitation)]
            public virtual Task<NullStruct> AcceptInvitationAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.declineInvitation)]
            public virtual Task<NullStruct> DeclineInvitationAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.getMembers)]
            public virtual Task<GetMembersResponse> GetMembersAsync(GetMembersRequest request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.promoteToGM)]
            public virtual Task<NullStruct> PromoteToGMAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.updateClubSettings)]
            public virtual Task<NullStruct> UpdateClubSettingsAsync(UpdateClubSettingsRequest request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.postNews)]
            public virtual Task<NullStruct> PostNewsAsync(PostNewsRequest request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.getNews)]
            public virtual Task<GetNewsResponse> GetNewsAsync(GetNewsRequest request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.setNewsItemHidden)]
            public virtual Task<NullStruct> SetNewsItemHiddenAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.setMetadata)]
            public virtual Task<NullStruct> SetMetadataAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.getClubsComponentSettings)]
            public virtual Task<ClubsComponentSettings> GetClubsComponentSettingsAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.getClubMembershipForUsers)]
            public virtual Task<NullStruct> GetClubMembershipForUsersAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.sendPetition)]
            public virtual Task<NullStruct> SendPetitionAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.getPetitions)]
            public virtual Task<GetPetitionsResponse> GetPetitionsAsync(GetPetitionsRequest request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.acceptPetition)]
            public virtual Task<NullStruct> AcceptPetitionAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.declinePetition)]
            public virtual Task<NullStruct> DeclinePetitionAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.revokePetition)]
            public virtual Task<NullStruct> RevokePetitionAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.joinClub)]
            public virtual Task<NullStruct> JoinClubAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.getClubRecordbook)]
            public virtual Task<NullStruct> GetClubRecordbookAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.resetClubRecords)]
            public virtual Task<NullStruct> ResetClubRecordsAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.updateMemberOnlineStatus)]
            public virtual Task<NullStruct> UpdateMemberOnlineStatusAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.getClubAwards)]
            public virtual Task<GetClubAwardsResponse> GetClubAwardsAsync(GetClubAwardsRequest request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.updateMemberMetadata)]
            public virtual Task<NullStruct> UpdateMemberMetadataAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.findClubsAsync)]
            public virtual Task<FindClubsResponse> FindClubsAsyncAsync(FindClubsRequest request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.listRivals)]
            public virtual Task<NullStruct> ListRivalsAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.getClubTickerMessages)]
            public virtual Task<NullStruct> GetClubTickerMessagesAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.setClubTickerMessagesSubscription)]
            public virtual Task<NullStruct> SetClubTickerMessagesSubscriptionAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.changeClubStrings)]
            public virtual Task<NullStruct> ChangeClubStringsAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.countMessages)]
            public virtual Task<NullStruct> CountMessagesAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.getClubBans)]
            public virtual Task<NullStruct> GetClubBansAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.getUserBans)]
            public virtual Task<NullStruct> GetUserBansAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.banMember)]
            public virtual Task<NullStruct> BanMemberAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.unbanMember)]
            public virtual Task<NullStruct> UnbanMemberAsync(NullStruct request, BlazeRpcContext context)
            {
                throw new BlazeRpcException(Blaze2RpcError.ERR_COMMAND_NOT_FOUND);
            }
            
            
            public static Task NotifyFindClubsAsyncNotificationAsync(BlazeServerConnection connection, FindClubsAsyncResult notification, bool waitUntilFree = false)
            {
                return connection.NotifyAsync(ClubsComponentBase.Id, (ushort)ClubsComponentNotification.FindClubsAsyncNotification, notification, waitUntilFree);
            }
            
            public static Task NotifyNewClubTickerMessageNotificationAsync(BlazeServerConnection connection, ClubTickerMessage notification, bool waitUntilFree = false)
            {
                return connection.NotifyAsync(ClubsComponentBase.Id, (ushort)ClubsComponentNotification.NewClubTickerMessageNotification, notification, waitUntilFree);
            }
            
            public override Type GetCommandRequestType(ClubsComponentCommand command) => ClubsComponentBase.GetCommandRequestType(command);
            public override Type GetCommandResponseType(ClubsComponentCommand command) => ClubsComponentBase.GetCommandResponseType(command);
            public override Type GetCommandErrorResponseType(ClubsComponentCommand command) => ClubsComponentBase.GetCommandErrorResponseType(command);
            public override Type GetNotificationType(ClubsComponentNotification notification) => ClubsComponentBase.GetNotificationType(notification);
            
        }
        
        public class Client : BlazeClientComponent<ClubsComponentCommand, ClubsComponentNotification, Blaze2RpcError>
        {
            BlazeClientConnection Connection { get; }
            private static Logger _logger = LogManager.GetCurrentClassLogger();
            
            public Client(BlazeClientConnection connection) : base(ClubsComponentBase.Id, ClubsComponentBase.Name)
            {
                Connection = connection;
                if (!Connection.Config.AddComponent(this))
                    throw new InvalidOperationException($"A component with Id({Id}) has already been created for the connection.");
            }
            
            
            public CreateClubResponse CreateClub(CreateClubRequest request)
            {
                return Connection.SendRequest<CreateClubRequest, CreateClubResponse, NullStruct>(this, (ushort)ClubsComponentCommand.createClub, request);
            }
            public Task<CreateClubResponse> CreateClubAsync(CreateClubRequest request)
            {
                return Connection.SendRequestAsync<CreateClubRequest, CreateClubResponse, NullStruct>(this, (ushort)ClubsComponentCommand.createClub, request);
            }
            
            public GetClubsResponse GetClubs(GetClubsRequest request)
            {
                return Connection.SendRequest<GetClubsRequest, GetClubsResponse, NullStruct>(this, (ushort)ClubsComponentCommand.getClubs, request);
            }
            public Task<GetClubsResponse> GetClubsAsync(GetClubsRequest request)
            {
                return Connection.SendRequestAsync<GetClubsRequest, GetClubsResponse, NullStruct>(this, (ushort)ClubsComponentCommand.getClubs, request);
            }
            
            public FindClubsResponse FindClubs(FindClubsRequest request)
            {
                return Connection.SendRequest<FindClubsRequest, FindClubsResponse, NullStruct>(this, (ushort)ClubsComponentCommand.findClubs, request);
            }
            public Task<FindClubsResponse> FindClubsAsync(FindClubsRequest request)
            {
                return Connection.SendRequestAsync<FindClubsRequest, FindClubsResponse, NullStruct>(this, (ushort)ClubsComponentCommand.findClubs, request);
            }
            
            public NullStruct RemoveMember()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.removeMember, new NullStruct());
            }
            public Task<NullStruct> RemoveMemberAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.removeMember, new NullStruct());
            }
            
            public NullStruct SendInvitation()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.sendInvitation, new NullStruct());
            }
            public Task<NullStruct> SendInvitationAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.sendInvitation, new NullStruct());
            }
            
            public NullStruct GetInvitations()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.getInvitations, new NullStruct());
            }
            public Task<NullStruct> GetInvitationsAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.getInvitations, new NullStruct());
            }
            
            public NullStruct RevokeInvitation()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.revokeInvitation, new NullStruct());
            }
            public Task<NullStruct> RevokeInvitationAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.revokeInvitation, new NullStruct());
            }
            
            public NullStruct AcceptInvitation()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.acceptInvitation, new NullStruct());
            }
            public Task<NullStruct> AcceptInvitationAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.acceptInvitation, new NullStruct());
            }
            
            public NullStruct DeclineInvitation()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.declineInvitation, new NullStruct());
            }
            public Task<NullStruct> DeclineInvitationAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.declineInvitation, new NullStruct());
            }
            
            public GetMembersResponse GetMembers(GetMembersRequest request)
            {
                return Connection.SendRequest<GetMembersRequest, GetMembersResponse, NullStruct>(this, (ushort)ClubsComponentCommand.getMembers, request);
            }
            public Task<GetMembersResponse> GetMembersAsync(GetMembersRequest request)
            {
                return Connection.SendRequestAsync<GetMembersRequest, GetMembersResponse, NullStruct>(this, (ushort)ClubsComponentCommand.getMembers, request);
            }
            
            public NullStruct PromoteToGM()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.promoteToGM, new NullStruct());
            }
            public Task<NullStruct> PromoteToGMAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.promoteToGM, new NullStruct());
            }
            
            public NullStruct UpdateClubSettings(UpdateClubSettingsRequest request)
            {
                return Connection.SendRequest<UpdateClubSettingsRequest, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.updateClubSettings, request);
            }
            public Task<NullStruct> UpdateClubSettingsAsync(UpdateClubSettingsRequest request)
            {
                return Connection.SendRequestAsync<UpdateClubSettingsRequest, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.updateClubSettings, request);
            }
            
            public NullStruct PostNews(PostNewsRequest request)
            {
                return Connection.SendRequest<PostNewsRequest, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.postNews, request);
            }
            public Task<NullStruct> PostNewsAsync(PostNewsRequest request)
            {
                return Connection.SendRequestAsync<PostNewsRequest, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.postNews, request);
            }
            
            public GetNewsResponse GetNews(GetNewsRequest request)
            {
                return Connection.SendRequest<GetNewsRequest, GetNewsResponse, NullStruct>(this, (ushort)ClubsComponentCommand.getNews, request);
            }
            public Task<GetNewsResponse> GetNewsAsync(GetNewsRequest request)
            {
                return Connection.SendRequestAsync<GetNewsRequest, GetNewsResponse, NullStruct>(this, (ushort)ClubsComponentCommand.getNews, request);
            }
            
            public NullStruct SetNewsItemHidden()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.setNewsItemHidden, new NullStruct());
            }
            public Task<NullStruct> SetNewsItemHiddenAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.setNewsItemHidden, new NullStruct());
            }
            
            public NullStruct SetMetadata()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.setMetadata, new NullStruct());
            }
            public Task<NullStruct> SetMetadataAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.setMetadata, new NullStruct());
            }
            
            public ClubsComponentSettings GetClubsComponentSettings()
            {
                return Connection.SendRequest<NullStruct, ClubsComponentSettings, NullStruct>(this, (ushort)ClubsComponentCommand.getClubsComponentSettings, new NullStruct());
            }
            public Task<ClubsComponentSettings> GetClubsComponentSettingsAsync()
            {
                return Connection.SendRequestAsync<NullStruct, ClubsComponentSettings, NullStruct>(this, (ushort)ClubsComponentCommand.getClubsComponentSettings, new NullStruct());
            }
            
            public NullStruct GetClubMembershipForUsers()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.getClubMembershipForUsers, new NullStruct());
            }
            public Task<NullStruct> GetClubMembershipForUsersAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.getClubMembershipForUsers, new NullStruct());
            }
            
            public NullStruct SendPetition()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.sendPetition, new NullStruct());
            }
            public Task<NullStruct> SendPetitionAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.sendPetition, new NullStruct());
            }
            
            public GetPetitionsResponse GetPetitions(GetPetitionsRequest request)
            {
                return Connection.SendRequest<GetPetitionsRequest, GetPetitionsResponse, NullStruct>(this, (ushort)ClubsComponentCommand.getPetitions, request);
            }
            public Task<GetPetitionsResponse> GetPetitionsAsync(GetPetitionsRequest request)
            {
                return Connection.SendRequestAsync<GetPetitionsRequest, GetPetitionsResponse, NullStruct>(this, (ushort)ClubsComponentCommand.getPetitions, request);
            }
            
            public NullStruct AcceptPetition()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.acceptPetition, new NullStruct());
            }
            public Task<NullStruct> AcceptPetitionAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.acceptPetition, new NullStruct());
            }
            
            public NullStruct DeclinePetition()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.declinePetition, new NullStruct());
            }
            public Task<NullStruct> DeclinePetitionAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.declinePetition, new NullStruct());
            }
            
            public NullStruct RevokePetition()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.revokePetition, new NullStruct());
            }
            public Task<NullStruct> RevokePetitionAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.revokePetition, new NullStruct());
            }
            
            public NullStruct JoinClub()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.joinClub, new NullStruct());
            }
            public Task<NullStruct> JoinClubAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.joinClub, new NullStruct());
            }
            
            public NullStruct GetClubRecordbook()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.getClubRecordbook, new NullStruct());
            }
            public Task<NullStruct> GetClubRecordbookAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.getClubRecordbook, new NullStruct());
            }
            
            public NullStruct ResetClubRecords()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.resetClubRecords, new NullStruct());
            }
            public Task<NullStruct> ResetClubRecordsAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.resetClubRecords, new NullStruct());
            }
            
            public NullStruct UpdateMemberOnlineStatus()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.updateMemberOnlineStatus, new NullStruct());
            }
            public Task<NullStruct> UpdateMemberOnlineStatusAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.updateMemberOnlineStatus, new NullStruct());
            }
            
            public GetClubAwardsResponse GetClubAwards(GetClubAwardsRequest request)
            {
                return Connection.SendRequest<GetClubAwardsRequest, GetClubAwardsResponse, NullStruct>(this, (ushort)ClubsComponentCommand.getClubAwards, request);
            }
            public Task<GetClubAwardsResponse> GetClubAwardsAsync(GetClubAwardsRequest request)
            {
                return Connection.SendRequestAsync<GetClubAwardsRequest, GetClubAwardsResponse, NullStruct>(this, (ushort)ClubsComponentCommand.getClubAwards, request);
            }
            
            public NullStruct UpdateMemberMetadata()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.updateMemberMetadata, new NullStruct());
            }
            public Task<NullStruct> UpdateMemberMetadataAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.updateMemberMetadata, new NullStruct());
            }
            
            public NullStruct FindClubsAsynchronously()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.findClubsAsync, new NullStruct());
            }
            public Task<NullStruct> FindClubsAsynchronouslyAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.findClubsAsync, new NullStruct());
            }
            
            public NullStruct ListRivals()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.listRivals, new NullStruct());
            }
            public Task<NullStruct> ListRivalsAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.listRivals, new NullStruct());
            }
            
            public NullStruct GetClubTickerMessages()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.getClubTickerMessages, new NullStruct());
            }
            public Task<NullStruct> GetClubTickerMessagesAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.getClubTickerMessages, new NullStruct());
            }
            
            public NullStruct SetClubTickerMessagesSubscription()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.setClubTickerMessagesSubscription, new NullStruct());
            }
            public Task<NullStruct> SetClubTickerMessagesSubscriptionAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.setClubTickerMessagesSubscription, new NullStruct());
            }
            
            public NullStruct ChangeClubStrings()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.changeClubStrings, new NullStruct());
            }
            public Task<NullStruct> ChangeClubStringsAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.changeClubStrings, new NullStruct());
            }
            
            public NullStruct CountMessages()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.countMessages, new NullStruct());
            }
            public Task<NullStruct> CountMessagesAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.countMessages, new NullStruct());
            }
            
            public NullStruct GetClubBans()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.getClubBans, new NullStruct());
            }
            public Task<NullStruct> GetClubBansAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.getClubBans, new NullStruct());
            }
            
            public NullStruct GetUserBans()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.getUserBans, new NullStruct());
            }
            public Task<NullStruct> GetUserBansAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.getUserBans, new NullStruct());
            }
            
            public NullStruct BanMember()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.banMember, new NullStruct());
            }
            public Task<NullStruct> BanMemberAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.banMember, new NullStruct());
            }
            
            public NullStruct UnbanMember()
            {
                return Connection.SendRequest<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.unbanMember, new NullStruct());
            }
            public Task<NullStruct> UnbanMemberAsync()
            {
                return Connection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.unbanMember, new NullStruct());
            }
            
            
            [BlazeNotification((ushort)ClubsComponentNotification.FindClubsAsyncNotification)]
            public virtual Task OnFindClubsAsyncNotificationAsync(FindClubsAsyncResult notification)
            {
                _logger.Warn($"{GetType().FullName}: OnFindClubsAsyncNotificationAsync NOT IMPLEMENTED!");
                return Task.CompletedTask;
            }
            
            [BlazeNotification((ushort)ClubsComponentNotification.NewClubTickerMessageNotification)]
            public virtual Task OnNewClubTickerMessageNotificationAsync(ClubTickerMessage notification)
            {
                _logger.Warn($"{GetType().FullName}: OnNewClubTickerMessageNotificationAsync NOT IMPLEMENTED!");
                return Task.CompletedTask;
            }
            
            public override Type GetCommandRequestType(ClubsComponentCommand command) => ClubsComponentBase.GetCommandRequestType(command);
            public override Type GetCommandResponseType(ClubsComponentCommand command) => ClubsComponentBase.GetCommandResponseType(command);
            public override Type GetCommandErrorResponseType(ClubsComponentCommand command) => ClubsComponentBase.GetCommandErrorResponseType(command);
            public override Type GetNotificationType(ClubsComponentNotification notification) => ClubsComponentBase.GetNotificationType(notification);
            
        }
        
        public class Proxy : BlazeProxyComponent<ClubsComponentCommand, ClubsComponentNotification, Blaze2RpcError>
        {
            public Proxy() : base(ClubsComponentBase.Id, ClubsComponentBase.Name)
            {
                
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.createClub)]
            public virtual Task<NullStruct> CreateClubAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.createClub, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.getClubs)]
            public virtual Task<NullStruct> GetClubsAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.getClubs, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.findClubs)]
            public virtual Task<FindClubsResponse> FindClubsAsync(FindClubsRequest request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<FindClubsRequest, FindClubsResponse, NullStruct>(this, (ushort)ClubsComponentCommand.findClubs, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.removeMember)]
            public virtual Task<NullStruct> RemoveMemberAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.removeMember, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.sendInvitation)]
            public virtual Task<NullStruct> SendInvitationAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.sendInvitation, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.getInvitations)]
            public virtual Task<NullStruct> GetInvitationsAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.getInvitations, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.revokeInvitation)]
            public virtual Task<NullStruct> RevokeInvitationAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.revokeInvitation, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.acceptInvitation)]
            public virtual Task<NullStruct> AcceptInvitationAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.acceptInvitation, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.declineInvitation)]
            public virtual Task<NullStruct> DeclineInvitationAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.declineInvitation, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.getMembers)]
            public virtual Task<NullStruct> GetMembersAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.getMembers, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.promoteToGM)]
            public virtual Task<NullStruct> PromoteToGMAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.promoteToGM, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.updateClubSettings)]
            public virtual Task<NullStruct> UpdateClubSettingsAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.updateClubSettings, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.postNews)]
            public virtual Task<NullStruct> PostNewsAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.postNews, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.getNews)]
            public virtual Task<NullStruct> GetNewsAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.getNews, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.setNewsItemHidden)]
            public virtual Task<NullStruct> SetNewsItemHiddenAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.setNewsItemHidden, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.setMetadata)]
            public virtual Task<NullStruct> SetMetadataAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.setMetadata, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.getClubsComponentSettings)]
            public virtual Task<ClubsComponentSettings> GetClubsComponentSettingsAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, ClubsComponentSettings, NullStruct>(this, (ushort)ClubsComponentCommand.getClubsComponentSettings, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.getClubMembershipForUsers)]
            public virtual Task<NullStruct> GetClubMembershipForUsersAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.getClubMembershipForUsers, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.sendPetition)]
            public virtual Task<NullStruct> SendPetitionAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.sendPetition, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.getPetitions)]
            public virtual Task<NullStruct> GetPetitionsAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.getPetitions, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.acceptPetition)]
            public virtual Task<NullStruct> AcceptPetitionAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.acceptPetition, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.declinePetition)]
            public virtual Task<NullStruct> DeclinePetitionAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.declinePetition, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.revokePetition)]
            public virtual Task<NullStruct> RevokePetitionAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.revokePetition, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.joinClub)]
            public virtual Task<NullStruct> JoinClubAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.joinClub, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.getClubRecordbook)]
            public virtual Task<NullStruct> GetClubRecordbookAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.getClubRecordbook, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.resetClubRecords)]
            public virtual Task<NullStruct> ResetClubRecordsAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.resetClubRecords, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.updateMemberOnlineStatus)]
            public virtual Task<NullStruct> UpdateMemberOnlineStatusAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.updateMemberOnlineStatus, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.getClubAwards)]
            public virtual Task<NullStruct> GetClubAwardsAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.getClubAwards, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.updateMemberMetadata)]
            public virtual Task<NullStruct> UpdateMemberMetadataAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.updateMemberMetadata, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.findClubsAsync)]
            public virtual Task<FindClubsResponse> FindClubsAsyncAsync(FindClubsRequest request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<FindClubsRequest, FindClubsResponse, NullStruct>(this, (ushort)ClubsComponentCommand.findClubsAsync, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.listRivals)]
            public virtual Task<NullStruct> ListRivalsAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.listRivals, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.getClubTickerMessages)]
            public virtual Task<NullStruct> GetClubTickerMessagesAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.getClubTickerMessages, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.setClubTickerMessagesSubscription)]
            public virtual Task<NullStruct> SetClubTickerMessagesSubscriptionAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.setClubTickerMessagesSubscription, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.changeClubStrings)]
            public virtual Task<NullStruct> ChangeClubStringsAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.changeClubStrings, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.countMessages)]
            public virtual Task<NullStruct> CountMessagesAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.countMessages, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.getClubBans)]
            public virtual Task<NullStruct> GetClubBansAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.getClubBans, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.getUserBans)]
            public virtual Task<NullStruct> GetUserBansAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.getUserBans, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.banMember)]
            public virtual Task<NullStruct> BanMemberAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.banMember, request);
            }
            
            [BlazeCommand((ushort)ClubsComponentCommand.unbanMember)]
            public virtual Task<NullStruct> UnbanMemberAsync(NullStruct request, BlazeProxyContext context)
            {
                return context.ClientConnection.SendRequestAsync<NullStruct, NullStruct, NullStruct>(this, (ushort)ClubsComponentCommand.unbanMember, request);
            }
            
            
            [BlazeNotification((ushort)ClubsComponentNotification.FindClubsAsyncNotification)]
            public virtual Task<FindClubsAsyncResult> OnFindClubsAsyncNotificationAsync(FindClubsAsyncResult notification)
            {
                return Task.FromResult(notification);
            }
            
            [BlazeNotification((ushort)ClubsComponentNotification.NewClubTickerMessageNotification)]
            public virtual Task<ClubTickerMessage> OnNewClubTickerMessageNotificationAsync(ClubTickerMessage notification)
            {
                return Task.FromResult(notification);
            }
            
            public override Type GetCommandRequestType(ClubsComponentCommand command) => ClubsComponentBase.GetCommandRequestType(command);
            public override Type GetCommandResponseType(ClubsComponentCommand command) => ClubsComponentBase.GetCommandResponseType(command);
            public override Type GetCommandErrorResponseType(ClubsComponentCommand command) => ClubsComponentBase.GetCommandErrorResponseType(command);
            public override Type GetNotificationType(ClubsComponentNotification notification) => ClubsComponentBase.GetNotificationType(notification);
            
        }
        
        public static Type GetCommandRequestType(ClubsComponentCommand command) => command switch
        {
            ClubsComponentCommand.createClub => typeof(CreateClubRequest),
            ClubsComponentCommand.getClubs => typeof(GetClubsRequest),
            ClubsComponentCommand.findClubs => typeof(FindClubsRequest),
            ClubsComponentCommand.removeMember => typeof(NullStruct),
            ClubsComponentCommand.sendInvitation => typeof(NullStruct),
            ClubsComponentCommand.getInvitations => typeof(NullStruct),
            ClubsComponentCommand.revokeInvitation => typeof(NullStruct),
            ClubsComponentCommand.acceptInvitation => typeof(NullStruct),
            ClubsComponentCommand.declineInvitation => typeof(NullStruct),
            ClubsComponentCommand.getMembers => typeof(GetMembersRequest),
            ClubsComponentCommand.promoteToGM => typeof(NullStruct),
            ClubsComponentCommand.updateClubSettings => typeof(UpdateClubSettingsRequest),
            ClubsComponentCommand.postNews => typeof(PostNewsRequest),
            ClubsComponentCommand.getNews => typeof(GetNewsRequest),
            ClubsComponentCommand.setNewsItemHidden => typeof(NullStruct),
            ClubsComponentCommand.setMetadata => typeof(NullStruct),
            ClubsComponentCommand.getClubsComponentSettings => typeof(NullStruct),
            ClubsComponentCommand.getClubMembershipForUsers => typeof(NullStruct),
            ClubsComponentCommand.sendPetition => typeof(NullStruct),
            ClubsComponentCommand.getPetitions => typeof(GetPetitionsRequest),
            ClubsComponentCommand.acceptPetition => typeof(NullStruct),
            ClubsComponentCommand.declinePetition => typeof(NullStruct),
            ClubsComponentCommand.revokePetition => typeof(NullStruct),
            ClubsComponentCommand.joinClub => typeof(NullStruct),
            ClubsComponentCommand.getClubRecordbook => typeof(NullStruct),
            ClubsComponentCommand.resetClubRecords => typeof(NullStruct),
            ClubsComponentCommand.updateMemberOnlineStatus => typeof(NullStruct),
            ClubsComponentCommand.getClubAwards => typeof(GetClubAwardsRequest),
            ClubsComponentCommand.updateMemberMetadata => typeof(NullStruct),
            ClubsComponentCommand.findClubsAsync => typeof(NullStruct),
            ClubsComponentCommand.listRivals => typeof(NullStruct),
            ClubsComponentCommand.getClubTickerMessages => typeof(NullStruct),
            ClubsComponentCommand.setClubTickerMessagesSubscription => typeof(NullStruct),
            ClubsComponentCommand.changeClubStrings => typeof(NullStruct),
            ClubsComponentCommand.countMessages => typeof(NullStruct),
            ClubsComponentCommand.getClubBans => typeof(NullStruct),
            ClubsComponentCommand.getUserBans => typeof(NullStruct),
            ClubsComponentCommand.banMember => typeof(NullStruct),
            ClubsComponentCommand.unbanMember => typeof(NullStruct),
            _ => typeof(NullStruct)
        };
        
        public static Type GetCommandResponseType(ClubsComponentCommand command) => command switch
        {
            ClubsComponentCommand.createClub => typeof(CreateClubResponse),
            ClubsComponentCommand.getClubs => typeof(GetClubsResponse),
            ClubsComponentCommand.findClubs => typeof(FindClubsResponse),
            ClubsComponentCommand.removeMember => typeof(NullStruct),
            ClubsComponentCommand.sendInvitation => typeof(NullStruct),
            ClubsComponentCommand.getInvitations => typeof(NullStruct),
            ClubsComponentCommand.revokeInvitation => typeof(NullStruct),
            ClubsComponentCommand.acceptInvitation => typeof(NullStruct),
            ClubsComponentCommand.declineInvitation => typeof(NullStruct),
            ClubsComponentCommand.getMembers => typeof(GetMembersResponse),
            ClubsComponentCommand.promoteToGM => typeof(NullStruct),
            ClubsComponentCommand.updateClubSettings => typeof(NullStruct),
            ClubsComponentCommand.postNews => typeof(NullStruct),
            ClubsComponentCommand.getNews => typeof(GetNewsResponse),
            ClubsComponentCommand.setNewsItemHidden => typeof(NullStruct),
            ClubsComponentCommand.setMetadata => typeof(NullStruct),
            ClubsComponentCommand.getClubsComponentSettings => typeof(ClubsComponentSettings),
            ClubsComponentCommand.getClubMembershipForUsers => typeof(NullStruct),
            ClubsComponentCommand.sendPetition => typeof(NullStruct),
            ClubsComponentCommand.getPetitions => typeof(GetPetitionsResponse),
            ClubsComponentCommand.acceptPetition => typeof(NullStruct),
            ClubsComponentCommand.declinePetition => typeof(NullStruct),
            ClubsComponentCommand.revokePetition => typeof(NullStruct),
            ClubsComponentCommand.joinClub => typeof(NullStruct),
            ClubsComponentCommand.getClubRecordbook => typeof(NullStruct),
            ClubsComponentCommand.resetClubRecords => typeof(NullStruct),
            ClubsComponentCommand.updateMemberOnlineStatus => typeof(NullStruct),
            ClubsComponentCommand.getClubAwards => typeof(GetClubAwardsResponse),
            ClubsComponentCommand.updateMemberMetadata => typeof(NullStruct),
            ClubsComponentCommand.findClubsAsync => typeof(NullStruct),
            ClubsComponentCommand.listRivals => typeof(NullStruct),
            ClubsComponentCommand.getClubTickerMessages => typeof(NullStruct),
            ClubsComponentCommand.setClubTickerMessagesSubscription => typeof(NullStruct),
            ClubsComponentCommand.changeClubStrings => typeof(NullStruct),
            ClubsComponentCommand.countMessages => typeof(NullStruct),
            ClubsComponentCommand.getClubBans => typeof(NullStruct),
            ClubsComponentCommand.getUserBans => typeof(NullStruct),
            ClubsComponentCommand.banMember => typeof(NullStruct),
            ClubsComponentCommand.unbanMember => typeof(NullStruct),
            _ => typeof(NullStruct)
        };
        
        public static Type GetCommandErrorResponseType(ClubsComponentCommand command) => command switch
        {
            ClubsComponentCommand.createClub => typeof(NullStruct),
            ClubsComponentCommand.getClubs => typeof(NullStruct),
            ClubsComponentCommand.findClubs => typeof(NullStruct),
            ClubsComponentCommand.removeMember => typeof(NullStruct),
            ClubsComponentCommand.sendInvitation => typeof(NullStruct),
            ClubsComponentCommand.getInvitations => typeof(NullStruct),
            ClubsComponentCommand.revokeInvitation => typeof(NullStruct),
            ClubsComponentCommand.acceptInvitation => typeof(NullStruct),
            ClubsComponentCommand.declineInvitation => typeof(NullStruct),
            ClubsComponentCommand.getMembers => typeof(NullStruct),
            ClubsComponentCommand.promoteToGM => typeof(NullStruct),
            ClubsComponentCommand.updateClubSettings => typeof(NullStruct),
            ClubsComponentCommand.postNews => typeof(NullStruct),
            ClubsComponentCommand.getNews => typeof(NullStruct),
            ClubsComponentCommand.setNewsItemHidden => typeof(NullStruct),
            ClubsComponentCommand.setMetadata => typeof(NullStruct),
            ClubsComponentCommand.getClubsComponentSettings => typeof(NullStruct),
            ClubsComponentCommand.getClubMembershipForUsers => typeof(NullStruct),
            ClubsComponentCommand.sendPetition => typeof(NullStruct),
            ClubsComponentCommand.getPetitions => typeof(NullStruct),
            ClubsComponentCommand.acceptPetition => typeof(NullStruct),
            ClubsComponentCommand.declinePetition => typeof(NullStruct),
            ClubsComponentCommand.revokePetition => typeof(NullStruct),
            ClubsComponentCommand.joinClub => typeof(NullStruct),
            ClubsComponentCommand.getClubRecordbook => typeof(NullStruct),
            ClubsComponentCommand.resetClubRecords => typeof(NullStruct),
            ClubsComponentCommand.updateMemberOnlineStatus => typeof(NullStruct),
            ClubsComponentCommand.getClubAwards => typeof(NullStruct),
            ClubsComponentCommand.updateMemberMetadata => typeof(NullStruct),
            ClubsComponentCommand.findClubsAsync => typeof(NullStruct),
            ClubsComponentCommand.listRivals => typeof(NullStruct),
            ClubsComponentCommand.getClubTickerMessages => typeof(NullStruct),
            ClubsComponentCommand.setClubTickerMessagesSubscription => typeof(NullStruct),
            ClubsComponentCommand.changeClubStrings => typeof(NullStruct),
            ClubsComponentCommand.countMessages => typeof(NullStruct),
            ClubsComponentCommand.getClubBans => typeof(NullStruct),
            ClubsComponentCommand.getUserBans => typeof(NullStruct),
            ClubsComponentCommand.banMember => typeof(NullStruct),
            ClubsComponentCommand.unbanMember => typeof(NullStruct),
            _ => typeof(NullStruct)
        };
        
        public static Type GetNotificationType(ClubsComponentNotification notification) => notification switch
        {
            ClubsComponentNotification.FindClubsAsyncNotification => typeof(FindClubsAsyncResult),
            ClubsComponentNotification.NewClubTickerMessageNotification => typeof(ClubTickerMessage),
            _ => typeof(NullStruct)
        };
        
        public enum ClubsComponentCommand : ushort
        {
            createClub = 1100,
            getClubs = 1200,
            findClubs = 1300,
            removeMember = 1400,
            sendInvitation = 1500,
            getInvitations = 1600,
            revokeInvitation = 1700,
            acceptInvitation = 1800,
            declineInvitation = 1900,
            getMembers = 2000,
            promoteToGM = 2100,
            updateClubSettings = 2200,
            postNews = 2300,
            getNews = 2400,
            setNewsItemHidden = 2450,
            setMetadata = 2500,
            getClubsComponentSettings = 2600,
            getClubMembershipForUsers = 2700,
            sendPetition = 2800,
            getPetitions = 2900,
            acceptPetition = 3000,
            declinePetition = 3100,
            revokePetition = 3200,
            joinClub = 3300,
            getClubRecordbook = 3400,
            resetClubRecords = 3410,
            updateMemberOnlineStatus = 3500,
            getClubAwards = 3600,
            updateMemberMetadata = 3700,
            findClubsAsync = 3800,
            listRivals = 3900,
            getClubTickerMessages = 4000,
            setClubTickerMessagesSubscription = 4100,
            changeClubStrings = 4200,
            countMessages = 4300,
            getClubBans = 4400,
            getUserBans = 4500,
            banMember = 4600,
            unbanMember = 4700,
        }
        
        public enum ClubsComponentNotification : ushort
        {
            FindClubsAsyncNotification = 14464,
            NewClubTickerMessageNotification = 15464,
        }
        
    }
}
