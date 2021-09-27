using Common.Protocol;


namespace Common.NetworkUtils.Interfaces
{
    public interface ISocketHandler
    {

        public void SendMessage(string headerConstant, int commandNumber, string message);
        public void SendHeader(Header header);
        public void SendData(byte[] data);
        public void ReceiveData(int Length, byte[] buffer);
        public Header ReceiveHeader();
        public string ReceiveString(int dataLength);
        public string ReceiveImage(string rawImageData, string pathToImageFolder, string gameName);
        string SendMessageAndRecieveResponse(int deletePublishedGame, string gameName);
        string RecieveResponse();
        public bool SendImage(string path);
        public void ShutdownSocket();
        public void SendImageProtocolData(string fileName, long fileSize);
    }
}
