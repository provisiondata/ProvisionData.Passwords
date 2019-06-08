using System;
using System.Collections.Generic;

namespace ProvisionData.Passwords
{
    public interface IPasswordGenerator
    {
        String Name { get; }
        String Generate(Int32 length = 8);
        IEnumerable<String> Generate(Int32 count, Int32 minLength = 4, Int32 maxLength = 8);
    }
}
