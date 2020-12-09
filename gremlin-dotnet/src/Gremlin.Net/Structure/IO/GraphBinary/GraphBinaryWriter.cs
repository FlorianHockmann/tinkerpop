using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Gremlin.Net.Structure.IO.GraphBinary
{
    public class GraphBinaryWriter
    {
        private const byte ValueFlagNone = 0;

        private Dictionary<Type, ITypeSerializer> _serializerByType = new Dictionary<Type, ITypeSerializer>
        {
            {typeof(int), SingleTypeSerializer<int>.IntSerializer}
        };
        
        public async Task WriteAsync<T>(T value, Stream stream)
        {
            var serializer = (ITypeSerializer<T>) _serializerByType[typeof(T)];
            await stream.WriteByteAsync(serializer.DataType.TypeCode).ConfigureAwait(false);
            await serializer.WriteAsync(value, stream, this);
        }
        
        public async Task WriteValueFlagNoneAsync(Stream stream) {
            await stream.WriteByteAsync(ValueFlagNone).ConfigureAwait(false);
        }
    }
}