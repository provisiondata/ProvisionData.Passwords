// Heavily modified version of https://github.com/grmartin/Mersenne-Twister/blob/master/MersenneTwister/mt19937ar.cs
using System;

namespace ProvisionData.Passwords
{
    public class MersenneTwister : IRandomNumberGenerator
    {
        private class MTState
        {
            public Int32 MTI;

            public UInt32[] MT = new UInt32[Nuplim + 1];
            public MTState()
            {
                MTI = Nplus1;
            }
        }

        private readonly MTState _state = new MTState();
        
        private const Int32 N = 624;
        private const Int32 M = 397;
        private const UInt32 MATRIX_A = 0x9908b0dfu;
        private const UInt32 UPPER_MASK = 0x80000000u;
        private const UInt32 LOWER_MASK = 0x7fffffffu;
        private const Int32 Nuplim = N - 1;
        private const Int32 Nplus1 = N + 1;
        private const Int32 NuplimLess1 = Nuplim - 1;

        private const Int32 NuplimLessM = Nuplim - M;

        private readonly UInt32[] _mag01 = { 0u, MATRIX_A };

        private const UInt32 KDefaultSeed = 5489;

        private const Double K2_31 = 2147483648.0;
        private const Double K2_32 = 2.0 * K2_31;
        private const Double K2_32b = K2_32 - 1.0;
        private const Double KMT_1 = 1.0 / K2_32b;

        public MersenneTwister()
        {
            // seed rng from the system clock by making a new instance
            var _rng = new Random();

            // initialize MTRandom with a pseudo-random Integer from rng
            InitGenRand(Convert.ToUInt32(Convert.ToInt64(_rng.Next(Int32.MinValue, Int32.MaxValue)) - Convert.ToInt64(Int32.MinValue)));
        }

        // initialize the MTRandom PRNG with seed
        public MersenneTwister(UInt32 seed)
        {
            InitGenRand(seed);
        }

        public void InitGenRand(UInt32 seed)
        {
            _state.MT[0] = (seed & 0xffffffffu);
            for (_state.MTI = 1; _state.MTI <= Nuplim; _state.MTI++)
            {
                var tt = _state.MT[_state.MTI - 1];
                _state.MT[_state.MTI] = Convert.ToUInt32((1812433253uL * Convert.ToUInt64(tt ^ (tt >> 30)) + Convert.ToUInt64(_state.MTI)) & 0xffffffffuL);
            }
        }

        public UInt32 GetUInt32()
        {
            UInt32 y;

            if ((_state.MTI >= N))
            {

                if (_state.MTI == Nplus1)
                {
                    InitGenRand(KDefaultSeed);
                }

                Int32 kk;
                for (kk = 0; kk <= (NuplimLessM); kk++)
                {
                    y = (_state.MT[kk] & UPPER_MASK) | (_state.MT[kk + 1] & LOWER_MASK);
                    _state.MT[kk] = _state.MT[kk + M] ^ (y >> 1) ^ _mag01[Convert.ToInt32(y & 1u)];
                }

                for ( ; kk <= NuplimLess1; kk++)
                {
                    y = (_state.MT[kk] & UPPER_MASK) | (_state.MT[kk + 1] & LOWER_MASK);
                    _state.MT[kk] = _state.MT[kk + (M - N)] ^ (y >> 1) ^ _mag01[Convert.ToInt32(y & 1u)];
                }

                y = (_state.MT[Nuplim] & UPPER_MASK) | (_state.MT[0] & LOWER_MASK);
                _state.MT[N - 1] = _state.MT[M - 1] ^ (y >> 1) ^ _mag01[Convert.ToInt32(y & 1u)];

                _state.MTI = 0;
            }


            y = _state.MT[_state.MTI];
            _state.MTI += 1;

            y ^= (y >> 11);

            y ^= (y << 7) & 0x9d2c5680u;

            y ^= (y << 15) & 0xefc60000u;

            return y ^ (y >> 18);
        }

        public Int32 GetInt32()
        {
            return Convert.ToInt32(GetUInt32() >> 1);
        }

        public Double GetDouble()
        {
            return GetUInt32() * KMT_1;
        }

        public UInt32 GetUInt32(UInt32 max)
        {
            // Returns a UInteger in [0,n] for 0 <= max < 2^32
            // Its lowest value is : 0
            // Its highest value is: 4294967295 but <= max

            // Translated by Ron Charlton from C++ file 'MersenneTwister.h' where it is named
            // MTRand::randInt(const uint32& n), and has the following comments:
            //-----
            // Mersenne Twister random number generator -- a C++ class MTRand
            // Based on code by Makoto Matsumoto, Takuji Nishimura, and Shawn Cokus
            // Richard J. Wagner  v1.0  15 May 2003  rjwagner@writeme.com

            // Copyright (C) 1997 - 2002, Makoto Matsumoto and Takuji Nishimura,
            // Copyright (C) 2000 - 2003, Richard J. Wagner
            // All rights reserved.                          
            //-----
            // MersenneTwister.h can be found at
            // http://www-personal.umich.edu/~wagnerr/MersenneTwister.html.

            // Find which bits are used in max
            // Optimized by Magnus Jonsson (magnus@smartelectronix.com)
            var used = max;
            used |= (used >> 1);
            used |= (used >> 2);
            used |= (used >> 4);
            used |= (used >> 8);
            used |= (used >> 16);

            // Draw numbers until one is found in [0,n]
            UInt32 i;
            do
            {
                i = GetUInt32() & used;
                // toss unused bits to shorten search
            } while (i > max);

            return i;
        }

        public UInt32 GetUInt32(UInt32 min, UInt32 max)
        {
            if (min < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(min), "Must not be less than 0.");
            }

            if (min > max)
            {
                throw new ArgumentException("The value of the 'min' parameter must not be greater than the value of the 'max' parameter.", nameof(min));
            }

            return min + GetUInt32(max - min);
        }

        public Int32 GetInt32(Int32 max)
        {
            // Returns a UInteger in [0,n] for 0 <= max < 2^32
            // Its lowest value is : 0
            // Its highest value is: 4294967295 but <= max

            // Translated by Ron Charlton from C++ file 'MersenneTwister.h' where it is named
            // MTRand::randInt(const uint32& n), and has the following comments:
            //-----
            // Mersenne Twister random number generator -- a C++ class MTRand
            // Based on code by Makoto Matsumoto, Takuji Nishimura, and Shawn Cokus
            // Richard J. Wagner  v1.0  15 May 2003  rjwagner@writeme.com

            // Copyright (C) 1997 - 2002, Makoto Matsumoto and Takuji Nishimura,
            // Copyright (C) 2000 - 2003, Richard J. Wagner
            // All rights reserved.                          
            //-----
            // MersenneTwister.h can be found at
            // http://www-personal.umich.edu/~wagnerr/MersenneTwister.html.

            // Find which bits are used in max
            // Optimized by Magnus Jonsson (magnus@smartelectronix.com)
            var used = max;
            used |= (used >> 1);
            used |= (used >> 2);
            used |= (used >> 4);
            used |= (used >> 8);
            //used |= (used >> 16);

            // Draw numbers until one is found in [0,n]
            Int32 i;
            do
            {
                i = GetInt32() & used;
                // toss unused bits to shorten search
            } while (i > max);

            return i;
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

            return min + GetInt32(Convert.ToInt32(max - min));
        }

        public UInt64 GetUInt64()
        {
            // Returns an unsigned long in the range [0,2^64-1]
            // Its lowest value is : 0
            // Its highest value is: 18446744073709551615
            //
            // Written by Ron Charlton, 2008-09-23.

            return Convert.ToUInt64(GetUInt32()) | (Convert.ToUInt64(GetUInt32()) << 32);
        }

        public Int64 GetInt64()
        {
            // Returns an Int64 in the range [0,2^63-1]
            // Its lowest value is : 0
            // Its highest value is: 9223372036854775807

            return Math.Abs(Convert.ToInt64(GetUInt32()) | (Convert.ToInt64(GetUInt32()) << 32));
        }
    }
}
