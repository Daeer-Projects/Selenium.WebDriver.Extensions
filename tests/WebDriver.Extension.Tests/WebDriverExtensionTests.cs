using System.Diagnostics;

using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using OpenQA.Selenium;
using WebDriver.Extensions;
using Xunit;

namespace WebDriver.Extension.Tests
{
    public class WebDriverExtensionTests
    {
        private readonly IWebDriver _driver;
        private readonly IWebElement _element;

        public WebDriverExtensionTests()
        {
            _driver = Substitute.For<IWebDriver>();
            _element = Substitute.For<IWebElement>();
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
        public void Test_safe_get_with_null_function_returns_null()
        {
            // Arrange.
            // Act.
            var result = _driver.SafeGetElement(By.Id("banana"), 1, null);

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
        public void Test_safe_get_with_one_second_timeout_and_element_not_found_returns_null_after_time_out()
        {
            // Arrange.
            _driver.FindElement(Arg.Any<By>()).ReturnsNull();
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
    }
}
