using BddSharp.Engine.Tests;
using WpfMagic.Mvvm;

namespace BddSharp.TestRunner.Models
{
    public class Test : NotifyableObject
    {
        private string givenDescription;
        private string whenDescription;

        private bool testRun;

        public string Name { get; private set; }
        public string Scenario { get; private set; }

        public Specification Specification { get; private set; }

        public string GivenDescription
        {
            get { return givenDescription; }
            set
            {
                givenDescription = value;
                NotifyPropertyChanged(() => GivenDescription);
            }
        }

        public string WhenDescription
        {
            get { return whenDescription; }
            set
            {
                whenDescription = value;
                NotifyPropertyChanged(() => WhenDescription);
            }
        }

        public bool TestRun
        {
            get { return testRun; }
            private set
            {
                testRun = value;
                NotifyPropertyChanged(() => TestRun);
            }
        }

        public Test(Specification spec, string scenario)
        {
            this.Specification = spec;

            this.Name = spec.TestName;
            this.Scenario = scenario;
        }

        public void Run()
        {
            Specification.Run();

            TestRun = true;

            GivenDescription = string.Join(" AND ", Specification.TestResult.Conditions);
            WhenDescription = string.Join(" AND ", Specification.TestResult.Events);

            NotifyPropertyChanged(() => Specification);
        }        
    }
}
