using System;

namespace BddSharp.Engine.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
    public class GivenAttribute : Attribute
    {
        public string Condition { get; private set; }

        public GivenAttribute(string condition)
        {
            this.Condition = condition;
        }
    }
}
