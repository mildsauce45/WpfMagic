using System;

namespace BddSharp.Engine.Attributes
{
    public class ScenarioAttribute : Attribute
    {
        public string Description { get; private set; }

        public ScenarioAttribute(string desc)
        {
            this.Description = desc;
        }
    }
}
