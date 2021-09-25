
namespace Common.Protocol
{
    public static class SpecificationHelper
    {
        public static long GetParts(long fileSize)
        {
            long parts = fileSize / Specification.MaxPacketSize;
            long nextParts = parts + 1;
            return parts * Specification.MaxPacketSize == fileSize ? parts : nextParts;
        }

        public static int GetImageDataLength()
        {
            return Specification.FixedFileNameLength + Specification.FixedFileSizeLength;
        }
    }
}
