using System.IO;
using System.Threading.Tasks;
using Gremlin.Net.Structure.IO.GraphBinary;
using Xunit;

namespace Gremlin.Net.UnitTest.Structure.IO.GraphBinary
{
    public class GraphBinaryTests
    {
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