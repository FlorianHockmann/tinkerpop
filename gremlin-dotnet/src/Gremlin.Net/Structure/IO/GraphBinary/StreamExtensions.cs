using System;
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

        public static async Task WriteIntAsync(this Stream stream, int value)
        {
            await stream.WriteAsync(BitConverter.GetBytes(value), 0, 4).ConfigureAwait(false);
        }
        
        public static async Task<int> ReadIntAsync(this Stream stream)
        {
            var bytes = new byte[4];
            await stream.ReadAsync(bytes, 0, 4).ConfigureAwait(false);
            return BitConverter.ToInt32(bytes, 0);
        }
        
        public static async Task WriteLongAsync(this Stream stream, long value)
        {
            await stream.WriteAsync(BitConverter.GetBytes(value), 0, 8).ConfigureAwait(false);
        }
        
        public static async Task<long> ReadLongAsync(this Stream stream)
        {
            var bytes = new byte[8];
            await stream.ReadAsync(bytes, 0, 8).ConfigureAwait(false);
            return BitConverter.ToInt64(bytes, 0);
        }
        
        public static async Task WriteFloatAsync(this Stream stream, float value)
        {
            await stream.WriteAsync(BitConverter.GetBytes(value), 0, 4).ConfigureAwait(false);
        }
        
        public static async Task<float> ReadFloatAsync(this Stream stream)
        {
            var bytes = new byte[4];
            await stream.ReadAsync(bytes, 0, 4).ConfigureAwait(false);
            return BitConverter.ToSingle(bytes, 0);
        }
        
        public static async Task WriteDoubleAsync(this Stream stream, double value)
        {
            await stream.WriteAsync(BitConverter.GetBytes(value), 0, 8).ConfigureAwait(false);
        }
        
        public static async Task<double> ReadDoubleAsync(this Stream stream)
        {
            var bytes = new byte[8];
            await stream.ReadAsync(bytes, 0, 8).ConfigureAwait(false);
            return BitConverter.ToDouble(bytes, 0);
        }
    }
}