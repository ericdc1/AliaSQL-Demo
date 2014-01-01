
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Demo.SeleniumTests.PageObjects.Basic
{
    public class BasicPageElements
    {
        [FindsBy(How = How.TagName, Using = "Title")]
#pragma warning disable 649
        private IWebElement _homePageTitle;
#pragma warning restore 649
        public IWebElement HomePageTitle
        {
            get { return _homePageTitle; }
        }

        [FindsBy(How = How.LinkText, Using = "Create New")] 
#pragma warning disable 649
        private IWebElement _createRecordButton;
#pragma warning restore 649
        public IWebElement CreateRecordButton
        {
            get { return _createRecordButton; }
        }

        [FindsBy(How = How.CssSelector, Using = "input.btn.btn-default")] 
#pragma warning disable 649
        private IWebElement _creatSubmitButton;
#pragma warning restore 649
        public IWebElement CreatSubmitButton
        {
            get { return _creatSubmitButton; }
        }

        [FindsBy(How = How.XPath, Using = "/html/body/div[2]/table/tbody/tr[3]/td")]
#pragma warning disable 649
        private IWebElement _newRecordCell;
#pragma warning restore 649
        public IWebElement NewRecordCell
        {
            get { return _newRecordCell; }
        }

        [FindsBy(How = How.XPath, Using = "/html/body/div[2]/table/tbody/tr[3]/td[5]/a[2]")] 
#pragma warning disable 649
        private IWebElement _deleteRecordLink;
#pragma warning restore 649
        public IWebElement DeleteRecordLink
        {
            get { return _deleteRecordLink; }
        }
    }
}
