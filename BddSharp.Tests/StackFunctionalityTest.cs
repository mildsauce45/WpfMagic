using System.Collections.Generic;
using BddSharp.Engine.Attributes;
using BddSharp.Engine.Tests;

namespace BddSharp.Tests
{
    [Scenario("Tests adding a value to a new stack and verifying the value sticks")]
    public class StackFunctionalityTest : Specification
    {
        public Stack<int> Stack
        {
            get { return World.stack as Stack<int>; }
        }

        [Given("A brand spanking new stack of ints")]
        public void CreateStack()
        {
            World.stack = new Stack<int>();
        }

        [When("An integer is pushed")]
        public void AddAnInt()
        {
            Stack.Push(1);
        }

        [Then("There should be an int in the stack")]
        public void CheckCount()
        {
            this.AssertTrue(Stack.Count == 1);
        }

        [Then("The value should be 1")]
        public void CheckValueInStack()
        {
            var val = Stack.Peek();

            this.AssertTrue(val == 1);
        }
    }
}
