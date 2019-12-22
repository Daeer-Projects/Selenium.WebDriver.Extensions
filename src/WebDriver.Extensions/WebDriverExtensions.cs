using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace WebDriver.Extensions
{
    public static class WebDriverExtensions
    {
        public static IWebElement SafeGetElement(this IWebDriver driver, By by, int timeOutInSeconds = 0, Func<IWebElement, IWebElement> elementCheckFunc = null)
        {
            if (by == null) return null;

            IWebElement foundElement = null;

            if (timeOutInSeconds > 0)
            {
                var timeToStop = DateTime.Now.AddSeconds(timeOutInSeconds);
                bool stop;
                do
                {
                    if (DateTime.Now < timeToStop)
                    {
                        var (webElement, found) = ProcessToFindElement(driver, by, elementCheckFunc);
                        foundElement = webElement;
                        stop = found;
                    }
                    else
                    {
                        stop = true;
                    }
                } while (!stop);
            }
            else
            {
                var (webElement, _) = ProcessToFindElement(driver, by, elementCheckFunc);
                foundElement = webElement;
            }

            return foundElement;
        }

        public static IReadOnlyCollection<IWebElement> SafeGetElements(this IWebDriver driver, By by, int timeOutInSeconds = 0)
        {
            if (by == null) return null;

            IReadOnlyCollection<IWebElement> foundElements = null;

            if (timeOutInSeconds > 0)
            {
                var timeToStop = DateTime.Now.AddSeconds(timeOutInSeconds);
                bool stop;
                do
                {
                    if (DateTime.Now < timeToStop)
                    {
                        var (elements, found) = ProcessFindElements(driver, by);
                        foundElements = elements;
                        stop = found;
                    }
                    else
                    {
                        stop = true;
                    }
                } while (!stop);
            }
            else
            {
                var (elements, _) = ProcessFindElements(driver, by);
                foundElements = elements;
            }

            return foundElements;
        }

        private static (IWebElement, bool) ProcessToFindElement(ISearchContext driver, By by, Func<IWebElement, IWebElement> elementCheckFunc = null)
        {
            IWebElement foundElement;
            var stop = false;
            try
            {
                var safeFind = driver.FindElement(by);
                if (safeFind != null && elementCheckFunc != null)
                {
                    foundElement = elementCheckFunc(safeFind);
                }
                else
                {
                    foundElement = safeFind;
                }
                stop = true;
            }
            catch (Exception)
            {
                foundElement = null;
            }

            return (foundElement, stop);
        }

        private static (IReadOnlyCollection<IWebElement>, bool) ProcessFindElements(IWebDriver driver, By by)
        {
            IReadOnlyCollection<IWebElement> foundElements;
            var stop = false;
            try
            {
                foundElements = driver.FindElements(by);
                stop = true;
            }
            catch (Exception)
            {
                foundElements = null;
            }

            return (foundElements, stop);
        }
    }
}
