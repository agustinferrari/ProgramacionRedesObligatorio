using System.Threading.Tasks;
using Common.Protocol;


namespace Common.NetworkUtils.Interfaces
{
    public interface ISocketHandler
    {
        public Task SendMessage(string headerConstant, int commandNumber, string message);

        public Task SendHeader(Header header);

        public Task SendData(byte[] data);

        public Task ReceiveData(int Length, byte[] buffer);

        public Task<Header> ReceiveHeader();

        public Task<string> ReceiveString(int dataLength);

        public Task<string> ReceiveImage(string rawImageData, string pathToImageFolder, string gameName);

        public Task<string> RecieveResponse();

        public Task<string> SendMessageAndRecieveResponse(int deletePublishedGame, string gameName);

        public Task<bool> SendImage(string path);

        public void ShutdownSocket();

        public Task SendImageProtocolData(string fileName, long fileSize);

    }
}
