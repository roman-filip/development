using AdventCalendar;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCalendarTest
{
    [TestClass]
    public class Day4Test
    {
        [TestMethod]
        public void Part1Test1()
        {
            var day4 = new Day4();
            var result = day4.Part1("abcdef");

            Assert.AreEqual(609043, result);
        }

        [TestMethod]
        public void Part1Test2()
        {
            var day4 = new Day4();
            var result = day4.Part1("pqrstuv");

            Assert.AreEqual(1048970, result);
        }

        [TestMethod]
        public void Part1Test3()
        {
            var day4 = new Day4();
            var result = day4.Part1(_input);

            Assert.AreEqual(117946, result);
        }

        [TestMethod]
        public void Part2Test1()
        {
            var day4 = new Day4();
            var result = day4.Part2(_input);

            Assert.AreEqual(3938038, result);
        }

        private string _input = "ckczppom";
    }
}
