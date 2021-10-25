using System;
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

        public Header()
        {
        }

        public Header(string direction, int command, int datalength)
        {
            _direction = Encoding.UTF8.GetBytes(direction);
            string stringCommand = command.ToString("D2");
            _command = Encoding.UTF8.GetBytes(stringCommand);
            string stringData = datalength.ToString("D4");
            _dataLength = Encoding.UTF8.GetBytes(stringData);
        }

        public byte[] GetRequest()
        {
            int initialPositionSource = 0;
            int initialPositionDestination = 0;

            byte[] header = new byte[HeaderConstants.Request.Length + HeaderConstants.CommandLength + HeaderConstants.DataLength];
            Array.Copy(_direction, initialPositionSource, header, initialPositionDestination, HeaderConstants.Request.Length);
            Array.Copy(_command, initialPositionSource, header, HeaderConstants.Request.Length, HeaderConstants.CommandLength);
            Array.Copy(_dataLength, initialPositionSource, header, HeaderConstants.Request.Length + HeaderConstants.CommandLength, HeaderConstants.DataLength);
            return header;
        }

        public bool DecodeData(byte[] data)
        {
            try
            {
                int initialPositionSource = 0;
                _sDirection = Encoding.UTF8.GetString(data, initialPositionSource, HeaderConstants.Request.Length);
                string command = Encoding.UTF8.GetString(data, HeaderConstants.Request.Length, HeaderConstants.CommandLength);
                _iCommand = int.Parse(command);
                string dataLength = Encoding.UTF8.GetString(data, HeaderConstants.Request.Length + HeaderConstants.CommandLength, HeaderConstants.DataLength);
                _iDataLength = int.Parse(dataLength);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }


    }
}
