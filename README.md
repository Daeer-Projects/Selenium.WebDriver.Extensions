# Selenium Web Driver Extension

I created this repository to help with Selenium testing.  I found there were some problems with using the built in tools.

* Exceptions
* Wait until

## Exceptions

The current tooling seems to throw exceptions ***a lot*** when the element can't be found.  This is due to the page not loading fully, or not being visible.

So, this extension uses the catch ```Exception``` to catch all exceptions raised by the find method.

## Wait until

There is a way to try and find an element using a class called ```WebDriverWait```.  However, I found that this still threw exceptions.  So, I have written this extension to wrap a ``do`` ```while``` loop around the ```try catch``` block.

## Signatures

```csharp
public static IWebElement SafeGetElement(this IWebDriver driver, By by, int timeOutInSeconds = 0, Func<IWebElement, IWebElement> elementCheckFunc = null)

public static IReadOnlyCollection<IWebElement> SafeGetElements(this IWebDriver driver, By by, int timeOutInSeconds = 0)
```

### IWebDriver

The object that we are using for testing.

### By

The ```By``` parameter we are using for the search.

### int

The time out in seconds that the process will run for while it is attempting to find the element.  If the time runs out, and the element still hasn't been found, then it will return null, and not throw an exception.

### Func<IWebElement, IWebElement>

This function is what you would like to run against the element found.  As long as it returns an ```IWebElement```.

You would use it to limit the element further.  You might not want the element that has been found, as it is not enabled, or displayed.

## Usages

Here are some examples of how to use the element.  They are, mostly, taken from the unit test project.

### Safe Get Element

```csharp
var result = _driver.SafeGetElement(By.Id("banana"));
```

```csharp
var result = _driver.SafeGetElement(By.Id("banana"), 5);
```

```csharp
var result = _driver.SafeGetElement(By.Id("banana"), 0, (element) => element.Displayed ? element : null);
```

```csharp
var result = _driver.SafeGetElement(By.Id("banana"), 5, (element) => element.Displayed ? element : null);
```

```csharp
var result = _driver.SafeGetElement(By.Id("banana"), 5, (element) => (element.Displayed && element.Enabled) ? element : null);
```

### Safe Get Elements

```csharp
var result = _driver.SafeGetElements(By.Id("banana"));
```

```csharp
var result = _driver.SafeGetElements(By.Id("banana"), 5);
```
