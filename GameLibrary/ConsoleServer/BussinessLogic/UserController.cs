﻿using ConsoleServer.Domain;
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
            User newUser = new User { Name = name.ToLower() };
            if (!_users.Contains(newUser))
                _users.Add(newUser);
        }

        public void BuyGame(string username, string gameName)
        {
            Game game = _gameController.GetGame(gameName);
            User user = GetUser(username);
            user.AddGame(game);
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

        public void DeleteGameFromAllUsers( Game gameToDelete)
        {
            if (gameToDelete == null)
                throw new InvalidGameException();
            foreach (User user in _users)
            {

                if (user.OwnedGames != null && user.OwnedGames.Contains(gameToDelete))
                    user.OwnedGames.Remove(gameToDelete);
            }
        }


        public void ModifyGameFromAllUser( Game gameToModify, Game newGame)
        {
            if (gameToModify == null)
                throw new InvalidGameException();
            foreach (User user in _users)
            {
                Game game = user.OwnedGames.Find(game => game.userOwner.Name.ToLower() == gameToModify.userOwner.Name.ToLower() &&
                game.Name.ToLower() == gameToModify.Name.ToLower());

                if (game != null)
                {
                    gameToModify.Name = newGame.Name;
                    gameToModify.Genre = newGame.Genre;
                    gameToModify.Synopsis = newGame.Synopsis;
                }
            }
        }
    }
}
