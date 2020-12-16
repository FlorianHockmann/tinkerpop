using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Gremlin.Net.Structure.IO.GraphBinary.Types;

namespace Gremlin.Net.Structure.IO.GraphBinary
{
    public class GraphBinaryWriter
    {
        private const byte ValueFlagNone = 0;
        private static readonly byte[] UnspecifiedNullBytes = {DataType.UnspecifiedNull.TypeCode, 0x01};

        private readonly Dictionary<Type, ITypeSerializer> _serializerByType = new Dictionary<Type, ITypeSerializer>
        {
            {typeof(int), SingleTypeSerializer<int>.IntSerializer},
            {typeof(long), SingleTypeSerializer<long>.LongSerializer},
            {typeof(string), new StringSerializer()},
            {typeof(DateTimeOffset), DateTimeOffsetSerializer.DateSerializer},
            {typeof(double), SingleTypeSerializer<double>.DoubleSerializer},
            {typeof(float), SingleTypeSerializer<long>.FloatSerializer},
            {typeof(IList), new ListSerializer<List<object>>()}
        };
        
        private readonly Dictionary<Type, ITypeSerializer> _wrappedSerializers = new Dictionary<Type, ITypeSerializer>
        {
            {typeof(int), new TypeConvertingSerializer<int, object>(SingleTypeSerializer<int>.IntSerializer)},
            {typeof(long), new TypeConvertingSerializer<long, object>(SingleTypeSerializer<long>.LongSerializer)},
            {typeof(string), new TypeConvertingSerializer<string, object>(new StringSerializer())},
            {typeof(DateTimeOffset), new TypeConvertingSerializer<DateTimeOffset, object>(DateTimeOffsetSerializer.DateSerializer)},
            {typeof(double), new TypeConvertingSerializer<double, object>(SingleTypeSerializer<double>.DoubleSerializer)},
            {typeof(float), new TypeConvertingSerializer<float, object>(SingleTypeSerializer<long>.FloatSerializer)},
            {typeof(IList), new TypeConvertingSerializer<List<object>, object>(new ListSerializer<List<object>>())}
        };
        
        public async Task WriteAsync<T>(T value, Stream stream)
        {
            if (value == null)
            {
                await stream.WriteAsync(UnspecifiedNullBytes, 0, UnspecifiedNullBytes.Length).ConfigureAwait(false);
                return;
            }

            var valueType = value.GetType();
            var serializer = GetSerializerFor<T>(valueType);
            await stream.WriteByteAsync(serializer.DataType.TypeCode).ConfigureAwait(false);
            await serializer.WriteAsync(value, stream, this).ConfigureAwait(false);
        }

        private ITypeSerializer<T> GetSerializerFor<T>(Type valueType)
        {
            if (_serializerByType.ContainsKey(valueType))
            {
                var serializer = typeof(T) == typeof(object)
                    ? _wrappedSerializers[valueType]
                    : _serializerByType[valueType];

                return (ITypeSerializer<T>) serializer;
            }
            foreach (var supportedType in _serializerByType.Keys)
                if (supportedType.IsAssignableFrom(valueType))
                {
                    var genericTypeSerializer = _serializerByType[supportedType];
                    var genericSerializerType = genericTypeSerializer.GetType().GetGenericTypeDefinition();
                    var concreteSerializerType = genericSerializerType.MakeGenericType(typeof(T));
                    var concreteTypeSerializer = Activator.CreateInstance(concreteSerializerType);
                    
                    
                    return (ITypeSerializer<T>) concreteTypeSerializer;
                    // var genericSerializer = _serializerByType[supportedType];
                    // genericSerializer.GetType().
                    // var concreteSerializerType = genericSerializer.GetType().MakeGenericType(valueType);
                    // var concreteSerializer = (ITypeSerializer<T>) Activator.CreateInstance(concreteSerializerType);
                    // _serializerByType[valueType] = concreteSerializer;
                    // return concreteSerializer;
                }

            throw new InvalidOperationException($"No serializer found for type ${valueType}.");
        }

        public async Task WriteValueFlagNoneAsync(Stream stream) {
            await stream.WriteByteAsync(ValueFlagNone).ConfigureAwait(false);
        }
    }
}