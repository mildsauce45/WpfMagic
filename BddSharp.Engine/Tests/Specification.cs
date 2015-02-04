using System;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using BddSharp.Engine.Attributes;
using BddSharp.Engine.Extensions;

namespace BddSharp.Engine.Tests
{
    public abstract class Specification
    {
        #region Private Variables

        private Type suiteType;
        private MethodInfo[] methods;
        private Outcome currentOutcome;

        #endregion

        #region Properties

        public dynamic World { get; private set; }

        public string TestName { get; private set; }
        public TestResult TestResult { get; internal set; }

        #endregion

        #region Constructors

        public Specification()
        {
            suiteType = this.GetType();
            methods = suiteType.GetMethods();

            TestName = suiteType.Name.Replace("Test", string.Empty).SeparateOnCamelCase();

            World = new ExpandoObject();
        }

        #endregion

        #region Test Running Methods

        public void Run()
        {
            TestResult = new TestResult(suiteType);

            RunGivens();
            RunWhens();
            RunThens();
        }

        private void RunGivens()
        {
            var givenMethods = methods.Where(mi => mi.GetCustomAttribute<GivenAttribute>() != null);

            givenMethods.ForEach(gm =>
            {
                var attr = gm.GetCustomAttribute<GivenAttribute>();

                TestResult.Conditions.Add(attr.Condition);

                gm.Invoke(this, null);
            });
        }

        private void RunWhens()
        {
            var whenMethods = methods.Where(mi => mi.GetCustomAttribute<WhenAttribute>() != null);

            whenMethods.ForEach(wm =>
            {
                var attr = wm.GetCustomAttribute<WhenAttribute>();

                TestResult.Events.Add(attr.Event);

                wm.Invoke(this, null);
            });
        }

        private void RunThens()
        {
            var thenMethods = methods.Where(mi => mi.GetCustomAttribute<ThenAttribute>() != null);

            // According to the documentation, you cannot count on the order of the methods being returned.
            // However there is an optional parameter you can pass to the attribute to order them appropriately.
            var orderedMethods = thenMethods.OrderBy(mi => mi.GetCustomAttribute<ThenAttribute>().Ordinal).ToList();

            orderedMethods.ForEach(tm =>
            {
                var attr = tm.GetCustomAttribute<ThenAttribute>();

                currentOutcome = new Outcome(attr.Outcome);

                try
                {
                    // Run the logic that is supposed to run before each test
                    BeforeEach();

                    tm.Invoke(this, null);

                    // Run the logic that is supposed to run after each test
                    AfterEach();
                }
                catch
                {
                    currentOutcome.FirstAssertionFailure = 0;
                }

                TestResult.Outcomes.Add(currentOutcome);

                // Clear the reference to the outcome for safety
                currentOutcome = null;
            });
        }

        #endregion

        #region Before / After Methods

        protected virtual void BeforeEach()
        {
        }

        protected virtual void AfterEach()
        {
        }

        #endregion

        #region Assertion Methods

        public void AssertTrue(bool assertion/*, [CallerMemberName] string methodName = ""*/)
        {
            // Increment the number of asssertions this test has seen so far
            currentOutcome.Assertions++;
            
            if (!assertion)
            {
                // We only care about the first failure
                if (!currentOutcome.FirstAssertionFailure.HasValue)
                    currentOutcome.FirstAssertionFailure = currentOutcome.Assertions;
            }
        }

        public void AssertFalse(bool assertion)
        {
            AssertTrue(!assertion);
        }

        public void AssertNull(object obj)
        {
            AssertTrue(obj == null);
        }

        public void AssertNotNUll(object obj)
        {
            AssertTrue(obj != null);
        }

        public void AssertNaN(double number)
        {
            AssertTrue(Double.IsNaN(number));
        }

        public void AssertIsEmpty(string str)
        {
            AssertTrue(string.IsNullOrWhiteSpace(str));
        }

        public void AssertIsNotEmpty(string str)
        {
            AssertTrue(!string.IsNullOrWhiteSpace(str));
        }

        /// <summary>
        /// Will pass if any exception is thrown while executing the given action
        /// </summary>
        /// <param name="action"></param>
        public void AssertException(Action action)
        {
            try
            {
                action();

                AssertTrue(false);
            }
            catch
            {
                AssertTrue(true);
            }
        }

        #endregion
    }
}
