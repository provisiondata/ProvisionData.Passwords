using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProvisionData.Passwords
{
    /// <summary>
    /// Summary description for CharacterGraphCreator.
    /// </summary>
    public class PronounceablePasswordGenerator
    {
        public static readonly String Lowercase = @"abcdefghijklmnopqrstuvwxyz";

        private readonly GenerateOptions _options;
        private readonly Graph _graph;

        public PronounceablePasswordGenerator()
            : this(new GenerateOptions(), Graph.Default)
        {
        }

        public PronounceablePasswordGenerator(GenerateOptions options, Graph graph)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _graph = graph ?? throw new ArgumentNullException("graph");
        }

        public IEnumerable<String> Generate()
        {
            var length = _options.Length;
            var count = _options.Count;
            var passwords = new List<String>(count);

            // Current Index into the Triples
            Int32 c1, c2, c3;
            Int32 index;
            Int64 accumulator;
            Int64 target;

            for (var counter = 0; counter < count; counter++)
            {

                var password = new StringBuilder(length);

                // Pick a random starting point.
                target = (Int64)(_options.RNG.GetDouble() * _graph.Sigma); // weight by sum of frequencies
                accumulator = 0;
                for (c1 = 0; c1 < 26; c1++)
                {
                    for (c2 = 0; c2 < 26; c2++)
                    {
                        for (c3 = 0; c3 < 26; c3++)
                        {
                            accumulator += _graph.Triples[c1, c2, c3];
                            if (accumulator > target)
                            {
                                password.Append(Lowercase.Substring(c1, 1));
                                password.Append(Lowercase.Substring(c2, 1));
                                password.Append(Lowercase.Substring(c3, 1));
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
                    c1 = Lowercase.IndexOf(password.ToString().Substring(index - 2, 1));
                    c2 = Lowercase.IndexOf(password.ToString().Substring(index - 1, 1));
                    accumulator = 0;
                    for (c3 = 0; c3 < 26; c3++)
                    {
                        accumulator += _graph.Triples[c1, c2, c3];
                    }

                    if (accumulator == 0)
                    {
                        break; // exit while loop
                    }
                    target = (Int64)(_options.RNG.GetDouble() * accumulator);
                    accumulator = 0;
                    for (c3 = 0; c3 < 26; c3++)
                    {
                        accumulator += _graph.Triples[c1, c2, c3];
                        if (accumulator > target)
                        {
                            password.Append(Lowercase.Substring(c3, 1));
                            c3 = 26; // break for loop
                        } // if sum
                    } // for c3
                    index++;
                } // while nchar

                passwords.Add(_options.ApplyModifiers ? ModifyPassword(_options, password.ToString()) : password.ToString());
            }

            return passwords;
        }

        private String ModifyPassword(GenerateOptions options, String password)
        {
            var modifiers = options.Modfiers.ToArray();

            var chars = password.ToCharArray();
            for (var i = 0; i < password.Length; i++)
            {
                if (_options.RNG.GetInt32(100) <= options.Frequency)
                {
                    var m = modifiers[_options.RNG.GetInt32(modifiers.Length - 1)];
                    chars[i] = m.Modify(chars[i]);
                }
            }

            return new String(chars);
        }
    }
}
