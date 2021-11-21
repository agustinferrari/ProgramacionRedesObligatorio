using ServerGRPC.Domain;


namespace ServerGRPC.BusinessLogic.Interfaces
{
    public interface IGameController
    {

        public static IGameController Instance;

        public void AddGame(Game gameToAdd);

        public string GetAllGames();

        public Game GetGame(string gameName);

        public void AddReview(string gameName, Review newReview);

        public string GetGamesFiltered(string rawData);

        public Game GetCertainGamePublishedByUser(User user, string gameName);

        public void DeletePublishedGameByUser(Game gameToDelete);

        public void ModifyGame(Game gameToModify, Game newGame);

    }
}
