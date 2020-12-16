using System;
using System.IO;
using System.Threading.Tasks;

namespace Gremlin.Net.Structure.IO.GraphBinary.Types
{
    public class DateTimeOffsetSerializer : SimpleTypeSerializer<DateTimeOffset>
    {
        public static readonly DateTimeOffsetSerializer DateSerializer = new DateTimeOffsetSerializer(DataType.Date);
        
        public DateTimeOffsetSerializer(DataType dataType) : base(dataType)
        {
        }

        protected override async Task WriteValueAsync(DateTimeOffset value, Stream stream, GraphBinaryWriter writer)
        {
            await stream.WriteLongAsync(value.ToUnixTimeMilliseconds()).ConfigureAwait(false);
        }

        protected override async Task<DateTimeOffset> ReadValueAsync(Stream stream, GraphBinaryReader reader)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(await stream.ReadLongAsync().ConfigureAwait(false));
        }
    }
}