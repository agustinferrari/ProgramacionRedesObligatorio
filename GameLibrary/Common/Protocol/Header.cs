using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Protocol
{
    public class Header
    {
        private byte[] _direction;
        private byte[] _command;
        private byte[] _dataLength;

        private String _sDirection;
        private int _iCommand;
        private int _iDataLength;
        private string _sDataLength;

        public string SDirection
        {
            get => _sDirection;
            set => _sDirection = value;
        }

        public int ICommand
        {
            get => _iCommand;
            set => _iCommand = value;
        }

        public int IDataLength
        {
            get => _iDataLength;
            set => _iDataLength = value;
        }

        public string SDataLength
        {
            get => _sDataLength;
            set => _sDataLength = value;
        }

        public Header()
        {

        }

        public Header(string direction, int command, int datalength)
        {
            _direction = Encoding.UTF8.GetBytes(direction);
            var stringCommand = command.ToString("D2");  //Maximo largo 2, si es menor a 2 cifras, completo con 0s a la izquierda 
            _command = Encoding.UTF8.GetBytes(stringCommand);
            var stringData = datalength.ToString("D4");  // 0 < Largo <= 9999 
            _dataLength = Encoding.UTF8.GetBytes(stringData);
        }

        /*
        public Header(string fileName, long fileSize, string direction, int command)
        {
            try
            {
                ValidateImageName(fileName);
                _direction = Encoding.UTF8.GetBytes(direction);
                string stringCommand = command.ToString("D2");  //Maximo largo 2, si es menor a 2 cifras, completo con 0s a la izquierda 
                _command = Encoding.UTF8.GetBytes(stringCommand);
                string data = fileName.Length.ToString("D" + Specification.FixedFileNameLength);
                data += fileSize.ToString("D" + Specification.FixedFileSizeLength);
                _dataLength = Encoding.UTF8.GetBytes(data);
            }
            catch (Exception e)
            {
                Console.WriteLine("Ruta equivocada, ingresar nuevamente. Cambiar de lugar catch");
            }

        }

        private void ValidateImageName(string fileName)
        {
            byte[] fileNameData = BitConverter.GetBytes(Encoding.UTF8.GetBytes(fileName).Length);
            if (fileNameData.Length != Specification.FixedFileNameLength)
                throw new Exception("There is something wrong with the file name");
        }*/

        public byte[] GetRequest()
        {
            byte[] header = new byte[HeaderConstants.Request.Length + HeaderConstants.CommandLength + HeaderConstants.DataLength];
            Array.Copy(_direction, 0, header, 0, HeaderConstants.Request.Length);
            Array.Copy(_command, 0, header, HeaderConstants.Request.Length, HeaderConstants.CommandLength);
            Array.Copy(_dataLength, 0, header, HeaderConstants.Request.Length + HeaderConstants.CommandLength, HeaderConstants.DataLength);
            return header;
        }

        public bool DecodeData(byte[] data)
        {
            //Hacer try y catch si falla devolver false
            try
            {
                _sDirection = Encoding.UTF8.GetString(data, 0, HeaderConstants.Request.Length);
                string command = Encoding.UTF8.GetString(data, HeaderConstants.Request.Length, HeaderConstants.CommandLength);
                _iCommand = int.Parse(command);
                _sDataLength = Encoding.UTF8.GetString(data, HeaderConstants.Request.Length + HeaderConstants.CommandLength, HeaderConstants.DataLength);
                _iDataLength = int.Parse(_sDataLength);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


    }
}
