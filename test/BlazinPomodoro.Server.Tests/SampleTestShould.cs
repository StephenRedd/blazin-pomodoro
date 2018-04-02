using System;
using Xunit;

namespace BlazinPomodoro.Server.Tests
{
    public class SampleTestShould
    {
        [Fact]
        [Trait("TestType", "Unit")]
        public void ReturnTrueBecauseReasons()
        {
            Assert.True(true, "reasons");
        }
    }
}
