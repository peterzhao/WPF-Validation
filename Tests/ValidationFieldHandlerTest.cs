using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Moq;
using NUnit.Framework;
using VisualValidation;

namespace Tests
{
    [TestFixture]
    public class ValidationFieldHandlerTest
    {
        private Mock<IUIElement> uiElmentMock;
        private Mock<IValidationContainer> validationContainerMock;
        private StubValidationSource validationSource1;
        private StubValidationSource validationSource2;
        private string fieldName = "field1";
        private string errorMessage1;
        private string errorMessage2;
        private ValidationResult validationResult;

        [SetUp]
        public void SetUp()
        {
            uiElmentMock = new Mock<IUIElement>();
            validationContainerMock = new Mock<IValidationContainer>();
            uiElmentMock.Setup(d => d.SetValue(ValidationContainer.ValidationResultProperty, It.IsAny<ValidationResult>()))
                .Callback((DependencyProperty property,object result) => validationResult = (ValidationResult) result);

            validationSource1 = new StubValidationSource();
            validationSource1.ValidationFuncs.Add(fieldName, () =>  errorMessage1) ;
            validationSource2 = new StubValidationSource();
            validationSource2.ValidationFuncs.Add(fieldName, () => errorMessage2);

            

        }

        [Test]
        public void ShouldAllow_ValidationSource_Null()
        {
            validationContainerMock.Setup(c => c.ValidationSource).Returns(()=>null);
            new ValidationFieldHandler(uiElmentMock.Object, fieldName, validationContainerMock.Object);

            Assert.AreEqual(string.Empty, validationResult.ErrorMessage);

        }

       

        [Test]
        public void ShouldUnRegisterEventFromOldValidationSourceWhenValidationSourceChanged()
        {
            validationSource1.ValidationEnabled = true;
            validationSource2.ValidationEnabled = true;

            IValidationSource validationSource = validationSource1;
            validationContainerMock.Setup(c => c.ValidationSource).Returns(() => validationSource);
            
            new ValidationFieldHandler(uiElmentMock.Object, fieldName, validationContainerMock.Object);

            validationSource = validationSource2;
            validationContainerMock.Raise(c => c.ValidationSourceChanged += null, EventArgs.Empty);

            errorMessage1 = "error";
            validationSource1.NotifyPropertyChanged(fieldName);
            Assert.IsTrue(validationResult.IsValid, "old validation source should not affect validation result");

            errorMessage2 = "error";
            validationSource2.NotifyPropertyChanged(fieldName);
            Assert.AreEqual(errorMessage2, validationResult.ErrorMessage, "new validation source should  affect validation result");

        }
      

        [Test]
        public void ShouldSet_ValidationResult_WhenValidationIsEnabled()
        {
            validationContainerMock.Setup(c => c.ValidationSource).Returns(validationSource1);
            validationSource1.ValidationEnabled = true;
            errorMessage1 = "an error";
            new ValidationFieldHandler(uiElmentMock.Object, fieldName, validationContainerMock.Object);

            Assert.AreEqual(errorMessage1, validationResult.ErrorMessage);
        }

        [Test]
        public void ShouldSet_ValidationResult_WhenValidationIsNotEnabled()
        {
            validationContainerMock.Setup(c => c.ValidationSource).Returns(validationSource1);
            validationSource1.ValidationEnabled = false;
            errorMessage1 = "an error"; //this does not matter
            new ValidationFieldHandler(uiElmentMock.Object, fieldName, validationContainerMock.Object);

            Assert.AreEqual(string.Empty, validationResult.ErrorMessage);
        }

        [Test]
        public void ShouldSetValidationResultWhenValidationIsEnabledChanged()
        {
            validationContainerMock.Setup(c => c.ValidationSource).Returns(validationSource1);
            validationSource1.ValidationEnabled = false;
            errorMessage1 = "an error";
            new ValidationFieldHandler(uiElmentMock.Object, fieldName, validationContainerMock.Object);

            validationSource1.ValidationEnabled = true;
            validationSource1.NotifyValidationEnabledChanged();

            Assert.AreEqual(errorMessage1, validationResult.ErrorMessage);
        }

        [Test]
        public void ShouldSetValidationResultWhenValidationSourcePropertyChanged()
        {
            validationContainerMock.Setup(c => c.ValidationSource).Returns(validationSource1);
            validationSource1.ValidationEnabled = true;
            new ValidationFieldHandler(uiElmentMock.Object, fieldName, validationContainerMock.Object);
            
            errorMessage1 = "an error for field " + fieldName;
            validationSource1.NotifyPropertyChanged(fieldName);


            Assert.AreEqual(errorMessage1, validationResult.ErrorMessage);
        }
    }
}
