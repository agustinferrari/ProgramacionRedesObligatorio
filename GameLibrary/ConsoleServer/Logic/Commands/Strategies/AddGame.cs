using Common.NetworkUtils;
using Common.Protocol;
using ConsoleServer.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.Logic.Commands.Strategies
{
    public class AddGame : CommandStrategy
    {

        public override void HandleRequest(Header header, SocketHandler clientSocketHandler)
        {
            string rawData = clientSocketHandler.ReceiveString(header.IDataLength);
            string[] gameData = rawData.Split('%');
            string name = gameData[0];
            string genre = gameData[1];
            string synopsis = gameData[2];
            Console.WriteLine("Name: " + name + ", Genre: " + genre + ", Synopsis: " + synopsis);
            string rawImageData = clientSocketHandler.ReceiveString(SpecificationHelper.GetImageDataLength());
            string pathToImageGame = clientSocketHandler.ReceiveImage(rawImageData); //Ver donde guardarla imagen
            Game newGame = new Game
            {
                Name = name,
                Genre = genre,
                Synopsis = synopsis,
                Rating = 0,
                PathToPhoto = pathToImageGame
            };
            this._gameController.AddGame(newGame);
            string responseMessageResult = ResponseConstants.AddGameSuccess;
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.AddGame, responseMessageResult);
        }
    }
}
