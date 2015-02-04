using System;
using System.Collections.Generic;
using System.Linq;
using BddSharp.Engine.Enums;
using BddSharp.Engine.Extensions;

namespace BddSharp.Engine.Tests
{
    public class TestResult
    {
        public Type SuiteType { get; private set; }

        public List<string> Conditions { get; private set; }
        public List<string> Events { get; private set; }
        public List<Outcome> Outcomes { get; private set; }

        public TestResultEnum Result
        {
            get
            {
                if (Outcomes.IsNullOrEmpty())
                    return TestResultEnum.NotRun;

                return Outcomes.Any(a => a.FirstAssertionFailure.HasValue) ? TestResultEnum.Failure : TestResultEnum.Success;
            }
        }

        public bool? Succeeded
        {
            get
            {
                var result = Result;

                return result == TestResultEnum.NotRun ? (bool?)null : result == TestResultEnum.Success;
            }
        }

        internal TestResult(Type suiteType)
        {
            SuiteType = suiteType;

            Conditions = new List<string>();
            Events = new List<string>();
            Outcomes = new List<Outcome>();
        }
    }
}
