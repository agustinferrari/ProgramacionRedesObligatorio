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
            var data = new byte[length];

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                fs.Position = offset;
                int bytesRead = 0;
                while (bytesRead < length)
                {
                    int read = fs.Read(data, bytesRead, length - bytesRead);
                    if (read == 0)
                    {
                        throw new UnableToReadFileException();
                    }
                    bytesRead += read;
                }
            }

            return data;
        }

        public void Write(string wantedPath, byte[] data)
        {

            if (File.Exists(wantedPath))
            {
                using (var fs = new FileStream(wantedPath, FileMode.Append))
                {
                    fs.Write(data, 0, data.Length);
                }
            }
            else
            {
                using (var fs = new FileStream(wantedPath, FileMode.Create))
                {
                    fs.Write(data, 0, data.Length);
                }
            }
        }
    }
}