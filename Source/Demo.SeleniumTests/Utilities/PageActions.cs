using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace Demo.SeleniumTests.Utilities
{
    public class PageActions
    {
        static IWebDriver myWebDriver = new FirefoxDriver();

        public PageActions(IWebDriver webDriver)
        {
            myWebDriver = webDriver;
        }

        //public DataTable ExecuteSQL(string connString, string SQL = “”, string SPName = “”, string[] Parameters = new string[] {})
        //{
        ////Some code which will execute a query or stored proceedure for you
        //}

        public void SwitchToWindow(string handle)
        {
            //Switches to another window
            myWebDriver.SwitchTo().Window(handle);
        }

        public void SwitchToFrame(string handle)
        {
            //switches to a different frame
            myWebDriver.SwitchTo().Frame(handle);
        }

        public void Goto(string url)
        {

            myWebDriver.Url = url;
        }

        //public void RefreshPage()
        //{
        //    myWebDriver.Navigate().Refresh();
        //}

        public void Execute(By by, Action<IWebElement> action)
        {
            var element = myWebDriver.FindElement(by);
            action(element);
        }

        public void SetText(string elementName, string newText)
        {
            Execute(By.Name(elementName), e =>
            {
                e.Clear();
                e.SendKeys(newText);
            });
        }

    }
}
