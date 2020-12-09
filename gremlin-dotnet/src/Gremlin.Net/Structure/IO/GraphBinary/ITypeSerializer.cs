using System.IO;
using System.Threading.Tasks;

namespace Gremlin.Net.Structure.IO.GraphBinary
{
    public interface ITypeSerializer
    {
        DataType DataType { get; }
    }
    
    public interface ITypeSerializer<T> : ITypeSerializer
    {
        Task WriteAsync(T value, Stream stream, GraphBinaryWriter writer);
        Task<T> ReadAsync(Stream stream, GraphBinaryReader reader);
    }
}