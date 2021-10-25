using System.Threading.Tasks;
using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;

namespace ConsoleServer.Logic.Interfaces
{
    public interface IClientHandler
    {
        public static IClientHandler Instance;

        public bool IsClientLogged(string userName);

        public bool IsSocketInUse(INetworkStreamHandler networkStreamHandler);

        public string GetUsername(INetworkStreamHandler networkStreamHandler);

        public void AddClient(INetworkStreamHandler networkStreamHandler, string userName);

        public void RemoveClient(INetworkStreamHandler networkStreamHandler);

        public Task HandleClient(INetworkStreamHandler clientNetworkStreamHandler);

    }

}
