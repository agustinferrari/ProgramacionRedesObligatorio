using System;
using System.IO;
using Common.FileUtils.Interfaces;

namespace Common.FileUtils
{
    public class FileStreamHandler : IFileStreamHandler
    {
        public byte[] Read(string path, long offset, int length)
        {
            var data = new byte[length];

            using (var fs = new FileStream(path, FileMode.Open))
            {
                fs.Position = offset;
                var bytesRead = 0;
                while (bytesRead < length)
                {
                    var read = fs.Read(data, bytesRead, length - bytesRead);
                    if (read == 0)
                    {
                        throw new Exception("Couldn't not read file");
                    }
                    bytesRead += read;
                }
            }

            return data;
        }

        public void Write(string fileName, byte[] data)
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName;
            string wantedPath = projectDirectory + "\\GamesImages\\" + fileName;
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