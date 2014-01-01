
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Demo.SeleniumTests.PageObjects.Basic
{
    public class BasicPageElements
    {
        [FindsBy(How = How.TagName, Using = "Title")]
        private IWebElement homePageTitle;
        public IWebElement HomePageTitle
        {
            get { return homePageTitle; }
        }

        [FindsBy(How = How.LinkText, Using = "Create New")] 
        private IWebElement createRecordButton;
        public IWebElement CreateRecordButton
        {
            get { return createRecordButton; }
        }

        [FindsBy(How = How.CssSelector, Using = "input.btn.btn-default")] private IWebElement creatSubmitButton;
        public IWebElement CreatSubmitButton
        {
            get { return creatSubmitButton; }
        }

        [FindsBy(How = How.XPath, Using = "/html/body/div[2]/table/tbody/tr[3]/td")]
        private IWebElement newRecordCell;
        public IWebElement NewRecordCell
        {
            get { return newRecordCell; }
        }

        [FindsBy(How = How.XPath, Using = "/html/body/div[2]/table/tbody/tr[3]/td[5]/a[2]")] private IWebElement deleteRecordLink;
        public IWebElement DeleteRecordLink
        {
            get { return deleteRecordLink; }
        }
    }
}
