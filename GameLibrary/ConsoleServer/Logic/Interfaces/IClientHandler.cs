using Common.NetworkUtils;


namespace ConsoleServer.Logic.Interfaces
{
    public interface IClientHandler
    {
        public static IClientHandler Instance;

        public bool IsClientLogged(string userName);

        public bool IsSocketInUse(SocketHandler socketHandler);

        public string GetUsername(SocketHandler socketHandler);

        public void AddClient(SocketHandler socketHandler, string userName);

        public void RemoveClient(SocketHandler socketHandler);

        public void HandleClient(SocketHandler clientSocketHandler);

    }

}
