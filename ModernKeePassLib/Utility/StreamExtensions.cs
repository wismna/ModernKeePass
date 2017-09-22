using System.IO;
using Windows.Storage.Streams;

namespace ModernKeePassLibPCL.Utility
{
    public static class StreamExtensions
    {
        public static Stream AsStream(this IRandomAccessStream inputStream)
        {
            var reader = new DataReader(inputStream.GetInputStreamAt(0));
            var bytes = new byte[inputStream.Size];
            reader.LoadAsync((uint)inputStream.Size).GetResults();
            reader.ReadBytes(bytes);
            return new MemoryStream(bytes);
        }
    }
}
