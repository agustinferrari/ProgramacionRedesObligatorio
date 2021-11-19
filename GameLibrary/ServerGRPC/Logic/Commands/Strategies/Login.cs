using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System.Threading.Tasks;
using LogsModels;

namespace ServerGRPC.Logic.Commands.Strategies
{
    public class Login : CommandStrategy
    {

        public override async Task<LogGameModel> HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
            LogGameModel log = new LogGameModel(header.ICommand);
            string userName = await clientNetworkStreamHandler.ReceiveString(header.IDataLength);
            log.User = userName;
            string responseMessageResult;
            if (_clientHandler.IsClientLogged(userName))
                responseMessageResult = ResponseConstants.LoginErrorAlreadyLogged;
            else
            {
                if (!_clientHandler.IsSocketInUse(clientNetworkStreamHandler))
                {
                    _clientHandler.AddClient(clientNetworkStreamHandler, userName);
                    _userController.TryAddUser(userName);
                    responseMessageResult = ResponseConstants.LoginSuccess;
                    log.Result = true;
                }
                else
                    responseMessageResult = ResponseConstants.LoginErrorSocketAlreadyInUse;
            }
            await clientNetworkStreamHandler.SendMessage(HeaderConstants.Response, CommandConstants.Login, responseMessageResult);
            return log;
        }
    }
}
