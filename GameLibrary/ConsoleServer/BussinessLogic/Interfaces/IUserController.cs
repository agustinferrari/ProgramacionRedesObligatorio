using ConsoleServer.Domain;


namespace ConsoleServer.BussinessLogic.Interfaces
{
    public interface IUserController
    {
        public static IGameController Instance;


        public void TryAddUser(string name);

        public void BuyGame(string username, string gameName);

        public User GetUser(string username);

        public string ListOwnedGameByUser(string username);

        public void DeleteGameFromAllUsers(Game gameToDelete);

        public void ModifyGameFromAllUser(Game gameToModify, Game newGame);

    }
}
