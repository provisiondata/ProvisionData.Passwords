using System;
using System.IO;

namespace ProvisionData.Passwords
{
    public class GraphCreator
    {
        /// <summary>
        /// Creates a <see cref="Graph"/> from a <see cref="Stream"/>.
        /// </summary>
        public Graph Create(Stream stream)
        {
            var triples = new Int32[26, 26, 26];
            var doubles = new Int32[26, 26];
            var singles = new Int32[26];
            Int64 totalLetters = 0;

            Int32 k1, k2, k3;
            Int32 c1, c2, c3;

            for (c1 = 0; c1 < 26; c1++)
            {
                // Explicitly Initialize arrays to zero
                singles[c1] = 0;
                for (c2 = 0; c2 < 26; c2++)
                {
                    doubles[c1, c2] = 0;
                    for (c3 = 0; c3 < 26; c3++)
                    {
                        triples[c1, c2, c3] = 0;
                    }
                }
            }

            k2 = -1;                /* k1, k2 are coords of previous letters */
            k1 = -1;
            k3 = stream.ReadByte();

            while (k3 != -1)
            {
                if (k3 > 'Z')
                {
                    k3 -= 'a';  /* map from a-z to 0-25 */
                }
                else
                {
                    k3 -= 'A';  /* map from A-Z to 0-25 */
                }

                // If k3 is a valid letter (a-z)...
                if (k3 >= 0 && k3 <= 25)
                {
                    // And we have two letters previously...
                    if (k1 >= 0 && k2 > 0)
                    {
                        // Increment the triple count
                        triples[k1, k2, k3]++;

                        // Increment the total characters count
                        totalLetters++;
                    }

                    // Or maybe only one letter previously...
                    if (k2 >= 0)
                    {
                        // Increment the double count
                        doubles[k2, k3]++;
                    }

                    // Increment the singles count
                    singles[k3]++;

                    // Shift
                    k1 = k2;
                    k2 = k3;
                }

                // Next character please.
                k3 = stream.ReadByte();
            }

            return new Graph("Untitled", DateTime.Now, totalLetters, singles, doubles, triples);

        }

    }
}
