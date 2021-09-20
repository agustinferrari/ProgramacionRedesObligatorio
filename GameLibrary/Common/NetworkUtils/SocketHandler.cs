using Common.FileUtils;
using Common.FileUtils.Interfaces;
using Common.Protocol;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Common.NetworkUtils
{
    public class SocketHandler
    {
        public Socket _socket;
        protected string _ipAddress;
        protected int _port;
        private IFileHandler _fileHandler;
        private IFileStreamHandler _fileStreamHandler;

        public SocketHandler(Socket socket)
        {
            _socket = socket;
            _fileHandler = new FileHandler();
            _fileStreamHandler = new FileStreamHandler();
        }

        public SocketHandler(string ipAddress, int port)
        {
            _fileHandler = new FileHandler();
            _fileStreamHandler = new FileStreamHandler();
            _ipAddress = ipAddress;
            _port = port;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(_ipAddress), _port));
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
            int sentBytes = 0;
            while (sentBytes < data.Length)
            {
                sentBytes += _socket.Send(data, sentBytes, data.Length - sentBytes, SocketFlags.None);
            }
        }

        public void ReceiveData(int Length, byte[] buffer)
        {
            var iRecv = 0;
            while (iRecv < Length)
            {
                try
                {
                    int localRecv = _socket.Receive(buffer, iRecv, Length - iRecv, SocketFlags.None);
                    if (localRecv == 0) // Si recieve retorna 0 -> la conexion se cerro desde el endpoint remoto
                    {
                        throw new Exception("Connection has been closed"); // Catchear en server y si el server sigue andando cerrar la conexion 
                    }
                    iRecv += localRecv;
                }
                catch (SocketException se)
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
            this.ReceiveData(headerLength, buffer);
            Header header = new Header();
            header.DecodeData(buffer);
            return header;
        }

        public string ReceiveString(int dataLength)
        {
            byte[] bufferData = new byte[dataLength];
            this.ReceiveData(dataLength, bufferData);
            string data = Encoding.UTF8.GetString(bufferData);
            return data;
        }

        public string ReceiveImage(string rawImageData)
        {
            // 1) Recibo 12 bytes
            // 2) Tomo los 4 primeros bytes para saber el largo del nombre del archivo
            // 3) Tomo los siguientes 8 bytes para saber el tamaño del archivo
            byte[] buffer = new byte[SpecificationHelper.GetImageDataLength()];
            string fileNameBytes = rawImageData.Substring(0, Specification.FixedFileNameLength);
            string fileSizeBytes = rawImageData.Substring(Specification.FixedFileNameLength, Specification.FixedFileSizeLength);
            int fileNameSize = (Int32.Parse(fileNameBytes));
            long fileSize = (Int64.Parse(fileSizeBytes));

            // 4) Recibo el nombre del archivo
            string fileName = ReceiveString(fileNameSize);
            string wantedPath = ChangePathToImagesFolder(fileName);

            // 5) Calculo la cantidad de partes a recibir
            long parts = SpecificationHelper.GetParts(fileSize);
            long offset = 0;
            long currentPart = 1;

            while (fileSize > offset)
            {
                byte[] data = new byte[fileSize];
                if (currentPart == parts)
                {
                    int lastPartSize = (int)(fileSize - offset);
                    ReceiveData(lastPartSize, data);
                    offset += lastPartSize;
                }
                else
                {
                    ReceiveData(Specification.MaxPacketSize, data);
                    offset += Specification.MaxPacketSize;
                }
                _fileStreamHandler.Write(wantedPath, data);
                currentPart++;
            }
            return wantedPath;
        }

        private string ChangePathToImagesFolder(string fileName)
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName;
            string wantedPath = workingDirectory + "\\GamesImages\\" + fileName;
            return wantedPath;
        }

        public void SendImage(string path)
        {
            // El envio del archivo se compone de las siguientes etapas:
            // 1) Creo un paquete de datos que tiene esta estructura XXXX YYYYYYYY <NOMBRE>
            //          a) XXXX -> Largo del nombre del archivo
            //          b) YYYYYYYY -> Tamaño en bytes del archivo
            //          c) <NOMBRE> -> Nombre del archivo

            long fileSize = _fileHandler.GetFileSize(path); //Obtenemos el tamaño del archivo
            string fileName = _fileHandler.GetFileName(path); //Obtenemos el nombre del archivo
            string protocolData = fileName.Length.ToString("D" + Specification.FixedFileNameLength);
            protocolData += fileSize.ToString("D" + Specification.FixedFileSizeLength);
            SendData(Encoding.UTF8.GetBytes(protocolData));
            SendData(Encoding.UTF8.GetBytes(fileName));

            // 2) Calculo tamaño y cantidad de partes a enviar
            var parts = SpecificationHelper.GetParts(fileSize);
            long offset = 0;
            long currentPart = 1;

            // 3) Mientras tengo partes, envio
            while (fileSize > offset)
            {
                byte[] data;
                if (currentPart == parts)
                {
                    int lastPartSize = (int)(fileSize - offset);
                    data = _fileStreamHandler.Read(path, offset, lastPartSize);
                    offset += lastPartSize;
                }
                else
                {
                    data = _fileStreamHandler.Read(path, offset, Specification.MaxPacketSize);
                    offset += Specification.MaxPacketSize;
                }

                SendData(data);
                currentPart++;
            }
        }

        public void ShutdownSocket()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }
    }
}
