using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using System.Threading.Tasks;

namespace ConsoleClient.Menu.Logic.Interfaces
{
    public interface IClientMenuHandler
    {

        public Task LoadMainMenu(INetworkStreamHandler clientNetworkStream);

        public Task LoadLoggedUserMenu(INetworkStreamHandler clientNetworkStream);

        public bool ValidateNotEmptyFields(string data);
    }
}
