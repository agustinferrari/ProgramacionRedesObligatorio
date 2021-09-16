namespace Common.FileHandler.Interfaces
{
    public interface IFileStreamHandler
    {
        byte[] Read(string path, long offset, int length);
        void Write(string fileName, byte[] data);
    }
}