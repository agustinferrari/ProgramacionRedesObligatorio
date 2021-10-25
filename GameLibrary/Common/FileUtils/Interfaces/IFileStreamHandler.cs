using System.Threading.Tasks;

namespace Common.FileUtils.Interfaces
{
    public interface IFileStreamHandler
    {
        Task<byte[]> Read(string path, long offset, int length);
        void Write(string fileName, byte[] data);
    }
}