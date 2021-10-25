using System;
using System.IO;
using Common.FileUtils.Interfaces;
using Common.Utils.CustomExceptions;

namespace Common.FileUtils
{
    public class FileStreamHandler : IFileStreamHandler
    {
        public byte[] Read(string path, long offset, int length)
        {
            byte[] data = new byte[length];

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                fs.Position = offset;
                int bytesRead = 0;
                while (bytesRead < length)
                {
                    int read = fs.Read(data, bytesRead, length - bytesRead);
                    bytesRead += read;
                }
            }

            return data;
        }

        public void Write(string wantedPath, byte[] data)
        {

            if (File.Exists(wantedPath))
            {
                using (FileStream fs = new FileStream(wantedPath, FileMode.Append))
                {
                    fs.Write(data, 0, data.Length);
                }
            }
            else
            {
                using (FileStream fs = new FileStream(wantedPath, FileMode.Create))
                {
                    fs.Write(data, 0, data.Length);
                }
            }
        }
    }
}