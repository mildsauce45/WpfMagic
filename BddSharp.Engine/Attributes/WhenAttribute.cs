using System;

namespace BddSharp.Engine.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
    public class WhenAttribute : Attribute
    {
        public string Event { get; private set; }

        public WhenAttribute(string eventName)
        {
            Event = eventName;
        }
    }
}
