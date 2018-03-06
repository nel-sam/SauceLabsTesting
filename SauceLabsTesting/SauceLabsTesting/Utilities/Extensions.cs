using OpenQA.Selenium;
using System;
using System.Linq;

namespace SauceLabsTesting
{
    internal static class Extensions
    {
        internal static string GetTitleWait(this IWebDriver browser, string expectedTitleText, int maxWaitSecs = 10)
        {
            // Break the wait into 250 ms increments 
            maxWaitSecs *= 4;

            while (maxWaitSecs-- > 0 && browser.Title != expectedTitleText)
            {
                System.Threading.Thread.Sleep(250);
            }

            if (maxWaitSecs <= 0 && browser.Title != expectedTitleText)
                throw new Exception("Title never become the expected value of " + expectedTitleText);

            return browser.Title;
        }

        internal static string GetTitleWaitForContains(this IWebDriver browser, string expectedContainingText, int maxWaitSecs = 10)
        {
            // Break the wait into 250 ms increments 
            maxWaitSecs *= 4;

            while (maxWaitSecs-- > 0 && !browser.Title.Contains(expectedContainingText))
            {
                System.Threading.Thread.Sleep(250);
            }

            if (maxWaitSecs <= 0 && !browser.Title.Contains(expectedContainingText))
                throw new Exception("Title never contained the expected value of " + expectedContainingText);

            return browser.Title;
        }

        internal static bool ElementByTagNameContains(this IWebDriver browser, string tagName, string expectedText, int maxWaitSecs = 10)
        {
            // Break the wait into 250 ms increments 
            maxWaitSecs *= 4;

            while (maxWaitSecs-- > 0 && browser.FindElement(By.TagName(tagName)) != null && !browser.FindElement(By.TagName(tagName)).Text.Contains(expectedText))
            {
                System.Threading.Thread.Sleep(250);
            }

            if (maxWaitSecs <= 0 && browser.FindElement(By.TagName(tagName)) != null && !browser.FindElement(By.TagName(tagName)).Text.Contains(expectedText))
                return false;

            return true;
        }
    }
}
