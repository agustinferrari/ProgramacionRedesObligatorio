using System.Threading.Tasks;
using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;

namespace ConsoleServer.Logic.Interfaces
{
    public interface IClientHandler
    {
        public static IClientHandler Instance;

        public bool IsClientLogged(string userName);

        public bool IsSocketInUse(ISocketHandler socketHandler);

        public string GetUsername(ISocketHandler socketHandler);

        public void AddClient(ISocketHandler socketHandler, string userName);

        public void RemoveClient(ISocketHandler socketHandler);

        public Task HandleClient(ISocketHandler clientSocketHandler);

    }

}
