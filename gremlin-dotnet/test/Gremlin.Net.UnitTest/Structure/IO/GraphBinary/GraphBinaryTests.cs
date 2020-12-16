using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gremlin.Net.Structure.IO.GraphBinary;
using Gremlin.Net.Structure.IO.GraphBinary.Types;
using Xunit;

namespace Gremlin.Net.UnitTest.Structure.IO.GraphBinary
{
    public class GraphBinaryTests
    {
        [Fact]
        public async Task TestNull()
        {
            var writer = CreateGraphBinaryWriter();
            var reader = CreateGraphBinaryReader();
            var serializationStream = new MemoryStream();
            
            await writer.WriteAsync<object>(null, serializationStream);
            serializationStream.Position = 0;
            var actual = await reader.ReadAsync<object>(serializationStream);
            
            Assert.Null(actual);
        }
    
        [Fact]
        public async Task TestInt()
        {
            const int expected = 100;
            var writer = CreateGraphBinaryWriter();
            var reader = CreateGraphBinaryReader();
            var serializationStream = new MemoryStream();
            
            await writer.WriteAsync(expected, serializationStream);
            serializationStream.Position = 0;
            var actual = await reader.ReadAsync<int>(serializationStream);
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task TestLong()
        {
            const long expected = 100;
            var writer = CreateGraphBinaryWriter();
            var reader = CreateGraphBinaryReader();
            var serializationStream = new MemoryStream();
            
            await writer.WriteAsync(expected, serializationStream);
            serializationStream.Position = 0;
            var actual = await reader.ReadAsync<long>(serializationStream);
            
            Assert.Equal(expected, actual);
        }
        
        [Theory]
        [InlineData(100.01f)]
        [InlineData(float.NaN)]
        [InlineData(float.NegativeInfinity)]
        [InlineData(float.PositiveInfinity)]
        public async Task TestFloat(float expected)
        {
            var writer = CreateGraphBinaryWriter();
            var reader = CreateGraphBinaryReader();
            var serializationStream = new MemoryStream();
            
            await writer.WriteAsync(expected, serializationStream);
            serializationStream.Position = 0;
            var actual = await reader.ReadAsync<float>(serializationStream);
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task TestDouble()
        {
            const double expected = 100.001;
            var writer = CreateGraphBinaryWriter();
            var reader = CreateGraphBinaryReader();
            var serializationStream = new MemoryStream();
            
            await writer.WriteAsync(expected, serializationStream);
            serializationStream.Position = 0;
            var actual = await reader.ReadAsync<double>(serializationStream);
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task TestDate()
        {
            var expected = DateTimeOffset.ParseExact("2016-12-14 16:14:36.295000", "yyyy-MM-dd HH:mm:ss.ffffff",
                CultureInfo.InvariantCulture);
            var writer = CreateGraphBinaryWriter();
            var reader = CreateGraphBinaryReader();
            var serializationStream = new MemoryStream();
            
            await writer.WriteAsync(expected, serializationStream);
            serializationStream.Position = 0;
            var actual = await reader.ReadAsync<DateTimeOffset>(serializationStream);
            
            Assert.Equal(expected, actual);
        }
        
        // TODO: Test timestamp, problem: same C# as for date

        [Fact]
        public async Task TestString()
        {
            const string expected = "serialize this!";
            var writer = CreateGraphBinaryWriter();
            var reader = CreateGraphBinaryReader();
            var serializationStream = new MemoryStream();
            
            await writer.WriteAsync(expected, serializationStream);
            serializationStream.Position = 0;
            var actual = await reader.ReadAsync<string>(serializationStream);
            
            Assert.Equal(expected, actual);
        }
        
        [Theory]
        [InlineData("serialize this!", "serialize that!", "serialize that!", "stop telling me what to serialize")]
        [InlineData(1, 2, 3, 4, 5)]
        [InlineData(0.1, 1.1, 2.5, double.NaN)]
        [InlineData(0.1f, 1.1f, 2.5f, float.NaN)]
        public async Task TestHomogeneousList(params object[] listMembers)
        {
            var expected = new List<object>(listMembers);
            var writer = CreateGraphBinaryWriter();
            var reader = CreateGraphBinaryReader();
            var serializationStream = new MemoryStream();
            
            await writer.WriteAsync(expected, serializationStream);
            serializationStream.Position = 0;
            var actual = await reader.ReadAsync<List<object>>(serializationStream);
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task TestHomogeneousTypeSafeList()
        {
            var expected = new List<string> {"test", "123"};
            var writer = CreateGraphBinaryWriter();
            var reader = CreateGraphBinaryReader();
            var serializationStream = new MemoryStream();
            
            await writer.WriteAsync(expected, serializationStream);
            serializationStream.Position = 0;
            var actual = await reader.ReadAsync<List<string>>(serializationStream);
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task TestHeterogeneousList()
        {
            var expected = new List<object>
                {"serialize this!", 0, "serialize that!", "serialize that!", 1, "stop telling me what to serialize", 2};
            var writer = CreateGraphBinaryWriter();
            var reader = CreateGraphBinaryReader();
            var serializationStream = new MemoryStream();
            
            await writer.WriteAsync(expected, serializationStream);
            serializationStream.Position = 0;
            var actual = await reader.ReadAsync<List<object>>(serializationStream);
            
            Assert.Equal(expected, actual);
        }

        // [Fact]
        // public void TypeTest()
        // {
        //     var listSerializer = new ListSerializer<IList<object>, object>();
        //     var first = listSerializer.GetType().GetGenericArguments().First();
        //     Assert.Equal(typeof(IList), first);
        //     
        //     Assert.Equal(typeof(ListSerializer<,>), listSerializer.GetType().GetGenericTypeDefinition());
        //
        //     var listType = typeof(List<string>);
        //     var genericSerializerType = listSerializer.GetType().GetGenericTypeDefinition();
        //     Assert.Equal(typeof(ListSerializer<,>), genericSerializerType);
        //     var listSerializerType = genericSerializerType.MakeGenericType(listType);
        //     Assert.Equal(typeof(ListSerializer<List<string>, string>), listSerializerType);
        //     var concreteListSerializer = Activator.CreateInstance(listSerializerType);
        //     
        //     Assert.Equal(typeof(ListSerializer<List<string>, string>), concreteListSerializer.GetType());
        // }
        
        private static GraphBinaryWriter CreateGraphBinaryWriter()
        {
            return new GraphBinaryWriter();
        }
        
        private static GraphBinaryReader CreateGraphBinaryReader()
        {
            return new GraphBinaryReader();
        }
    }
}