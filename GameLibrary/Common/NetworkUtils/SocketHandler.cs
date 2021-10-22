using Common.FileUtils;
using Common.FileUtils.Interfaces;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using Common.Utils.CustomExceptions;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Common.NetworkUtils
{
    public class SocketHandler : ISocketHandler
    {
        protected NetworkStream _networkStream;
        protected string _ipAddress;
        protected int _port;
        private IFileHandler _fileHandler;
        private IFileStreamHandler _fileStreamHandler;

        public SocketHandler(NetworkStream networkStream)
        {
            _networkStream = networkStream;
            _fileHandler = new FileHandler();
            _fileStreamHandler = new FileStreamHandler();
        }

        public SocketHandler()
        {
            _fileHandler = new FileHandler();
            _fileStreamHandler = new FileStreamHandler();
        }

        public void SendMessage(string headerConstant, int commandNumber, string message)
        {
            Header header = new Header(headerConstant, commandNumber, message.Length);
            SendHeader(header);
            byte[] bytesMessage = Encoding.UTF8.GetBytes(message);
            SendData(bytesMessage);
        }

        public void SendHeader(Header header)
        {
            byte[] data = header.GetRequest();
            SendData(data);
        }

        public void SendData(byte[] data)
        {
            _networkStream.Write(data, 0, data.Length);
        }

        public void ReceiveData(int Length, byte[] buffer)
        {
            var iRecv = 0;
            while (iRecv < Length)
            {
                try
                {
                    int localRecv = _networkStream.Read(buffer, iRecv, Length - iRecv);
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

        public Header ReceiveHeader()
        {
            int headerLength = HeaderConstants.Request.Length + HeaderConstants.CommandLength +
                                   HeaderConstants.DataLength;
            byte[] buffer = new byte[headerLength];
            ReceiveData(headerLength, buffer);
            Header header = new Header();
            if (header.DecodeData(buffer) == false) throw new FormatException();
            return header;
        }

        public string ReceiveString(int dataLength)
        {
            byte[] bufferData = new byte[dataLength];
            ReceiveData(dataLength, bufferData);
            string data = Encoding.UTF8.GetString(bufferData);
            return data;
        }

        public string SendMessageAndRecieveResponse(int command, string messageToSend)
        {
            SendMessage(HeaderConstants.Request, command, messageToSend);
            return RecieveResponse();
        }

        public string RecieveResponse()
        {
            string response;
            try
            {
                Header header = ReceiveHeader();
                response = ReceiveString(header.IDataLength);
            }
            catch (FormatException)
            {
                response = "No se pudo decodificar correctamente";
            }
            return response;
        }

        public string ReceiveImage(string rawImageData, string pathToImageFolder, string gameName)
        {
            string fileNameBytes = rawImageData.Substring(0, Specification.FixedFileNameLength);
            string fileSizeBytes = rawImageData.Substring(Specification.FixedFileNameLength, Specification.FixedFileSizeLength);
            int fileNameSize = (Int32.Parse(fileNameBytes));
            long fileSize = (Int64.Parse(fileSizeBytes));

            string dir = "";
            if (fileNameSize != 0 && fileSize != 0)
            {
                string fileName = ReceiveString(fileNameSize);
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
                        ReceiveData(lastPartSize, data);
                        offset += lastPartSize;
                    }
                    else
                    {
                        data = new byte[Specification.MaxPacketSize];
                        ReceiveData(Specification.MaxPacketSize, data);
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

        public bool SendImage(string path)
        {
            bool imageSentCorrectly = true;
            long fileSize = _fileHandler.GetFileSize(path);
            string fileName = _fileHandler.GetFileName(path);
            SendImageProtocolData(fileName, fileSize);
            SendData(Encoding.UTF8.GetBytes(fileName));

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
                        data = _fileStreamHandler.Read(path, offset, lastPartSize);
                    else
                        data = _fileStreamHandler.Read(path, offset, Specification.MaxPacketSize);
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

                SendData(data);
                currentPart++;
            }
            return imageSentCorrectly;
        }

        public void SendImageProtocolData(string fileName, long fileSize)
        {
            string protocolData = fileName.Length.ToString("D" + Specification.FixedFileNameLength);
            protocolData += fileSize.ToString("D" + Specification.FixedFileSizeLength);
            SendData(Encoding.UTF8.GetBytes(protocolData));
        }

        public void ShutdownSocket()
        {
            //_networkStream.Shutdown(SocketShutdown.Both);
            _networkStream.Close();
        }

        public bool IsSocketClosed()
        {
            //return _networkStream.SafeHandle.IsClosed;
            return true;
        }
    }
}
