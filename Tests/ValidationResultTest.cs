using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using VisualValidation;

namespace Tests
{
    [TestFixture]
    public class ValidationResultTest
    {
        [Test]
        public void  IsValidWhenErrorMessageIsEmpty()
        {
            ValidationResult result = new ValidationResult();
            Assert.IsTrue(result.IsValid);

            result = new ValidationResult(string.Empty);
            Assert.IsTrue(result.IsValid);

            result = new ValidationResult(null);
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void IsNotValidWhenErrorMessageIsNotEmpty()
        {
            ValidationResult result = new ValidationResult("something");
            Assert.IsFalse(result.IsValid);
        }
        
    }
}
