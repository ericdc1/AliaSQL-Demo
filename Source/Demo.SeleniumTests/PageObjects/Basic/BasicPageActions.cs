using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.SeleniumTests.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Demo.SeleniumTests.PageObjects.Basic
{
    public class BasicPageActions
    {

        public static void GoTo()
        {
            Browser.StartWebDriver();
        }

        public static bool IsAt()
        {
            var homepage = Browser.Driver.FindElement(By.TagName("Title"));
            var basicPage = new BasicPageElements();
            PageFactory.InitElements(Browser.Driver, basicPage);
            return basicPage.HomePageTitle.Text == homepage.Text;
        }

        public static void AddNewRecord()
        {
            var basicPage = new BasicPageElements();
            PageFactory.InitElements(Browser.Driver, basicPage);
            basicPage.CreateRecordButton.Click();

            Browser.Wait().Until((d) => d.FindElement(By.Id("FullName")));
            var newName = Browser.Driver.FindElement(By.Id("FullName"));
            newName.Clear();
            newName.SendKeys("Test");

            var newValue1 = Browser.Driver.FindElement(By.Id("value1"));
            newValue1.Clear();
            newValue1.SendKeys("1");

            var newValue2 = Browser.Driver.FindElement(By.Id("value2"));
            newValue2.Clear();
            newValue2.SendKeys("1");

            basicPage.CreatSubmitButton.Click();           

        }

        public static bool WasNewRecordAdded()
        {
            var newRecordCreated = Browser.Driver.FindElement(By.XPath("/html/body/div[2]/table/tbody/tr[3]/td")).Text;
            var basicPage = new BasicPageElements();
            PageFactory.InitElements(Browser.Driver, basicPage);
            return basicPage.NewRecordCell.Text == newRecordCreated;
        }

        public static void DeleteRecord()
        {
            var basicPage = new BasicPageElements();
            PageFactory.InitElements(Browser.Driver, basicPage);
            basicPage.DeleteRecordLink.Click();
        }

        public static bool WasRecordDeleted()
        {           
            var basicPage = new BasicPageElements();
            PageFactory.InitElements(Browser.Driver, basicPage);
            IWebElement element = null;
            if (TryFindElement(By.XPath("/html/body/div[2]/table/tbody/tr[3]/td"), out element))
            {
                return false;
            }
            else
            {
                return true;
            }


        }

        public static void StopTests()
        {
            Browser.StopTests();
        }

        public static bool TryFindElement(By by, out IWebElement element)
        {           
            try
            {
                element = Browser.Driver.FindElement(by);
            }
            catch (NoSuchElementException ex)
            {
                element = null;
                return false;
            }
            return true;
        }
    }

}


