using System;
using System.Security.Cryptography;

namespace ProvisionData.Passwords
{
    public class StockRng : IRandomNumberGenerator
    {
        private readonly Random _random;

        public StockRng()
        {
            _random = new Random();
        }

        public Int32 GetInt32()
        {
            return _random.Next();
        }

        public Int32 GetInt32(Int32 max)
        {
            return _random.Next(max);
        }

        public Int32 GetInt32(Int32 min, Int32 max)
        {
            return _random.Next(min, max);
        }

        public Int64 GetInt64()
        {
            return Math.BigMul(_random.Next(), _random.Next());
        }

        public Double GetDouble()
        {
            return _random.NextDouble();
        }
    }

}
