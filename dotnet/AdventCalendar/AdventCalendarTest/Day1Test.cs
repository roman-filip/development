using AdventCalendar;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCalendarTest
{
    [TestClass]
    public class Day1Test
    {
        [TestMethod]
        public void Part1Test()
        {
            var day1 = new Day1();
            var answer = day1.Part1();
            Assert.AreEqual(232, answer);
        }

        [TestMethod]
        public void Part2Test()
        {
            var day1 = new Day1();
            var answer = day1.Part2();
            Assert.AreEqual(1783, answer);
        }
    }
}
