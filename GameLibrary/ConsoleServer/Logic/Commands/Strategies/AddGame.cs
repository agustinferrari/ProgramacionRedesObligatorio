using Common.NetworkUtils;
using Common.Protocol;
using ConsoleServer.Domain;
using ConsoleServer.Utils.CustomExceptions;


namespace ConsoleServer.Logic.Commands.Strategies
{
    public class AddGame : CommandStrategy
    {

        public override void HandleRequest(Header header, SocketHandler clientSocketHandler)
        {
            int firstElement = 0;
            int secondElement = 1;
            int thirdElement = 2;
            string rawData = clientSocketHandler.ReceiveString(header.IDataLength);
            string[] gameData = rawData.Split('%');
            string name = gameData[firstElement];
            string genre = gameData[secondElement];
            string synopsis = gameData[thirdElement];
            string rawImageData = clientSocketHandler.ReceiveString(SpecificationHelper.GetImageDataLength());
            string pathToImageGame = clientSocketHandler.ReceiveImage(rawImageData);
            Game newGame = new Game
            {
                Name = name,
                Genre = genre,
                Synopsis = synopsis,
                Rating = 0,
                PathToPhoto = pathToImageGame
            };
            string responseMessageResult;
            try
            { 
                this._gameController.AddGame(newGame);
                 responseMessageResult = ResponseConstants.AddGameSuccess;
            }
            catch (GameAlreadyAddedException)
            {
                responseMessageResult = ResponseConstants.AddGameError;
            }
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.AddGame, responseMessageResult);
        }
    }
}
