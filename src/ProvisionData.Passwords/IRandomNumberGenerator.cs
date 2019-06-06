using System;

namespace ProvisionData.Passwords
{
    public interface IRandomNumberGenerator
    {
        Int32 GetInt32();
        Int32 GetInt32(Int32 max);

        /// <summary>
        /// Generate a random integer between lower inclusive and upper inclusive for 0 <= <paramref name="min"/> <= <paramref name="max"/> <= 2147483647 (2^31-1).
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns>a <see cref="Int32"/> in the range [<paramref name="min"/>,<paramref name="max"/>]</returns>
        Int32 GetInt32(Int32 min, Int32 max);

        Int64 GetInt64();

        /// <summary>
        /// Returns a random number between 0.0 and 1.0.
        /// </summary>
        /// <returns>A double-precision floating point number greater than or equal to 0.0, and less than 1.0.</returns>
        Double GetDouble();
    }
}
