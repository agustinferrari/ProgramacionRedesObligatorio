
namespace Common.Protocol
{
    public static class SpecificationHelper
    {
        public static long GetParts(long fileSize)
        {
            long parts = fileSize / Specification.MaxPacketSize;
            return parts * Specification.MaxPacketSize == fileSize ? parts : parts + 1;
        }

        public static int GetImageLength()
        {
            return Specification.FixedFileNameLength + Specification.FixedFileSizeLength;
        }
    }
}
