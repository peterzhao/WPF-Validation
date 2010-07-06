using System;
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
           
            validationContainerMock.Setup(c => c.ValidationEnabled).Returns(true);

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
            validationContainerMock.Setup(c => c.ValidationEnabled).Returns(true);
            errorMessage1 = "an error";
            new ValidationFieldHandler(uiElmentMock.Object, fieldName, validationContainerMock.Object);

            Assert.AreEqual(errorMessage1, validationResult.ErrorMessage);
        }

        [Test]
        public void ShouldSet_ValidationResult_WhenValidationIsNotEnabled()
        {
            validationContainerMock.Setup(c => c.ValidationSource).Returns(validationSource1);
            validationContainerMock.Setup(c => c.ValidationEnabled).Returns(false);
            errorMessage1 = "an error"; //this does not matter
            new ValidationFieldHandler(uiElmentMock.Object, fieldName, validationContainerMock.Object);

            Assert.AreEqual(string.Empty, validationResult.ErrorMessage);
        }

        [Test]
        public void ShouldSetValidationResultWhenValidationIsEnabledChanged()
        {
            validationContainerMock.Setup(c => c.ValidationSource).Returns(validationSource1);
            var validationEnabled = false;
            validationContainerMock.Setup(c => c.ValidationEnabled).Returns(()=> validationEnabled);
            errorMessage1 = "an error";
            new ValidationFieldHandler(uiElmentMock.Object, fieldName, validationContainerMock.Object);

            validationEnabled = true;
            validationContainerMock.Raise(c => c.ValidationEnabledChanged += null,EventArgs.Empty);

            Assert.AreEqual(errorMessage1, validationResult.ErrorMessage);
        }

        [Test]
        public void ShouldSetValidationResultWhenValidationSourcePropertyChanged()
        {
            validationContainerMock.Setup(c => c.ValidationSource).Returns(validationSource1);
            validationContainerMock.Setup(c => c.ValidationEnabled).Returns(true);
            new ValidationFieldHandler(uiElmentMock.Object, fieldName, validationContainerMock.Object);
            
            errorMessage1 = "an error for field " + fieldName;
            validationSource1.NotifyPropertyChanged(fieldName);


            Assert.AreEqual(errorMessage1, validationResult.ErrorMessage);
        }
    }
}
