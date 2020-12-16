using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Gremlin.Net.Structure.IO.GraphBinary.Types
{
    public class ListSerializer<TList> : SimpleTypeSerializer<TList>
        where TList : IList, new()
    {
        public ListSerializer() : base(DataType.List)
        {
        }

        protected override async Task WriteValueAsync(TList value, Stream stream, GraphBinaryWriter writer)
        {
            await stream.WriteIntAsync(value.Count).ConfigureAwait(false);
            
            foreach (var item in value)
            {
                await writer.WriteAsync(item, stream).ConfigureAwait(false);
            }
        }

        protected override async Task<TList> ReadValueAsync(Stream stream, GraphBinaryReader reader)
        {
            var length = await stream.ReadIntAsync().ConfigureAwait(false);
            var result = new TList();
            for (var i = 0; i < length; i++)
            {
                result.Add(await reader.ReadAsync<object>(stream).ConfigureAwait(false));
            }

            return result;
        }
    }
}