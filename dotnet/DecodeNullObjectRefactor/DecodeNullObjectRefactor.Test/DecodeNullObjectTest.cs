using System;
using System.Windows.Forms.Integration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DecodeNullObjectRefactor.Test
{
    [TestClass]
    public class DecodeNullObjectTest
    {
        private DecodeNullObject _decodeNullObject;

        [TestInitialize]
        public void InitializeTests()
        {
            _decodeNullObject = new DecodeNullObject();
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void TestOldImplementation_IntValueInputParameter()
        {
            var innerValue = 5;

            try
            {
                _decodeNullObject.DecodeNullObjectOldImplementation<ElementHost>(innerValue);
            }
            catch (InvalidCastException ex)
            {
                Assert.AreEqual(@"Unable to cast object of type 'System.Int32' to type 'System.Windows.Forms.Integration.ElementHost'.", ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void TestOldImplementation_NullInputParameter()
        {
            ElementHost innerValue = null;
            var result = _decodeNullObject.DecodeNullObjectOldImplementation<ElementHost>(innerValue);
            
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestOldImplementation_ElementHostInputParameter()
        {
            var innerValue = new ElementHost();
            var result = _decodeNullObject.DecodeNullObjectOldImplementation<ElementHost>(innerValue);
            
            Assert.AreEqual(innerValue, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void TestOldImplementation_NullObjectInputParameter()
        {
            var innerValue = new NullObject();

            try
            {
                _decodeNullObject.DecodeNullObjectOldImplementation<ElementHost>(innerValue);
            }
            catch (InvalidCastException ex)
            {
                Assert.AreEqual(@"Unable to cast object of type 'DecodeNullObjectRefactor.NullObject' to type 'System.Windows.Forms.Integration.ElementHost'.", ex.Message);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void TestOldImplementation_TypeOfElementHostInputParameter()
        {
            var innerValue = typeof(ElementHost);

            try
            {
                _decodeNullObject.DecodeNullObjectOldImplementation<ElementHost>(innerValue);
            }
            catch (InvalidCastException ex)
            {
                Assert.AreEqual(@"Unable to cast object of type 'System.RuntimeType' to type 'System.Windows.Forms.Integration.ElementHost'.", ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void TestOldImplementation_TypeOfNullObjectInputParameter()
        {
            var innerValue = typeof(NullObject);
            var result = _decodeNullObject.DecodeNullObjectOldImplementation<ElementHost>(innerValue);

            Assert.IsNull(result);
        }



        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void TestNewImplementation_IntValueInputParameter()
        {
            var innerValue = 5;

            try
            {
                _decodeNullObject.DecodeNullObjectNewImplementation<ElementHost>(innerValue);
            }
            catch (InvalidCastException ex)
            {
                Assert.AreEqual(@"Unable to cast object of type 'System.Int32' to type 'System.Windows.Forms.Integration.ElementHost'.", ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void TestNewImplementation_NullInputParameter()
        {
            ElementHost innerValue = null;
            var result = _decodeNullObject.DecodeNullObjectNewImplementation<ElementHost>(innerValue);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestNewImplementation_ElementHostInputParameter()
        {
            var innerValue = new ElementHost();
            var result = _decodeNullObject.DecodeNullObjectNewImplementation<ElementHost>(innerValue);

            Assert.AreEqual(innerValue, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void TestNewImplementation_NullObjectInputParameter()
        {
            var innerValue = new NullObject();

            try
            {
                _decodeNullObject.DecodeNullObjectNewImplementation<ElementHost>(innerValue);
            }
            catch (InvalidCastException ex)
            {
                Assert.AreEqual(@"Unable to cast object of type 'DecodeNullObjectRefactor.NullObject' to type 'System.Windows.Forms.Integration.ElementHost'.", ex.Message);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void TestNewImplementation_TypeOfElementHostInputParameter()
        {
            var innerValue = typeof(ElementHost);

            try
            {
                _decodeNullObject.DecodeNullObjectNewImplementation<ElementHost>(innerValue);
            }
            catch (InvalidCastException ex)
            {
                Assert.AreEqual(@"Unable to cast object of type 'System.RuntimeType' to type 'System.Windows.Forms.Integration.ElementHost'.", ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void TestNewImplementation_TypeOfNullObjectInputParameter()
        {
            var innerValue = typeof(NullObject);
            var result = _decodeNullObject.DecodeNullObjectNewImplementation<ElementHost>(innerValue);

            Assert.IsNull(result);
        }
    }
}
