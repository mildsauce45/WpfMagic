
namespace BddSharp.Engine.Tests
{
    public class Outcome
    {
        public string Name { get; private set; }

        public int Assertions { get; internal set; }
        public int? FirstAssertionFailure { get; internal set; }

        public bool Succeeded
        {
            get { return Assertions > 0 && !FirstAssertionFailure.HasValue; }
        }

        public Outcome(string name)
        {
            this.Name = name;
        }
    }
}
