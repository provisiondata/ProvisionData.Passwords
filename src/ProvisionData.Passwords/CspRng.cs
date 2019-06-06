using System;
using System.Security.Cryptography;

namespace ProvisionData.Passwords
{
    public class CspRng : IRandomNumberGenerator
    {
        private const Double K2_31 = 2147483648.0;
        private const Double K2_32 = 2.0 * K2_31;
        private const Double K2_32b = K2_32 - 1.0;
        private const Double KMT_1 = 1.0 / K2_32b;

        private static readonly RNGCryptoServiceProvider _csp = new RNGCryptoServiceProvider();

        public Int32 GetInt32()
        {
            var bytes = new Byte[4];
            _csp.GetBytes(bytes);
            return Math.Abs(BitConverter.ToInt32(bytes, 0));
        }

        public UInt32 GetUInt32()
        {
            var bytes = new Byte[4];
            _csp.GetBytes(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }

        public Int32 GetInt32(Int32 max)
        {
            var used = max;
            used |= (used >> 1);
            used |= (used >> 2);
            used |= (used >> 4);
            used |= (used >> 8);

            Int32 number;
            do
            {
                number = GetInt32() & used;
                // toss unused bits to shorten search
            } while (number > max);

            return number;
        }

        public Int32 GetInt32(Int32 min, Int32 max)
        {
            if (min < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(min), "Must not be less than 0.");
            }

            if (min > max)
            {
                throw new ArgumentException("The value of the 'min' parameter must not be greater than the value of the 'max' parameter.", nameof(min));
            }

            return min + GetInt32(max - min);
        }

        public Int64 GetInt64()
        {
            var bytes = new Byte[8];
            _csp.GetBytes(bytes);
            return Math.Abs(BitConverter.ToInt64(bytes, 0));
        }

        public Double GetDouble()
        {
            return GetUInt32() * KMT_1;
        }
    }
}
