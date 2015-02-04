using System.Collections.Generic;
using BddSharp.Engine.Attributes;
using BddSharp.Engine.Tests;

namespace BddSharp.Tests
{
    public class QueueFunctionalityTest : Specification
    {
        public Queue<int> Queue
        {
            get { return World.queue as Queue<int>; }
        }

        [Given("A new queue")]
        public void CreateQueue()
        {
            World.queue = new Queue<int>();
        }

        [When("An integer is queued")]
        public void AddAnInt()
        {
            Queue.Enqueue(1);
        }

        [Then("You should be able to remove the item.")]
        public void CheckDequeue()
        {
            var val = Queue.Dequeue();

            this.AssertTrue(val == 1);
            this.AssertTrue(Queue.Count == 0);
        }

        [Then("Dequeueing again should throw an exception", 1)]
        public void DequeueAgain()
        {
            this.AssertException(() => Queue.Dequeue());
        }
    }
}
