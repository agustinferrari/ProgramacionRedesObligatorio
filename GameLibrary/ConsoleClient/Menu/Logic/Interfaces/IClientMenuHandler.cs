using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;

namespace ConsoleClient.Menu.Logic.Interfaces
{
    public interface IClientMenuHandler
    {

        public void LoadMainMenu(INetworkStreamHandler clientNetworkStream);

        public void LoadLoggedUserMenu(INetworkStreamHandler clientNetworkStream);

        public bool ValidateNotEmptyFields(string data);
    }
}
