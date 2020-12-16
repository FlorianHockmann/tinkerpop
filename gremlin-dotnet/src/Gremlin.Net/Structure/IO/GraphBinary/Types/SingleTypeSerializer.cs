using System;
using System.IO;
using System.Threading.Tasks;

namespace Gremlin.Net.Structure.IO.GraphBinary.Types
{
    public class SingleTypeSerializer<T> : ITypeSerializer<T>
    {
        public static SingleTypeSerializer<int> IntSerializer = new SingleTypeSerializer<int>(DataType.Int,
            (value, stream) => stream.WriteIntAsync(value), stream => stream.ReadIntAsync());

        public static SingleTypeSerializer<long> LongSerializer = new SingleTypeSerializer<long>(DataType.Long,
            (value, stream) => stream.WriteLongAsync(value), stream => stream.ReadLongAsync());
        
        public static SingleTypeSerializer<double> DoubleSerializer = new SingleTypeSerializer<double>(DataType.Double,
            (value, stream) => stream.WriteDoubleAsync(value), stream => stream.ReadDoubleAsync());
        
        public static SingleTypeSerializer<float> FloatSerializer = new SingleTypeSerializer<float>(DataType.Float,
            (value, stream) => stream.WriteFloatAsync(value), stream => stream.ReadFloatAsync());
        
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
        
        public DataType DataType { get; }
    }
}