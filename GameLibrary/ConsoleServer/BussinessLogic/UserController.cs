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
            Game game = _gameController.GetOneGame(gameName);
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
    }
}
