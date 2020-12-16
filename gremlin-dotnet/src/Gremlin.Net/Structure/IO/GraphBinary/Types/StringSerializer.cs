using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Gremlin.Net.Structure.IO.GraphBinary.Types
{
    public class StringSerializer : SimpleTypeSerializer<string>
    {
        public StringSerializer() : base(DataType.String)
        {
        }

        protected override async Task WriteValueAsync(string value, Stream stream, GraphBinaryWriter writer)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            var length = bytes.Length;
            await stream.WriteIntAsync(length).ConfigureAwait(false);
            await stream.WriteAsync(bytes, 0, length).ConfigureAwait(false);
        }

        protected override async Task<string> ReadValueAsync(Stream stream, GraphBinaryReader reader)
        {
            var length = await stream.ReadIntAsync().ConfigureAwait(false);
            var bytes = new byte[length];
            await stream.ReadAsync(bytes, 0, length).ConfigureAwait(false);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}