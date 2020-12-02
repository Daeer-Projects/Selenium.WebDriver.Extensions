using System;
using System.Collections.Generic;

using OpenQA.Selenium;

namespace WebDriver.Extensions
{
    /// <summary>
    /// A small collection of extensions for the Selenium Web Driver.
    /// </summary>
    public static class WebDriverExtensions
    {
        /// <summary>
        /// An extension to safely get an element using the IWebDriver, by catching all of the exceptions that can be raised.  If an element can't be found,
        /// and the time out expires, then it will return null.
        /// </summary>
        /// <param name="driver"> The web driver set up in the calling application. </param>
        /// <param name="by"> The by search parameter set up in the calling application. </param>
        /// <param name="timeOutInSeconds"> An optional parameter to set a time out for the check. </param>
        /// <param name="elementCheckFunc"> An optional function to be used on the found element for additional checks. </param>
        /// <returns>
        /// The found element, or null.
        /// </returns>
        [Obsolete("use 'SafeFindElement' instead", false)]
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

        /// <summary>
        /// An extension to safely get elements using the IWebDriver, by catching all of the exceptions that can be raised.  If the elements can't be found, and
        /// the time out expires, this it will return null.
        /// </summary>
        /// <param name="driver"> The web driver set up in the calling application. </param>
        /// <param name="by"> The by search parameter set up in the calling application. </param>
        /// <param name="timeOutInSeconds"> An optional parameter to set a time out for the check. </param>
        /// <returns>
        /// A read only collection of IWebElements or null.
        /// </returns>
        [Obsolete("use 'SafeFindElements' instead", false)]
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

        /// <summary>
        /// An extension to safely find an element using the IWebDriver, by catching all of the exceptions that can be raised.  If an element can't be found,
        /// and the time out expires, then it will return null.
        /// </summary>
        /// <param name="driver"> The web driver set up in the calling application. </param>
        /// <param name="by"> The by search parameter set up in the calling application. </param>
        /// <param name="timeOutInSeconds"> An optional parameter to set a time out for the check. </param>
        /// <param name="elementCheckFunc"> An optional function to be used on the found element for additional checks. </param>
        /// <returns>
        /// The found element, or null.
        /// </returns>
        public static IWebElement SafeFindElement(this IWebDriver driver, By by, int timeOutInSeconds = 0, Func<IWebElement, IWebElement> elementCheckFunc = null)
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

        /// <summary>
        /// An extension to safely find elements using the IWebDriver, by catching all of the exceptions that can be raised.  If the elements can't be found, and
        /// the time out expires, this it will return null.
        /// </summary>
        /// <param name="driver"> The web driver set up in the calling application. </param>
        /// <param name="by"> The by search parameter set up in the calling application. </param>
        /// <param name="timeOutInSeconds"> An optional parameter to set a time out for the check. </param>
        /// <returns>
        /// A read only collection of IWebElements or null.
        /// </returns>
        public static IReadOnlyCollection<IWebElement> SafeFindElements(this IWebDriver driver, By by, int timeOutInSeconds = 0)
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