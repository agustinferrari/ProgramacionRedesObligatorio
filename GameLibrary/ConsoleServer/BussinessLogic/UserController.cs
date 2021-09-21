using ConsoleServer.Domain;
using ConsoleServer.Utils.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.BussinessLogic
{
    public class UserController
    {
        private static readonly object padlock = new object();
        private List<User> _users;
        private GameController _gameController;
        private static UserController _instance = null;

        private UserController()
        {
            _users = new List<User>();
            _gameController = GameController.Instance;
        }

        public static UserController Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new UserController();
                    }
                    return _instance;
                }
            }
        }

        public void TryAddUser(string name)
        {
            User newUser = new User { Name = name };
            if (!_users.Contains(newUser))
                _users.Add(newUser);
        }

        public void BuyGame(string username, string gameName)
        {
            Game game = _gameController.GetGame(gameName);
            if (_users.Exists(user => user.Name == username))
            {
                User user = _users.Find(user => user.Name == username);
                user.AddGame(game);
            }
            else
            {
                throw new InvalidUsernameException();
            }
        }

        public User GetUser(string username)
        {
            foreach (User user in _users)
            {
                if (user.Name == username)
                    return user;
            }
            throw new InvalidUsernameException();
        }

        public string ListOwnedGameByUser (string username)
        {
            User user = GetUser(username);
            return GameListToString(user);
        }

        private string GameListToString(User user)
        {
            string result = "";
            List<Game> games = user.OwnedGames;
            if (games == null) 
                return "";
            for (int i = 0; i < games.Count; i++)
            {
                Game game = games[i];
                result += game.Name;
                if (i < games.Count - 1)
                    result += "\n";
            }
            return result;
        }

        public void DeleteOwnedGame(string userName, string gameName)
        {
            bool searchingGame = true;
            User user = GetUser(userName);
            List<Game> games = user.OwnedGames;
            if (games == null)
                searchingGame = false;
            for (int i = 0; searchingGame && i < games.Count; i++)
            {
                Game game = games[i];
                if (game.Name.ToLower() == gameName.ToLower())
                {
                    games.RemoveAt(i);
                    searchingGame = false;
                }
            }
            if (searchingGame)
                throw new GameDoesNotExistOnLibraryExcpetion();
        }
    }
}
