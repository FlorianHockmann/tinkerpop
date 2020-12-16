using System.IO;
using System.Threading.Tasks;

namespace Gremlin.Net.Structure.IO.GraphBinary.Types
{
    public class TypeConvertingSerializer<TFrom, TTo> : ITypeSerializer<TTo>
        where TFrom : TTo
    {
        private ITypeSerializer<TFrom> _wrappedSerializer;
        
        public TypeConvertingSerializer(ITypeSerializer<TFrom> wrappedSerializer)
        {
            _wrappedSerializer = wrappedSerializer;
        }

        public DataType DataType => _wrappedSerializer.DataType;
        public async Task WriteAsync(TTo value, Stream stream, GraphBinaryWriter writer)
        {
            await _wrappedSerializer.WriteAsync((TFrom) value, stream, writer).ConfigureAwait(false);
        }

        public async Task<TTo> ReadAsync(Stream stream, GraphBinaryReader reader)
        {
            return await _wrappedSerializer.ReadAsync(stream, reader).ConfigureAwait(false);
        }
    }
}