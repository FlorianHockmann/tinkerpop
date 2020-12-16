using System.IO;
using System.Threading.Tasks;

namespace Gremlin.Net.Structure.IO.GraphBinary.Types
{
    public abstract class SimpleTypeSerializer<T> : ITypeSerializer<T>
    {
        protected SimpleTypeSerializer(DataType dataType)
        {
            DataType = dataType;
        }

        public DataType DataType { get; }
        public async Task WriteAsync(T value, Stream stream, GraphBinaryWriter writer)
        {
            await WriteValueAsync(value, stream, writer).ConfigureAwait(false);
        }

        protected abstract Task WriteValueAsync(T value, Stream stream, GraphBinaryWriter writer);

        public async Task<T> ReadAsync(Stream stream, GraphBinaryReader reader)
        {
            return await ReadValueAsync(stream, reader).ConfigureAwait(false);
        }

        protected abstract Task<T> ReadValueAsync(Stream stream, GraphBinaryReader reader);
    }
}