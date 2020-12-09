using System;

namespace Gremlin.Net.Structure.IO.GraphBinary
{
    public class DataType : IEquatable<DataType>
    {
        public static DataType Int = new DataType(0x01);

        private DataType(int code)
        {
            TypeCode = (byte) code;
        }
        
        public byte TypeCode { get; }

        public static DataType FromTypeCode(int code)
        {
            return new DataType(code);
        }

        public bool Equals(DataType other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return TypeCode == other.TypeCode;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DataType) obj);
        }

        public override int GetHashCode()
        {
            return TypeCode.GetHashCode();
        }
    }
}