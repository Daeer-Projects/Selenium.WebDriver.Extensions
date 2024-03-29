﻿#pragma warning disable 618

namespace WebDriver.Extension.Tests;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Extensions;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using OpenQA.Selenium;
using Xunit;

/// <summary>
///     The tests for the extension methods.
/// </summary>
public class WebDriverExtensionTests
{
    /// <summary>
    ///     Initialises a new instance of the WebDriverExtensionTests that sets up the driver and element for testing.
    /// </summary>
    public WebDriverExtensionTests()
    {
        _driver = Substitute.For<IWebDriver>();
        _element = Substitute.For<IWebElement>();
    }

    private readonly IWebDriver _driver;
    private readonly IWebElement _element;

    #region Safe Get Element

    [Fact]
    public void Test_safe_get_with_null_parameters_returns_null()
    {
        // Arrange.
        // Act.
        IWebElement result = _driver.SafeGetElement(null);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_get_with_null_function_returns_null()
    {
        // Arrange.
        // Act.
        IWebElement result = _driver.SafeGetElement(null, 4);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_get_with_null_by_returns_null()
    {
        // Arrange.
        // Act.
        IWebElement result = _driver.SafeGetElement(null, 1, element => element);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_get_with_null_timeout_and_function_and_element_not_found_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .ReturnsNull();

        // Act.
        IWebElement result = _driver.SafeGetElement(By.Id("banana"));

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_get_with_zero_timeout_and_element_not_found_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .ReturnsNull();

        // Act.
        IWebElement result = _driver.SafeGetElement(By.Id("banana"), 0, element => element.Displayed ? element : null);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_get_with_two_timeout_and_element_not_found_with_function_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .ReturnsNull();

        // Act.
        IWebElement result = _driver.SafeGetElement(By.Id("banana"), 2, element => element.Displayed ? element : null);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_get_with_null_function_and_element_found_returns_element()
    {
        // Arrange.
        _element.Displayed.Returns(true);
        _driver.FindElement(Arg.Any<By>())
            .Returns(_element);

        // Act.
        IWebElement result = _driver.SafeGetElement(By.Id("banana"), 5);

        // Assert.
        result.Should()
            .Be(_element);
    }

    [Fact]
    public void Test_safe_get_with_five_timeout_and_element_found_returns_element()
    {
        // Arrange.
        _element.Displayed.Returns(true);
        _driver.FindElement(Arg.Any<By>())
            .Returns(_element);

        // Act.
        IWebElement result = _driver.SafeGetElement(By.Id("banana"), 5, element => element.Displayed ? element : null);

        // Assert.
        result.Should()
            .Be(_element);
    }

    [Fact]
    public void Test_safe_get_with_five_timeout_and_null_function_element_found_returns_element_before_time_out_has_passed()
    {
        // Arrange.
        _element.Displayed.Returns(true);
        _driver.FindElement(Arg.Any<By>())
            .Returns(_element);
        Stopwatch stopWatch = new();

        // Act.
        stopWatch.Start();
        IWebElement result = _driver.SafeGetElement(By.Id("banana"), 5);
        stopWatch.Stop();

        // Assert.
        result.Should()
            .Be(_element);
        stopWatch.Elapsed.Seconds.Should()
            .BeLessThan(5);
    }

    [Fact]
    public void Test_safe_get_with_five_timeout_and_element_found_returns_element_before_time_out_has_passed()
    {
        // Arrange.
        _element.Displayed.Returns(true);
        _driver.FindElement(Arg.Any<By>())
            .Returns(_element);
        Stopwatch stopWatch = new();

        // Act.
        stopWatch.Start();
        IWebElement result = _driver.SafeGetElement(By.Id("banana"), 5, element => element.Displayed ? element : null);
        stopWatch.Stop();

        // Assert.
        result.Should()
            .Be(_element);
        stopWatch.Elapsed.Seconds.Should()
            .BeLessThan(5);
    }

    [Fact]
    public void Test_safe_get_with_zero_timeout_and_null_function_and_no_such_element_exception_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .Throws(new NoSuchElementException());

        // Act.
        IWebElement result = _driver.SafeGetElement(By.Id("banana"));

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_get_with_zero_timeout_and_null_function_and_no_such_frame_exceptions_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .Throws(new NoSuchFrameException());

        // Act.
        IWebElement result = _driver.SafeGetElement(By.Id("banana"));

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_get_with_zero_timeout_and_null_function_and_no_such_window_exceptions_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .Throws(new NoSuchWindowException());

        // Act.
        IWebElement result = _driver.SafeGetElement(By.Id("banana"));

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_get_with_zero_timeout_and_null_function_and_exceptions_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .Throws(new Exception());

        // Act.
        IWebElement result = _driver.SafeGetElement(By.Id("banana"));

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_get_with_one_timeout_and_null_function_and_no_such_element_exception_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .Throws(new NoSuchElementException());

        // Act.
        IWebElement result = _driver.SafeGetElement(By.Id("banana"), 1);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_get_with_one_timeout_and_null_function_and_no_such_frame_exceptions_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .Throws(new NoSuchFrameException());

        // Act.
        IWebElement result = _driver.SafeGetElement(By.Id("banana"), 1);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_get_with_one_timeout_and_null_function_and_no_such_window_exceptions_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .Throws(new NoSuchWindowException());

        // Act.
        IWebElement result = _driver.SafeGetElement(By.Id("banana"), 1);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_get_with_one_timeout_and_function_and_no_such_element_exception_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .Throws(new NoSuchElementException());

        // Act.
        IWebElement result = _driver.SafeGetElement(By.Id("banana"), 1, element => element);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_get_with_one_timeout_and_function_and_no_such_frame_exceptions_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .Throws(new NoSuchFrameException());

        // Act.
        IWebElement result = _driver.SafeGetElement(By.Id("banana"), 1, element => element);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_get_with_one_timeout_and_function_and_no_such_window_exceptions_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .Throws(new NoSuchWindowException());

        // Act.
        IWebElement result = _driver.SafeGetElement(By.Id("banana"), 1, element => element);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_get_with_three_second_timeout_and_element_not_found_returns_null_after_time_out()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .Throws(new NoSuchElementException());
        Stopwatch stopWatch = new();

        // Act.
        stopWatch.Start();
        IWebElement result = _driver.SafeGetElement(By.Id("banana"), 3, element => element.Displayed ? element : null);
        stopWatch.Stop();

        // Assert.
        result.Should()
            .BeNull();
        stopWatch.Elapsed.TotalMilliseconds.Should()
            .BeInRange(2500.0, 3500.0);
    }

    [Fact]
    public void Test_safe_get_with_zero_timeout_and_exception_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .Throws(new NoSuchElementException());

        // Act.
        IWebElement result = _driver.SafeGetElement(By.Id("banana"), 0, element => element.Displayed ? element : null);

        // Assert.
        result.Should()
            .BeNull();
    }

    #endregion Safe Get Element

    #region Safe Get Elements

    [Fact]
    public void Test_safe_get_elements_with_null_parameters_returns_null()
    {
        // Arrange.
        // Act.
        IReadOnlyCollection<IWebElement> result = _driver.SafeGetElements(null);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_get_elements_with_timeout_returns_null()
    {
        // Arrange.
        // Act.
        IReadOnlyCollection<IWebElement> result = _driver.SafeGetElements(null, 4);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_get_elements_with_null_timeout_and_elements_not_found_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .ReturnsNull();

        // Act.
        IReadOnlyCollection<IWebElement> result = _driver.SafeGetElements(By.Id("banana"));

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_get_elements_with_zero_timeout_and_element_not_found_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .ReturnsNull();

        // Act.
        IReadOnlyCollection<IWebElement> result = _driver.SafeGetElements(By.Id("banana"));

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_get_elements_with_time_out_and_element_found_returns_element()
    {
        // Arrange.
        List<IWebElement> expected = new()
        {
            _element
        };

        _driver.FindElements(Arg.Any<By>())
            .Returns(expected.AsReadOnly());

        // Act.
        IReadOnlyCollection<IWebElement> result = _driver.SafeGetElements(By.Id("banana"), 5);

        // Assert.
        result.Should()
            .BeEquivalentTo(expected);
    }

    [Fact]
    public void
        Test_safe_get_elements_with_five_timeout_and_null_function_element_found_returns_element_before_time_out_has_passed()
    {
        // Arrange.
        List<IWebElement> expected = new()
        {
            _element
        };

        _driver.FindElements(Arg.Any<By>())
            .Returns(expected.AsReadOnly());
        Stopwatch stopWatch = new();

        // Act.
        stopWatch.Start();
        IReadOnlyCollection<IWebElement> result = _driver.SafeGetElements(By.Id("banana"), 5);
        stopWatch.Stop();

        // Assert.
        result.Should()
            .BeEquivalentTo(expected);
        stopWatch.Elapsed.Seconds.Should()
            .BeLessThan(5);
    }

    [Fact]
    public void Test_safe_get_elements_with_zero_timeout_and_null_function_and_no_such_element_exception_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElements(Arg.Any<By>())
            .Throws(new NoSuchElementException());

        // Act.
        IReadOnlyCollection<IWebElement> result = _driver.SafeGetElements(By.Id("banana"));

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_get_elements_with_zero_timeout_and_null_function_and_no_such_frame_exceptions_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElements(Arg.Any<By>())
            .Throws(new NoSuchFrameException());

        // Act.
        IReadOnlyCollection<IWebElement> result = _driver.SafeGetElements(By.Id("banana"));

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_get_elements_with_zero_timeout_and_null_function_and_no_such_window_exceptions_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElements(Arg.Any<By>())
            .Throws(new NoSuchWindowException());

        // Act.
        IReadOnlyCollection<IWebElement> result = _driver.SafeGetElements(By.Id("banana"));

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_get_elements_with_zero_timeout_and_null_function_and_exceptions_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElements(Arg.Any<By>())
            .Throws(new Exception());

        // Act.
        IReadOnlyCollection<IWebElement> result = _driver.SafeGetElements(By.Id("banana"));

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_get_elements_with_one_timeout_and_null_function_and_no_such_element_exception_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElements(Arg.Any<By>())
            .Throws(new NoSuchElementException());

        // Act.
        IReadOnlyCollection<IWebElement> result = _driver.SafeGetElements(By.Id("banana"), 1);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_get_elements_with_one_timeout_and_null_function_and_no_such_frame_exceptions_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElements(Arg.Any<By>())
            .Throws(new NoSuchFrameException());

        // Act.
        IReadOnlyCollection<IWebElement> result = _driver.SafeGetElements(By.Id("banana"), 1);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_get_elements_with_one_timeout_and_null_function_and_no_such_window_exceptions_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElements(Arg.Any<By>())
            .Throws(new NoSuchWindowException());

        // Act.
        IReadOnlyCollection<IWebElement> result = _driver.SafeGetElements(By.Id("banana"), 1);

        // Assert.
        result.Should()
            .BeNull();
    }

    #endregion Safe Get Elements

    #region Safe Find Element

    [Fact]
    public void Test_safe_find_with_null_parameters_returns_null()
    {
        // Arrange.
        // Act.
        IWebElement result = _driver.SafeFindElement(null);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_find_with_null_function_returns_null()
    {
        // Arrange.
        // Act.
        IWebElement result = _driver.SafeFindElement(null, 4);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_find_with_null_by_returns_null()
    {
        // Arrange.
        // Act.
        IWebElement result = _driver.SafeFindElement(null, 1, element => element);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_find_with_null_timeout_and_function_and_element_not_found_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .ReturnsNull();

        // Act.
        IWebElement result = _driver.SafeFindElement(By.Id("banana"));

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_find_with_zero_timeout_and_element_not_found_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .ReturnsNull();

        // Act.
        IWebElement result = _driver.SafeFindElement(By.Id("banana"), 0, element => element.Displayed ? element : null);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_find_with_two_timeout_and_element_not_found_with_function_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .ReturnsNull();

        // Act.
        IWebElement result = _driver.SafeFindElement(By.Id("banana"), 2, element => element.Displayed ? element : null);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_find_with_null_function_and_element_found_returns_element()
    {
        // Arrange.
        _element.Displayed.Returns(true);
        _driver.FindElement(Arg.Any<By>())
            .Returns(_element);

        // Act.
        IWebElement result = _driver.SafeFindElement(By.Id("banana"), 5);

        // Assert.
        result.Should()
            .Be(_element);
    }

    [Fact]
    public void Test_safe_find_with_five_timeout_and_element_found_returns_element()
    {
        // Arrange.
        _element.Displayed.Returns(true);
        _driver.FindElement(Arg.Any<By>())
            .Returns(_element);

        // Act.
        IWebElement result = _driver.SafeFindElement(By.Id("banana"), 5, element => element.Displayed ? element : null);

        // Assert.
        result.Should()
            .Be(_element);
    }

    [Fact]
    public void Test_safe_find_with_five_timeout_and_null_function_element_found_returns_element_before_time_out_has_passed()
    {
        // Arrange.
        _element.Displayed.Returns(true);
        _driver.FindElement(Arg.Any<By>())
            .Returns(_element);
        Stopwatch stopWatch = new();

        // Act.
        stopWatch.Start();
        IWebElement result = _driver.SafeFindElement(By.Id("banana"), 5);
        stopWatch.Stop();

        // Assert.
        result.Should()
            .Be(_element);
        stopWatch.Elapsed.Seconds.Should()
            .BeLessThan(5);
    }

    [Fact]
    public void Test_safe_find_with_five_timeout_and_element_found_returns_element_before_time_out_has_passed()
    {
        // Arrange.
        _element.Displayed.Returns(true);
        _driver.FindElement(Arg.Any<By>())
            .Returns(_element);
        Stopwatch stopWatch = new();

        // Act.
        stopWatch.Start();
        IWebElement result = _driver.SafeFindElement(By.Id("banana"), 5, element => element.Displayed ? element : null);
        stopWatch.Stop();

        // Assert.
        result.Should()
            .Be(_element);
        stopWatch.Elapsed.Seconds.Should()
            .BeLessThan(5);
    }

    [Fact]
    public void Test_safe_find_with_zero_timeout_and_null_function_and_no_such_element_exception_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .Throws(new NoSuchElementException());

        // Act.
        IWebElement result = _driver.SafeFindElement(By.Id("banana"));

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_find_with_zero_timeout_and_null_function_and_no_such_frame_exceptions_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .Throws(new NoSuchFrameException());

        // Act.
        IWebElement result = _driver.SafeFindElement(By.Id("banana"));

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_find_with_zero_timeout_and_null_function_and_no_such_window_exceptions_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .Throws(new NoSuchWindowException());

        // Act.
        IWebElement result = _driver.SafeFindElement(By.Id("banana"));

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_find_with_zero_timeout_and_null_function_and_exceptions_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .Throws(new Exception());

        // Act.
        IWebElement result = _driver.SafeFindElement(By.Id("banana"));

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_find_with_one_timeout_and_null_function_and_no_such_element_exception_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .Throws(new NoSuchElementException());

        // Act.
        IWebElement result = _driver.SafeFindElement(By.Id("banana"), 1);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_find_with_one_timeout_and_null_function_and_no_such_frame_exceptions_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .Throws(new NoSuchFrameException());

        // Act.
        IWebElement result = _driver.SafeFindElement(By.Id("banana"), 1);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_find_with_one_timeout_and_null_function_and_no_such_window_exceptions_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .Throws(new NoSuchWindowException());

        // Act.
        IWebElement result = _driver.SafeFindElement(By.Id("banana"), 1);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_find_with_one_timeout_and_function_and_no_such_element_exception_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .Throws(new NoSuchElementException());

        // Act.
        IWebElement result = _driver.SafeFindElement(By.Id("banana"), 1, element => element);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_find_with_one_timeout_and_function_and_no_such_frame_exceptions_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .Throws(new NoSuchFrameException());

        // Act.
        IWebElement result = _driver.SafeFindElement(By.Id("banana"), 1, element => element);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_find_with_one_timeout_and_function_and_no_such_window_exceptions_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .Throws(new NoSuchWindowException());

        // Act.
        IWebElement result = _driver.SafeFindElement(By.Id("banana"), 1, element => element);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_find_with_three_second_timeout_and_element_not_found_returns_null_after_time_out()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .Throws(new NoSuchElementException());
        Stopwatch stopWatch = new();

        // Act.
        stopWatch.Start();
        IWebElement result = _driver.SafeFindElement(By.Id("banana"), 3, element => element.Displayed ? element : null);
        stopWatch.Stop();

        // Assert.
        result.Should()
            .BeNull();
        stopWatch.Elapsed.TotalMilliseconds.Should()
            .BeInRange(2500.0, 3500.0);
    }

    [Fact]
    public void Test_safe_find_with_zero_timeout_and_exception_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElement(Arg.Any<By>())
            .Throws(new NoSuchElementException());

        // Act.
        IWebElement result = _driver.SafeFindElement(By.Id("banana"), 0, element => element.Displayed ? element : null);

        // Assert.
        result.Should()
            .BeNull();
    }

    #endregion Safe Find Element

    #region Safe Find Elements

    [Fact]
    public void Test_safe_find_elements_with_null_parameters_returns_null()
    {
        // Arrange.
        // Act.
        IReadOnlyCollection<IWebElement> result = _driver.SafeFindElements(null);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_find_elements_with_timeout_returns_null()
    {
        // Arrange.
        // Act.
        IReadOnlyCollection<IWebElement> result = _driver.SafeFindElements(null, 4);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_find_elements_with_null_timeout_and_elements_not_found_returns_null()
    {
        // Arrange.
        _driver.FindElements(Arg.Any<By>())
            .ReturnsNull();

        // Act.
        IReadOnlyCollection<IWebElement> result = _driver.SafeFindElements(By.Id("banana"));

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_find_elements_with_zero_timeout_and_element_not_found_returns_null()
    {
        // Arrange.
        _driver.FindElements(Arg.Any<By>())
            .ReturnsNull();

        // Act.
        IReadOnlyCollection<IWebElement> result = _driver.SafeFindElements(By.Id("banana"));

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_find_elements_with_time_out_and_element_found_returns_element()
    {
        // Arrange.
        List<IWebElement> expected = new()
        {
            _element
        };

        _driver.FindElements(Arg.Any<By>())
            .Returns(expected.AsReadOnly());

        // Act.
        IReadOnlyCollection<IWebElement> result = _driver.SafeFindElements(By.Id("banana"), 5);

        // Assert.
        result.Should()
            .BeEquivalentTo(expected);
    }

    [Fact]
    public void
        Test_safe_find_elements_with_five_timeout_and_null_function_element_found_returns_element_before_time_out_has_passed()
    {
        // Arrange.
        List<IWebElement> expected = new()
        {
            _element
        };

        _driver.FindElements(Arg.Any<By>())
            .Returns(expected.AsReadOnly());
        Stopwatch stopWatch = new();

        // Act.
        stopWatch.Start();
        IReadOnlyCollection<IWebElement> result = _driver.SafeFindElements(By.Id("banana"), 5);
        stopWatch.Stop();

        // Assert.
        result.Should()
            .BeEquivalentTo(expected);
        stopWatch.Elapsed.Seconds.Should()
            .BeLessThan(5);
    }

    [Fact]
    public void Test_safe_find_elements_with_zero_timeout_and_null_function_and_no_such_element_exception_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElements(Arg.Any<By>())
            .Throws(new NoSuchElementException());

        // Act.
        IReadOnlyCollection<IWebElement> result = _driver.SafeFindElements(By.Id("banana"));

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_find_elements_with_zero_timeout_and_null_function_and_no_such_frame_exceptions_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElements(Arg.Any<By>())
            .Throws(new NoSuchFrameException());

        // Act.
        IReadOnlyCollection<IWebElement> result = _driver.SafeFindElements(By.Id("banana"));

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_find_elements_with_zero_timeout_and_null_function_and_no_such_window_exceptions_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElements(Arg.Any<By>())
            .Throws(new NoSuchWindowException());

        // Act.
        IReadOnlyCollection<IWebElement> result = _driver.SafeFindElements(By.Id("banana"));

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_find_elements_with_zero_timeout_and_null_function_and_exceptions_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElements(Arg.Any<By>())
            .Throws(new Exception());

        // Act.
        IReadOnlyCollection<IWebElement> result = _driver.SafeFindElements(By.Id("banana"));

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_find_elements_with_one_timeout_and_null_function_and_no_such_element_exception_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElements(Arg.Any<By>())
            .Throws(new NoSuchElementException());

        // Act.
        IReadOnlyCollection<IWebElement> result = _driver.SafeFindElements(By.Id("banana"), 1);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_find_elements_with_one_timeout_and_null_function_and_no_such_frame_exceptions_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElements(Arg.Any<By>())
            .Throws(new NoSuchFrameException());

        // Act.
        IReadOnlyCollection<IWebElement> result = _driver.SafeFindElements(By.Id("banana"), 1);

        // Assert.
        result.Should()
            .BeNull();
    }

    [Fact]
    public void Test_safe_find_elements_with_one_timeout_and_null_function_and_no_such_window_exceptions_thrown_returns_null()
    {
        // Arrange.
        _driver.FindElements(Arg.Any<By>())
            .Throws(new NoSuchWindowException());

        // Act.
        IReadOnlyCollection<IWebElement> result = _driver.SafeFindElements(By.Id("banana"), 1);

        // Assert.
        result.Should()
            .BeNull();
    }

    #endregion Safe Find Elements
}