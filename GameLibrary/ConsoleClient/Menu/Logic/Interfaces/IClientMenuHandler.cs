﻿using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;

namespace ConsoleClient.Menu.Logic.Interfaces
{
    public interface IClientMenuHandler
    {
        public static IClientMenuHandler Instance;

        public void LoadMainMenu(ISocketHandler clientSocket);

        public void LoadLoggedUserMenu(ISocketHandler clientSocket);

        public string SendMessageAndRecieveResponse(ISocketHandler clientSocket, int command, string messageToSend);

        public string RecieveResponse(ISocketHandler clientSocket);

        public void HandleListGamesFiltered(ISocketHandler clientSocket);

        public bool ValidateNotEmptyFields(string data);
        bool ValidateAtLeastOneField(string changes);
    }
}