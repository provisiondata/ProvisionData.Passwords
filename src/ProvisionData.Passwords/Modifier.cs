using System;

namespace ProvisionData.Passwords
{
    public interface IModifier
    {
        Char Modify(Char c);
    }

    public class Modifier : IModifier
    {
        private readonly Func<Char, Char> _action;
        private readonly String _name;

        public Modifier(String name, Func<Char, Char> action)
        {
            _name = name;
            _action = action;
        }

        public Char Modify(Char c)
        {
            return _action(c);
        }

        public override String ToString()
        {
            return _name;
        }
    }
}
