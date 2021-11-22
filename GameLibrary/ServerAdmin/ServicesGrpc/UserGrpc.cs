using System;
using System.Threading.Tasks;
using Common.Settings.Interfaces;
using CommonModels;
using Grpc.Net.Client;
using ServerAdmin.ServicesGrpcInterfaces;

namespace ServerAdmin.ServicesGrpc
{
    public class UserGrpc : IUserGrpc
    {
        private UserProto.UserProtoClient _client;
        private static readonly ISettingsManager SettingsMgr = new SettingsManager();

        public UserGrpc()
        {
            string appContext = SettingsMgr.ReadSetting(ServerConfig.SeverAppContextConfigKey);
            string grpcChannel = SettingsMgr.ReadSetting(ServerConfig.ServerChannelPortConfigKey);
            AppContext.SetSwitch(appContext, true);
            var channel = GrpcChannel.ForAddress(grpcChannel);
            _client = new UserProto.UserProtoClient(channel);
        }

        public async Task<string> GetUsers(string userAsking)
        {
            UsersReply response = await _client.GetUsersAsync(new UsersRequest() { UserAsking = userAsking });
            return response.Response;
        }

        public async Task<string> AddUser(string userAsking, UserModel userToAdd)
        {
            UsersReply response = await _client.AddUserAsync(
                new AddModifyUserRequest() { UserAsking = userAsking, UserToAddModify = userToAdd.Name });
            return response.Response;
        }

        public async Task<string> DeleteUser(string userAsking, string userToDelete)
        {
            UsersReply response = await _client.DeleteUserAsync(
                new DeleteUserRequest() { UserAsking = userAsking, UserToDelete = userToDelete });
            return response.Response;
        }

        public async Task<string> BuyGame(string userAsking, string gameToBuy)
        {
            UsersReply response = await _client.BuyGameAsync(
                new BuyDeleteGameRequest() { UserAsking = userAsking, Game = gameToBuy });
            return response.Response;
        }

        public async Task<string> ModifyUser(string userAsking, UserModel newUserName)
        {
            UsersReply response = await _client.ModifyUserAsync(
                new AddModifyUserRequest() { UserAsking = userAsking, UserToAddModify = newUserName.Name });
            return response.Response;
        }

        public async Task<string> TaskDeleteGameForUser(string userAsking, string gameToDeleteForUser)
        {
            UsersReply response = await _client.DeleteGameForUserAsync(
                new BuyDeleteGameRequest() { UserAsking = userAsking, Game = gameToDeleteForUser });
            return response.Response;
        }

        public async Task<string> DeleteGameForUser(string userAsking, string gameToDeleteForUser)
        {
            UsersReply response = await _client.DeleteGameForUserAsync(
                new BuyDeleteGameRequest() { UserAsking = userAsking, Game = gameToDeleteForUser });
            return response.Response;
        }
    }
}