
namespace Common.Protocol
{
    public static class SpecificationHelper
    {
        public static long GetParts(long fileSize)
        {
            var parts = fileSize / Specification.MaxPacketSize;
            return parts * Specification.MaxPacketSize == fileSize ? parts : parts + 1;
        }
    }
}
