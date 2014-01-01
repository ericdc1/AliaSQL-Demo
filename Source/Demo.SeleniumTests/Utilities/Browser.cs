using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Demo.SeleniumTests.Utilities
{
    public class Browser
    {
        static IWebDriver webDriver;

        public static IWebDriver StartWebDriver()
        {
            webDriver = Host.Instance.WebDriver;
            webDriver.Navigate().GoToUrl(webDriver.Url);
            return webDriver;
        }


        public static string Title
        {
            get { return webDriver.Title; }
        }

        public static ISearchContext Driver
        {
            get { return webDriver; }
        }

        public static void Goto(string url)
        {
            webDriver.Url = url;

        }

        public static WebDriverWait Wait()
        {
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
            return wait;
        }

        public static void StopTests()
        {
            webDriver.Close();
            webDriver.Quit();
            Host.Instance.WebServer.Stop();
        }
    }
}

