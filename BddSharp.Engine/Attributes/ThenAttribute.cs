using System;

namespace BddSharp.Engine.Attributes
{
    public class ThenAttribute : Attribute
    {
        public string Outcome { get; private set; }
        public int Ordinal { get; private set; }

        public ThenAttribute(string outcome, int ordinal = 0)
        {
            this.Outcome = outcome;
            this.Ordinal = ordinal;
        }
    }
}
