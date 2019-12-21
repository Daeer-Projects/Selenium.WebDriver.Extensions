using System;

using OpenQA.Selenium;

namespace WebDriverExtensions
{
    public static class WebDriverExtensions
    {
        public static IWebElement SafeGetElement(this IWebDriver driver, By by, int timeOutInSeconds, Func<IWebElement, IWebElement> elementCheckFunc)
        {
            if (by == null) return null;
            if (elementCheckFunc == null) return null;

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

        private static (IWebElement, bool) ProcessToFindElement(ISearchContext driver, By by, Func<IWebElement, IWebElement> elementCheckFunc)
        {
            IWebElement foundElement;
            var stop = false;
            try
            {
                var safeFind = driver.FindElement(by);
                foundElement = elementCheckFunc(safeFind);
                stop = true;
            }
            catch (Exception)
            {
                foundElement = null;
            }

            return (foundElement, stop);
        }
    }
}
