using Common.NetworkUtils;


namespace ConsoleClient.Menu.Logic.Interfaces
{
    public interface IClientMenuHandler
    {
        public static IClientMenuHandler Instance;

        public void LoadMainMenu(SocketHandler clientSocket);

        public void LoadLoggedUserMenu(SocketHandler clientSocket);

        public string SendMessageAndRecieveResponse(SocketHandler clientSocket, int command, string messageToSend);

        public string RecieveResponse(SocketHandler clientSocket);

        public void HandleListGamesFiltered(SocketHandler clientSocket);

        public bool ValidateNotEmptyFields(string data);
    }
}
