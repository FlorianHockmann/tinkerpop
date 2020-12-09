using System;
using System.IO;
using System.Threading.Tasks;

namespace Gremlin.Net.Structure.IO.GraphBinary
{
    public class SingleTypeSerializer<T> : ITypeSerializer<T>
    {
        public static SingleTypeSerializer<int> IntSerializer =
            new SingleTypeSerializer<int>(DataType.Int,
                (value, stream) => stream.WriteAsync(BitConverter.GetBytes(value), 0, 4),
                (ReadIntAsync));

        private readonly Func<T, Stream, Task> _writeFunc;
        private readonly Func<Stream, Task<T>> _readFunc;
        
        public SingleTypeSerializer(DataType dataType, Func<T, Stream, Task> writeFunc, Func<Stream, Task<T>> readFunc)
        {
            DataType = dataType;
            _writeFunc = writeFunc;
            _readFunc = readFunc;
        }
        
        public async Task WriteAsync(T value, Stream stream, GraphBinaryWriter writer)
        {
            //await writer.WriteValueFlagNoneAsync(stream).ConfigureAwait(false);
            await _writeFunc.Invoke(value, stream).ConfigureAwait(false);
        }

        public async Task<T> ReadAsync(Stream stream, GraphBinaryReader reader)
        {
            //var valueFlag = await stream.ReadByteAsync(); // TODO: Implement nullable support
            return await _readFunc.Invoke(stream).ConfigureAwait(false);
        }

        private static async Task<int> ReadIntAsync(Stream stream)
        {
            var bytes = new byte[4];
            await stream.ReadAsync(bytes, 0, 4);
            return BitConverter.ToInt32(bytes, 0);
        }

        public DataType DataType { get; }
    }
}