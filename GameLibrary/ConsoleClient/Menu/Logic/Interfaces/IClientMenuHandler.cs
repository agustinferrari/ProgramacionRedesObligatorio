using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;

namespace ConsoleClient.Menu.Logic.Interfaces
{
    public interface IClientMenuHandler
    {

        public void LoadMainMenu(ISocketHandler clientSocket);

        public void LoadLoggedUserMenu(ISocketHandler clientSocket);

        public bool ValidateNotEmptyFields(string data);
    }
}
