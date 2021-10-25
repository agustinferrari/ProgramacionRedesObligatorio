using Common.FileUtils;
using Common.FileUtils.Interfaces;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using Common.Utils.CustomExceptions;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Common.NetworkUtils
{
    public class NetworkStreamHandler : INetworkStreamHandler
    {
        protected NetworkStream _networkStream;
        protected string _ipAddress;
        protected int _port;
        private IFileHandler _fileHandler;
        private IFileStreamHandler _fileStreamHandler;

        public NetworkStreamHandler(NetworkStream networkStream)
        {
            _networkStream = networkStream;
            _fileHandler = new FileHandler();
            _fileStreamHandler = new FileStreamHandler();
        }

        public NetworkStreamHandler()
        {
            _fileHandler = new FileHandler();
            _fileStreamHandler = new FileStreamHandler();
        }

        public async Task SendMessage(string headerConstant, int commandNumber, string message)
        {
            Header header = new Header(headerConstant, commandNumber, message.Length);
            await SendHeader(header);
            byte[] bytesMessage = Encoding.UTF8.GetBytes(message);
            await SendData(bytesMessage);
        }

        public async Task SendHeader(Header header)
        {
            byte[] data = header.GetRequest();
            await SendData(data);
        }

        public async Task SendData(byte[] data)
        {
            await _networkStream.WriteAsync(data, 0, data.Length);
        }

        public async Task ReceiveData(int Length, byte[] buffer)
        {
            int iRecv = 0;
            while (iRecv < Length)
            {
                try
                {
                    int localRecv = await _networkStream.ReadAsync(buffer, iRecv, Length - iRecv);
                    bool connectionCloseOnRemoteEndPoint = localRecv == 0;
                    if (connectionCloseOnRemoteEndPoint)
                    {
                        throw new SocketClientException();
                    }
                    iRecv += localRecv;
                }
                catch (IOException se)
                {
                    Console.WriteLine(se.Message);
                    return;
                }
            }
        }

        public async Task<Header> ReceiveHeader()
        {
            int headerLength = HeaderConstants.Request.Length + HeaderConstants.CommandLength +
                                   HeaderConstants.DataLength;
            byte[] buffer = new byte[headerLength];
            await ReceiveData(headerLength, buffer);
            Header header = new Header();
            if (header.DecodeData(buffer) == false) throw new FormatException();
            return header;
        }

        public async Task<string> ReceiveString(int dataLength)
        {
            byte[] bufferData = new byte[dataLength];
            await ReceiveData(dataLength, bufferData);
            string data = Encoding.UTF8.GetString(bufferData);
            return data;
        }

        public async Task<string> SendMessageAndRecieveResponse(int command, string messageToSend)
        {
            await SendMessage(HeaderConstants.Request, command, messageToSend);
            return await RecieveResponse();
        }

        public async Task<string> RecieveResponse()
        {
            string response;
            try
            {
                Header header = await ReceiveHeader();
                response = await ReceiveString(header.IDataLength);
            }
            catch (FormatException)
            {
                response = "No se pudo decodificar correctamente";
            }
            return response;
        }

        public async Task<string> ReceiveImage(string rawImageData, string pathToImageFolder, string gameName)
        {
            string fileNameBytes = rawImageData.Substring(0, Specification.FixedFileNameLength);
            string fileSizeBytes = rawImageData.Substring(Specification.FixedFileNameLength, Specification.FixedFileSizeLength);
            int fileNameSize = (Int32.Parse(fileNameBytes));
            long fileSize = (Int64.Parse(fileSizeBytes));

            string dir = "";
            if (fileNameSize != 0 && fileSize != 0)
            {
                string fileName = await ReceiveString(fileNameSize);
                dir = CreateFolder(pathToImageFolder, fileName, gameName);

                long parts = SpecificationHelper.GetParts(fileSize);
                long offset = 0;
                long currentPart = 1;

                while (fileSize > offset)
                {
                    byte[] data;
                    if (currentPart == parts)
                    {
                        int lastPartSize = (int)(fileSize - offset);
                        data = new byte[lastPartSize];
                        await ReceiveData(lastPartSize, data);
                        offset += lastPartSize;
                    }
                    else
                    {
                        data = new byte[Specification.MaxPacketSize];
                        await ReceiveData(Specification.MaxPacketSize, data);
                        offset += Specification.MaxPacketSize;
                    }
                    _fileStreamHandler.Write(dir, data);
                    currentPart++;
                }
            }
            return dir;
        }

        private string CreateFolder(string pathToImageFolder, string fileName, string gameName)
        {
            if (!Directory.Exists(pathToImageFolder))
            {
                Directory.CreateDirectory(pathToImageFolder);
            }
            string newName = (gameName == "") ? fileName : gameName + "_" + fileName;
            string path = pathToImageFolder + newName;
            return path;
        }

        public async Task<bool> SendImage(string path)
        {
            bool imageSentCorrectly = true;
            long fileSize = _fileHandler.GetFileSize(path);
            string fileName = _fileHandler.GetFileName(path);
            await SendImageProtocolData(fileName, fileSize);
            await SendData(Encoding.UTF8.GetBytes(fileName));

            long parts = SpecificationHelper.GetParts(fileSize);
            long offset = 0;
            long currentPart = 1;

            while (fileSize > offset)
            {
                byte[] data;
                int lastPartSize = (int)(fileSize - offset);
                try
                {
                    if (currentPart == parts)
                        data = await _fileStreamHandler.Read(path, offset, lastPartSize);
                    else
                        data = await _fileStreamHandler.Read(path, offset, Specification.MaxPacketSize);
                }
                catch (UnauthorizedAccessException)
                {
                    imageSentCorrectly = false;
                    if (currentPart == parts)
                        data = new byte[lastPartSize];
                    else
                        data = new byte[Specification.MaxPacketSize];
                }
                finally
                {
                    offset += (currentPart == parts) ? lastPartSize : Specification.MaxPacketSize;
                }

                await SendData(data);
                currentPart++;
            }
            return imageSentCorrectly;
        }

        public async Task SendImageProtocolData(string fileName, long fileSize)
        {
            string protocolData = fileName.Length.ToString("D" + Specification.FixedFileNameLength);
            protocolData += fileSize.ToString("D" + Specification.FixedFileSizeLength);
            await SendData(Encoding.UTF8.GetBytes(protocolData));
        }

        public void ShutdownSocket()
        {
            _networkStream.Close();
        }

    }
}
