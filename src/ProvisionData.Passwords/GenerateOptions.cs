using System;
using System.Collections.Generic;
using System.Linq;

namespace ProvisionData.Passwords
{
    public class GenerateOptions
    {
        public const String Digits = @"0123456789";
        public const String Minus = @"-";
        public const String Underscore = @"_";
        public const String Space = @" ";
        public const String Special = @"~!@#$%^&*+=:;,.?";
        public const String Brackets = @"[]{}()<>";

        public static Int32 MinLength => 5;
        public static Int32 MaxLength => 30;
        public static Int32 MaxCount => 10;

        private Int32 _length;
        private Int32 _count;

        public GenerateOptions()
            : this(new CspRng())
        {
        }

        public GenerateOptions(IRandomNumberGenerator rng)
        {
            Count = 1;
            Length = MaxCount;
            Frequency = 15;
            UseUppercase = true;
            RNG = rng;
        }

        public IRandomNumberGenerator RNG { get; }

        public Int32 Length {
            get => _length;
            set {
                if (value < MinLength)
                {
                    _length = MinLength;
                }
                else if (value > MaxLength)
                {
                    _length = MaxLength;
                }
                else
                {
                    _length = value;
                }
            }
        }

        public Int32 Count {
            get => _count;
            set {
                if (value < 1)
                {
                    _count = 1;
                }
                else if (value > MaxCount)
                {
                    _count = MaxCount;
                }
                else
                {
                    _count = value;
                }
            }
        }

        public Int32 Frequency { get; set; }
        public Boolean UseBrackets { get; set; }
        public Boolean UseDigits { get; set; }
        public Boolean UseMinus { get; set; }
        public Boolean UseSpace { get; set; }
        public Boolean UseSpecial { get; set; }
        public Boolean UseUnderscore { get; set; }
        public Boolean UseUppercase { get; set; }

        public Boolean ApplyModifiers {
            get {
                return Modfiers.Any();
            }
        }

        public IEnumerable<IModifier> Modfiers {
            get {
                if (UseBrackets)
                {
                    yield return new Modifier("Brackets", x => GetRandomChar(Brackets));
                }

                if (UseDigits)
                {
                    yield return new Modifier("Digits", x => GetRandomChar(Digits));
                }

                if (UseMinus)
                {
                    yield return new Modifier("Minus", x => GetRandomChar(Minus));
                }

                if (UseSpace)
                {
                    yield return new Modifier("Space", x => GetRandomChar(Space));
                }

                if (UseSpecial)
                {
                    yield return new Modifier("Special", x => GetRandomChar(Special));
                }

                if (UseUnderscore)
                {
                    yield return new Modifier("Underscore", x => GetRandomChar(Underscore));
                }

                if (UseUppercase)
                {
                    yield return new Modifier("Uppercase", Char.ToUpper);
                }
            }
        }

        private Char GetRandomChar(String chars)
        {
            return chars[RNG.GetInt32(chars.Length - 1)];
        }
    }
}
