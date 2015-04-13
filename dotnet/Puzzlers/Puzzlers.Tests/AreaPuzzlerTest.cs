using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Puzzlers.Tests
{
    [TestClass]
    public class AreaPuzzlerTest
    {
        private AreaPuzzler _puzzler;

        [TestInitialize]
        public void InitialezeAreaPuzzler()
        {
            _puzzler = new AreaPuzzler();
        }

        [TestMethod]
        public void CalculateAreaForNullBars()
        {
            var area = _puzzler.CalculateArea(null);
            Assert.AreEqual(0, area);
        }

        [TestMethod]
        public void CalculateAreaForTwoBars()
        {
            var area = _puzzler.CalculateArea(new[] { 10, 5 });
            Assert.AreEqual(0, area);
        }

        [TestMethod]
        public void CalculateAreaForEqualBars()
        {
            var area = _puzzler.CalculateArea(new[] { 5, 5, 5, 5 });
            Assert.AreEqual(0, area);
        }

        [TestMethod]
        public void CalculateAreaForRisingBars()
        {
            var area = _puzzler.CalculateArea(new[] { 1, 5, 14, 20 });
            Assert.AreEqual(0, area);
        }

        [TestMethod]
        public void CalculateAreaForFallingBars()
        {
            var area = _puzzler.CalculateArea(new[] { 30, 10, 8, 1 });
            Assert.AreEqual(0, area);
        }

        [TestMethod]
        public void CalculateAreaForRisingAndFallingsBars()
        {
            var area = _puzzler.CalculateArea(new[] { 1, 5, 10, 8, 2 });
            Assert.AreEqual(0, area);
        }

        [TestMethod]
        public void CalculateAreaForThreeBars3()
        {
            var area = _puzzler.CalculateArea(new[] { 10, 4, 4, 4 });
            Assert.AreEqual(0, area);
        }

        [TestMethod]
        public void CalculateAreaForThreeBars()
        {
            var area = _puzzler.CalculateArea(new[] { 11, 5, 11 });
            Assert.AreEqual(6, area);
        }

        [TestMethod]
        public void CalculateAreaForThreeBars2()
        {
            var area = _puzzler.CalculateArea(new[] { 10, 6, 8 });
            Assert.AreEqual(2, area);
        }

        [TestMethod]
        public void CalculateAreaForFiveBars()
        {
            var area = _puzzler.CalculateArea(new[] { 1, 2, 10, 3, 10 });
            Assert.AreEqual(7, area);
        }

        [TestMethod]
        public void CalculateAreaForSevenBars()
        {
            var area = _puzzler.CalculateArea(new[] { 1, 2, 10, 5, 7, 9, 10 });
            Assert.AreEqual(9, area);
        }

        [TestMethod]
        public void CalculateAreaForSevenBars2()
        {
            var area = _puzzler.CalculateArea(new[] { 1, 2, 10, 5, 7, 7, 10 });
            Assert.AreEqual(11, area);
        }

        [TestMethod]
        public void CalculateAreaForEightBars()
        {
            var area = _puzzler.CalculateArea(new[] { 1, 2, 10, 5, 7, 7, 10, 10 });
            Assert.AreEqual(11, area);
        }

        [TestMethod]
        public void CalculateAreaForEightBars2()
        {
            var area = _puzzler.CalculateArea(new[] { 1, 2, 10, 5, 7, 7, 10, 10, 10 });
            Assert.AreEqual(11, area);
        }

        [TestMethod]
        public void ExistCalculateAreaMethodsWithParameter2()
        {
            var area = _puzzler.CalculateArea(new[] { 10, 20, 80, 70, 60, 90 });
            Assert.AreEqual(30, area);
        }

        [TestMethod]
        public void ExistCalculateAreaMethodsWithParameter()
        {
            var area = _puzzler.CalculateArea(new[] { 10, 20, 80, 70, 60, 90, 40, 30, 40, 70 });
            Assert.AreEqual(130, area);
        }
    }
}
