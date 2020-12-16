using System;

namespace Gremlin.Net.Structure.IO.GraphBinary
{
    public class DataType : IEquatable<DataType>
    {
        public static DataType Int = new DataType(0x01);
        public static DataType Long = new DataType(0x02);
        public static DataType String = new DataType(0x03);
        public static DataType Date = new DataType(0x04);
        
        public static DataType Double = new DataType(0x07);
        public static DataType Float = new DataType(0x08);
        public static DataType List = new DataType(0x09);
        
        public static DataType UnspecifiedNull = new DataType(0xFE);

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
            if (obj.GetType() != GetType()) return false;
            return Equals((DataType) obj);
        }

        public override int GetHashCode()
        {
            return TypeCode.GetHashCode();
        }

        public static bool operator ==(DataType first, DataType second)
        {
            if (ReferenceEquals(null, first))
            {
                if (ReferenceEquals(null, second))
                {
                    return true;
                }

                return false;
            }

            return first.Equals(second);
        }

        public static bool operator !=(DataType first, DataType second)
        {
            return !(first == second);
        }
    }
}