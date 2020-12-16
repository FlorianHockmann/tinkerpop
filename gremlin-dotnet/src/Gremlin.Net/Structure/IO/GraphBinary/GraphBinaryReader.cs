using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Gremlin.Net.Structure.IO.GraphBinary.Types;
using Gremlin.Net.Structure.IO.GraphSON;

namespace Gremlin.Net.Structure.IO.GraphBinary
{
    public class GraphBinaryReader
    {
        private readonly Dictionary<DataType, ITypeSerializer> _serializerByType = new Dictionary<DataType, ITypeSerializer>
        {
            {DataType.Int, SingleTypeSerializer<int>.IntSerializer},
            {DataType.Long, SingleTypeSerializer<long>.LongSerializer},
            {DataType.String, new StringSerializer()},
            {DataType.Date, DateTimeOffsetSerializer.DateSerializer},
            {DataType.Double, SingleTypeSerializer<double>.DoubleSerializer},
            {DataType.Float, SingleTypeSerializer<float>.FloatSerializer},
            {DataType.List, new ListSerializer<List<object>>()},
        };

        private readonly Dictionary<DataType, ITypeSerializer> _wrappedSerializers = new Dictionary<DataType, ITypeSerializer>
        {
            {DataType.Int, new TypeConvertingSerializer<int, object>(SingleTypeSerializer<int>.IntSerializer)},
            {DataType.Long, new TypeConvertingSerializer<long, object>(SingleTypeSerializer<long>.LongSerializer)},
            {DataType.String, new TypeConvertingSerializer<string, object>(new StringSerializer())},
            {DataType.Date, new TypeConvertingSerializer<DateTimeOffset, object>(DateTimeOffsetSerializer.DateSerializer)},
            {DataType.Double, new TypeConvertingSerializer<double, object>(SingleTypeSerializer<double>.DoubleSerializer)},
            {DataType.Float, new TypeConvertingSerializer<float, object>(SingleTypeSerializer<float>.FloatSerializer)},
            {DataType.List, new TypeConvertingSerializer<List<object>, object>(new ListSerializer<List<object>>())},
        };
        
        public async Task<T> ReadAsync<T>(Stream stream)
        {
            var type = DataType.FromTypeCode(await stream.ReadByteAsync().ConfigureAwait(false));

            if (type == DataType.UnspecifiedNull)
            {
                await stream.ReadByteAsync().ConfigureAwait(false); // read value byte to advance the index
                return default; // should be null (TODO?)
            }

            var serializer = typeof(T) == typeof(object)
                    ? _wrappedSerializers[type]
                    : _serializerByType[type];

            ITypeSerializer<T> typedSerializer;
            if (serializer is ITypeSerializer<T>)
            {
                typedSerializer = (ITypeSerializer<T>) serializer;    
            }
            else
            {
                // TODO: cast ListSerializer<List<object>> to ListSerializer<T> like ListSerializer<List<string>>
                //typedSerializer = new TypeConvertingSerializer<List<object>, List<string>> (new ListSerializer<List<object>>());
                // this is not working:
                typedSerializer = (ITypeSerializer<T>) serializer;    
            }
            
            return await typedSerializer.ReadAsync(stream, this);
        }
    }
}