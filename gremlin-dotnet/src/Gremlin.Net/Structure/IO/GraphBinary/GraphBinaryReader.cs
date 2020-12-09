using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Gremlin.Net.Structure.IO.GraphBinary
{
    public class GraphBinaryReader
    {
        private readonly Dictionary<DataType, ITypeSerializer> _serializerByType = new Dictionary<DataType, ITypeSerializer>
        {
            {DataType.Int, SingleTypeSerializer<int>.IntSerializer}
        };
        
        public async Task<T> ReadAsync<T>(Stream stream)
        {
            var type = DataType.FromTypeCode(await stream.ReadByteAsync().ConfigureAwait(false));
            var serializer = (ITypeSerializer<T>) _serializerByType[type];
            return await serializer.ReadAsync(stream, this);
        }
    }
}