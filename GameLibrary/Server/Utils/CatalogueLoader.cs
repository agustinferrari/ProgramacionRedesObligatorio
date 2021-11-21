using Server.BusinessLogic;
using Server.Domain;

namespace Server.Utils
{
    public class CatalogueLoader
    {
        public static void AddGames(UserController userController)
        {
            User user = new User
            {
                Name = "Pepe"
            };
            Game newGame = new Game
            {
                Name = "Minecraft",
                Genre = "Building",
                PathToPhoto = "ahtddstd",
                Synopsis = "A building world made of blocks",
                Rating = 10,
                OwnerUser = user
            };
            Game newGame1 = new Game
            {
                Name = "Call of Duty",
                Genre = "Shooter",
                PathToPhoto = "ahtddstd",
                Synopsis = "First person online shooter",
                Rating = 5,
                OwnerUser = user
            };
            Game newGame2 = new Game
            {
                Name = "Fifa",
                Genre = "Football",
                PathToPhoto = "ahtddstd",
                Synopsis = "Footbal game",
                Rating = 3,
                OwnerUser = user
            };
            Game newGame3 = new Game
            {
                Name = "Hades",
                Genre = "Roguelike",
                PathToPhoto = "ahtddstd",
                Synopsis = "Son of Hades escaped underworld",
                Rating = 7,
                OwnerUser = user
            };
            userController.TryAddUser(user.Name);
            GameController.Instance.AddGame(newGame);
            GameController.Instance.AddGame(newGame1);
            GameController.Instance.AddGame(newGame2);
            GameController.Instance.AddGame(newGame3);
        }
    }
}
