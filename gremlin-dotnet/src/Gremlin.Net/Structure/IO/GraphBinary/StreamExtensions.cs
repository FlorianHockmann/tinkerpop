using System.IO;
using System.Threading.Tasks;

namespace Gremlin.Net.Structure.IO.GraphBinary
{
    internal static class StreamExtensions
    {
        public static async Task WriteByteAsync(this Stream stream, byte value)
        {
            await stream.WriteAsync(new[] {value}, 0, 1).ConfigureAwait(false);
        }
        
        public static async Task<byte> ReadByteAsync(this Stream stream)
        {
            var readBuffer = new byte[1];
            await stream.ReadAsync(readBuffer, 0, 1);
            return readBuffer[0];
        }
    }
}