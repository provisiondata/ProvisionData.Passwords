using System;
using System.Collections.Generic;
using System.Text;

namespace ProvisionData.Passwords
{
    /// <summary>
    /// Summary description for CharacterGraphCreator.
    /// </summary>
    public class PronounceablePasswordGenerator : IPasswordGenerator
    {
        public const Char DefaultSeparator = ' ';
        public const String Digits = @"0123456789";
        public const String Minus = @"-";
        public const String Underscore = @"_";
        public const String Space = @" ";
        public const String Special = @"~!@#$%^&*+=:;,.?";
        public const String Brackets = @"[]{}()<>";

        public static readonly Int32 MinLength = 5;
        public static readonly Int32 MaxLength = 30;
        public static readonly Int32 MaxCount = 30;

        private static readonly String _lowercase = @"abcdefghijklmnopqrstuvwxyz";

        public PronounceablePasswordGenerator()
            : this(new CspRng(), Graph.Default, DefaultSeparator)
        {
        }

        public PronounceablePasswordGenerator(IRandomNumberGenerator rng, Graph graph, Char separator)
        {
            RNG = rng ?? throw new ArgumentNullException(nameof(rng));
            Graph = graph ?? throw new ArgumentNullException(nameof(graph));
            Separator = separator;
        }

        public IRandomNumberGenerator RNG { get; set; }

        public Graph Graph { get; set; }

        public Char Separator { get; set; }

        public IEnumerable<String> Generate(Int32 count, Int32 minLength = 4, Int32 maxLength = 8)
        {
            count = Math.Clamp(count, 1, 10);

            for (var index = 0; index < count; index++)
            {
                yield return Generate(RNG.GetInt32(minLength, maxLength));
            }
        }

        public String Generate(Int32 length = 8)
        {
            // Current Index into the Triples
            Int32 c1, c2, c3;
            Int32 index;
            Int64 accumulator;
            Int64 target;

            length = Math.Clamp(length, MinLength, MaxLength);

            var password = new StringBuilder(length);

            // Pick a random starting point.
            target = (Int64)(RNG.GetDouble() * Graph.Sigma); // weight by sum of frequencies
            accumulator = 0;
            for (c1 = 0; c1 < 26; c1++)
            {
                for (c2 = 0; c2 < 26; c2++)
                {
                    for (c3 = 0; c3 < 26; c3++)
                    {
                        accumulator += Graph.Triples[c1, c2, c3];
                        if (accumulator > target)
                        {
                            password.Append(_lowercase.Substring(c1, 1));
                            password.Append(_lowercase.Substring(c2, 1));
                            password.Append(_lowercase.Substring(c3, 1));
                            c1 = 26; // Found start. Break all 3 loops.
                            c2 = 26;
                            c3 = 26;
                        } // if sum
                    } // for c3
                } // for c2
            } // for c1

            // Now do a random walk.
            index = 3;
            while (index < length)
            {
                c1 = _lowercase.IndexOf(password.ToString().Substring(index - 2, 1));
                c2 = _lowercase.IndexOf(password.ToString().Substring(index - 1, 1));
                accumulator = 0;
                for (c3 = 0; c3 < 26; c3++)
                {
                    accumulator += Graph.Triples[c1, c2, c3];
                }

                if (accumulator == 0)
                {
                    break; // exit while loop
                }
                target = (Int64)(RNG.GetDouble() * accumulator);
                accumulator = 0;
                for (c3 = 0; c3 < 26; c3++)
                {
                    accumulator += Graph.Triples[c1, c2, c3];
                    if (accumulator > target)
                    {
                        password.Append(_lowercase.Substring(c3, 1));
                        c3 = 26; // break for loop
                    } // if sum
                } // for c3
                index++;
            } // while nchar

            return password.ToString();
        }
    }
}
