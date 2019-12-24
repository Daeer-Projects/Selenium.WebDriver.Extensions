using System;
using System.Collections.Generic;
using System.Diagnostics;

using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using OpenQA.Selenium;
using WebDriver.Extensions;
using Xunit;

namespace WebDriver.Extension.Tests
{
    /// <summary>
    /// The tests for the extension methods.
    /// </summary>
    public class WebDriverExtensionTests
    {
        private readonly IWebDriver _driver;
        private readonly IWebElement _element;

        /// <summary>
        /// Initialises a new instance of the WebDriverExtensionTests that sets up the driver and element for testing.
        /// </summary>
        public WebDriverExtensionTests()
        {
            _driver = Substitute.For<IWebDriver>();
            _element = Substitute.For<IWebElement>();
        }

        #region Safe Get Element

        [Fact]
        public void Test_safe_get_with_null_parameters_returns_null()
        {
            // Arrange.
            // Act.
            var result = _driver.SafeGetElement(null);

            // Assert.
            result.Should().BeNull();
        }

        [Fact]
        public void Test_safe_get_with_null_function_returns_null()
        {
            // Arrange.
            // Act.
            var result = _driver.SafeGetElement(null, 4);

            // Assert.
            result.Should().BeNull();
        }

        [Fact]
        public void Test_safe_get_with_null_by_returns_null()
        {
            // Arrange.
            // Act.
            var result = _driver.SafeGetElement(null, 1, (element) => element);

            // Assert.
            result.Should().BeNull();
        }

        [Fact]
        public void Test_safe_get_with_null_timeout_and_function_and_element_not_found_returns_null()
        {
            // Arrange.
            _driver.FindElement(Arg.Any<By>()).ReturnsNull();

            // Act.
            var result = _driver.SafeGetElement(By.Id("banana"));

            // Assert.
            result.Should().BeNull();
        }

        [Fact]
        public void Test_safe_get_with_zero_timeout_and_element_not_found_returns_null()
        {
            // Arrange.
            _driver.FindElement(Arg.Any<By>()).ReturnsNull();

            // Act.
            var result = _driver.SafeGetElement(By.Id("banana"), 0, (element) => element.Displayed ? element : null);

            // Assert.
            result.Should().BeNull();
        }

        [Fact]
        public void Test_safe_get_with_two_timeout_and_element_not_found_with_function_returns_null()
        {
            // Arrange.
            _driver.FindElement(Arg.Any<By>()).ReturnsNull();

            // Act.
            var result = _driver.SafeGetElement(By.Id("banana"), 2, (element) => element.Displayed ? element : null);

            // Assert.
            result.Should().BeNull();
        }

        [Fact]
        public void Test_safe_get_with_null_function_and_element_found_returns_element()
        {
            // Arrange.
            _element.Displayed.Returns(true);
            _driver.FindElement(Arg.Any<By>()).Returns(_element);

            // Act.
            var result = _driver.SafeGetElement(By.Id("banana"), 5);

            // Assert.
            result.Should().Be(_element);
        }

        [Fact]
        public void Test_safe_get_with_five_timeout_and_element_found_returns_element()
        {
            // Arrange.
            _element.Displayed.Returns(true);
            _driver.FindElement(Arg.Any<By>()).Returns(_element);

            // Act.
            var result = _driver.SafeGetElement(By.Id("banana"), 5, (element) => element.Displayed ? element : null);

            // Assert.
            result.Should().Be(_element);
        }

        [Fact]
        public void Test_safe_get_with_five_timeout_and_null_function_element_found_returns_element_before_time_out_has_passed()
        {
            // Arrange.
            _element.Displayed.Returns(true);
            _driver.FindElement(Arg.Any<By>()).Returns(_element);
            var stopWatch = new Stopwatch();

            // Act.
            stopWatch.Start();
            var result = _driver.SafeGetElement(By.Id("banana"), 5);
            stopWatch.Stop();

            // Assert.
            result.Should().Be(_element);
            stopWatch.Elapsed.Seconds.Should().BeLessThan(5);
        }

        [Fact]
        public void Test_safe_get_with_five_timeout_and_element_found_returns_element_before_time_out_has_passed()
        {
            // Arrange.
            _element.Displayed.Returns(true);
            _driver.FindElement(Arg.Any<By>()).Returns(_element);
            var stopWatch = new Stopwatch();

            // Act.
            stopWatch.Start();
            var result = _driver.SafeGetElement(By.Id("banana"), 5, (element) => element.Displayed ? element : null);
            stopWatch.Stop();

            // Assert.
            result.Should().Be(_element);
            stopWatch.Elapsed.Seconds.Should().BeLessThan(5);
        }

        [Fact]
        public void Test_safe_get_with_zero_timeout_and_null_function_and_no_such_element_exception_thrown_returns_null()
        {
            // Arrange.
            _driver.FindElement(Arg.Any<By>()).Throws(new NoSuchElementException());

            // Act.
            var result = _driver.SafeGetElement(By.Id("banana"));

            // Assert.
            result.Should().BeNull();
        }

        [Fact]
        public void Test_safe_get_with_zero_timeout_and_null_function_and_no_such_frame_exceptions_thrown_returns_null()
        {
            // Arrange.
            _driver.FindElement(Arg.Any<By>()).Throws(new NoSuchFrameException());

            // Act.
            var result = _driver.SafeGetElement(By.Id("banana"));

            // Assert.
            result.Should().BeNull();
        }

        [Fact]
        public void Test_safe_get_with_zero_timeout_and_null_function_and_no_such_window_exceptions_thrown_returns_null()
        {
            // Arrange.
            _driver.FindElement(Arg.Any<By>()).Throws(new NoSuchWindowException());

            // Act.
            var result = _driver.SafeGetElement(By.Id("banana"));

            // Assert.
            result.Should().BeNull();
        }

        [Fact]
        public void Test_safe_get_with_zero_timeout_and_null_function_and_exceptions_thrown_returns_null()
        {
            // Arrange.
            _driver.FindElement(Arg.Any<By>()).Throws(new Exception());

            // Act.
            var result = _driver.SafeGetElement(By.Id("banana"));

            // Assert.
            result.Should().BeNull();
        }

        [Fact]
        public void Test_safe_get_with_one_timeout_and_null_function_and_no_such_element_exception_thrown_returns_null()
        {
            // Arrange.
            _driver.FindElement(Arg.Any<By>()).Throws(new NoSuchElementException());

            // Act.
            var result = _driver.SafeGetElement(By.Id("banana"), 1);

            // Assert.
            result.Should().BeNull();
        }

        [Fact]
        public void Test_safe_get_with_one_timeout_and_null_function_and_no_such_frame_exceptions_thrown_returns_null()
        {
            // Arrange.
            _driver.FindElement(Arg.Any<By>()).Throws(new NoSuchFrameException());

            // Act.
            var result = _driver.SafeGetElement(By.Id("banana"), 1);

            // Assert.
            result.Should().BeNull();
        }

        [Fact]
        public void Test_safe_get_with_one_timeout_and_null_function_and_no_such_window_exceptions_thrown_returns_null()
        {
            // Arrange.
            _driver.FindElement(Arg.Any<By>()).Throws(new NoSuchWindowException());

            // Act.
            var result = _driver.SafeGetElement(By.Id("banana"), 1);

            // Assert.
            result.Should().BeNull();
        }

        [Fact]
        public void Test_safe_get_with_one_timeout_and_function_and_no_such_element_exception_thrown_returns_null()
        {
            // Arrange.
            _driver.FindElement(Arg.Any<By>()).Throws(new NoSuchElementException());

            // Act.
            var result = _driver.SafeGetElement(By.Id("banana"), 1, (element) => element);

            // Assert.
            result.Should().BeNull();
        }

        [Fact]
        public void Test_safe_get_with_one_timeout_and_function_and_no_such_frame_exceptions_thrown_returns_null()
        {
            // Arrange.
            _driver.FindElement(Arg.Any<By>()).Throws(new NoSuchFrameException());

            // Act.
            var result = _driver.SafeGetElement(By.Id("banana"), 1, (element) => element);

            // Assert.
            result.Should().BeNull();
        }

        [Fact]
        public void Test_safe_get_with_one_timeout_and_function_and_no_such_window_exceptions_thrown_returns_null()
        {
            // Arrange.
            _driver.FindElement(Arg.Any<By>()).Throws(new NoSuchWindowException());

            // Act.
            var result = _driver.SafeGetElement(By.Id("banana"), 1, (element) => element);

            // Assert.
            result.Should().BeNull();
        }

        [Fact]
        public void Test_safe_get_with_one_second_timeout_and_element_not_found_returns_null_after_time_out()
        {
            // Arrange.
            _driver.FindElement(Arg.Any<By>()).Throws(new NoSuchElementException());
            var stopWatch = new Stopwatch();

            // Act.
            stopWatch.Start();
            var result = _driver.SafeGetElement(By.Id("banana"), 2, (element) => element.Displayed ? element : null);
            stopWatch.Stop();

            // Assert.
            result.Should().BeNull();
            stopWatch.Elapsed.Seconds.Should().Be(2);
        }

        [Fact]
        public void Test_safe_get_with_zero_timeout_and_exception_thrown_returns_null()
        {
            // Arrange.
            _driver.FindElement(Arg.Any<By>()).Throws(new NoSuchElementException());

            // Act.
            var result = _driver.SafeGetElement(By.Id("banana"), 0, (element) => element.Displayed ? element : null);

            // Assert.
            result.Should().BeNull();
        }

        #endregion Safe Get Element

        #region Safe Get Elements

        [Fact]
        public void Test_safe_get_elements_with_null_parameters_returns_null()
        {
            // Arrange.
            // Act.
            var result = _driver.SafeGetElements(null);

            // Assert.
            result.Should().BeNull();
        }

        [Fact]
        public void Test_safe_get_elements_with_timeout_returns_null()
        {
            // Arrange.
            // Act.
            var result = _driver.SafeGetElements(null, 4);

            // Assert.
            result.Should().BeNull();
        }

        [Fact]
        public void Test_safe_get_elements_with_null_timeout_and_elements_not_found_returns_null()
        {
            // Arrange.
            _driver.FindElement(Arg.Any<By>()).ReturnsNull();

            // Act.
            var result = _driver.SafeGetElements(By.Id("banana"));

            // Assert.
            result.Should().BeNull();
        }

        [Fact]
        public void Test_safe_get_elements_with_zero_timeout_and_element_not_found_returns_null()
        {
            // Arrange.
            _driver.FindElement(Arg.Any<By>()).ReturnsNull();

            // Act.
            var result = _driver.SafeGetElements(By.Id("banana"), 0);

            // Assert.
            result.Should().BeNull();
        }

        [Fact]
        public void Test_safe_get_elements_with_time_out_and_element_found_returns_element()
        {
            // Arrange.
            var expected = new List<IWebElement>
            {
                _element
            };

            _driver.FindElements(Arg.Any<By>()).Returns(expected.AsReadOnly());

            // Act.
            var result = _driver.SafeGetElements(By.Id("banana"), 5);

            // Assert.
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Test_safe_get_elements_with_five_timeout_and_null_function_element_found_returns_element_before_time_out_has_passed()
        {
            // Arrange.
            var expected = new List<IWebElement>
            {
                _element
            };

            _driver.FindElements(Arg.Any<By>()).Returns(expected.AsReadOnly());
            var stopWatch = new Stopwatch();

            // Act.
            stopWatch.Start();
            var result = _driver.SafeGetElements(By.Id("banana"), 5);
            stopWatch.Stop();

            // Assert.
            result.Should().BeEquivalentTo(expected);
            stopWatch.Elapsed.Seconds.Should().BeLessThan(5);
        }

        [Fact]
        public void Test_safe_get_elements_with_zero_timeout_and_null_function_and_no_such_element_exception_thrown_returns_null()
        {
            // Arrange.
            _driver.FindElements(Arg.Any<By>()).Throws(new NoSuchElementException());

            // Act.
            var result = _driver.SafeGetElements(By.Id("banana"));

            // Assert.
            result.Should().BeNull();
        }

        [Fact]
        public void Test_safe_get_elements_with_zero_timeout_and_null_function_and_no_such_frame_exceptions_thrown_returns_null()
        {
            // Arrange.
            _driver.FindElements(Arg.Any<By>()).Throws(new NoSuchFrameException());

            // Act.
            var result = _driver.SafeGetElements(By.Id("banana"));

            // Assert.
            result.Should().BeNull();
        }

        [Fact]
        public void Test_safe_get_elements_with_zero_timeout_and_null_function_and_no_such_window_exceptions_thrown_returns_null()
        {
            // Arrange.
            _driver.FindElements(Arg.Any<By>()).Throws(new NoSuchWindowException());

            // Act.
            var result = _driver.SafeGetElements(By.Id("banana"));

            // Assert.
            result.Should().BeNull();
        }

        [Fact]
        public void Test_safe_get_elements_with_zero_timeout_and_null_function_and_exceptions_thrown_returns_null()
        {
            // Arrange.
            _driver.FindElements(Arg.Any<By>()).Throws(new Exception());

            // Act.
            var result = _driver.SafeGetElements(By.Id("banana"));

            // Assert.
            result.Should().BeNull();
        }

        [Fact]
        public void Test_safe_get_elements_with_one_timeout_and_null_function_and_no_such_element_exception_thrown_returns_null()
        {
            // Arrange.
            _driver.FindElements(Arg.Any<By>()).Throws(new NoSuchElementException());

            // Act.
            var result = _driver.SafeGetElements(By.Id("banana"), 1);

            // Assert.
            result.Should().BeNull();
        }

        [Fact]
        public void Test_safe_get_elements_with_one_timeout_and_null_function_and_no_such_frame_exceptions_thrown_returns_null()
        {
            // Arrange.
            _driver.FindElements(Arg.Any<By>()).Throws(new NoSuchFrameException());

            // Act.
            var result = _driver.SafeGetElements(By.Id("banana"), 1);

            // Assert.
            result.Should().BeNull();
        }

        [Fact]
        public void Test_safe_get_elements_with_one_timeout_and_null_function_and_no_such_window_exceptions_thrown_returns_null()
        {
            // Arrange.
            _driver.FindElements(Arg.Any<By>()).Throws(new NoSuchWindowException());

            // Act.
            var result = _driver.SafeGetElements(By.Id("banana"), 1);

            // Assert.
            result.Should().BeNull();
        }

        #endregion Safe Get Elements
    }
}