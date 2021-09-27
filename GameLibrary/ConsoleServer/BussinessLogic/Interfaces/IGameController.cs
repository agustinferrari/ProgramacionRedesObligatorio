using ConsoleServer.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.BussinessLogic.Interfaces
{ 
    public interface IGameController
    {

        public static IGameController Instance;

        public void AddGame(Game gameToAdd);

        public string GetGames();

        public Game GetGame(string gameName);

        public void AddReview(string gameName, Review newReview);

        public string GetGamesFiltered(string rawData);

        public Game GetCertainGamePublishedByUser(User user, string gameName);

        public void DeletePublishedGameByUser(Game gameToDelete);

        public void ModifyGame(Game gameToModify, Game newGame);

    }
}
