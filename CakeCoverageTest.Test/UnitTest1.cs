using System;
using Xunit;

namespace CakeCoverageTest.Test
{
    public class UnitTest1
    {
        [Fact]
        public void GivenAName_WhenGreeted_ThenSaluationContainsName()
        {
            var greeter = new Greeter();
            var greeting = greeter.Greet("Bob");
            Assert.Equal("Hello Bob", greeting);
        }
    }
}
