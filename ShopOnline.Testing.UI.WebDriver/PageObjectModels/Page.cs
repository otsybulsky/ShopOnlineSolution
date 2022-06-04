namespace ShopOnline.Testing.UI.WebDriver.PageObjectModels;

public class Page
{
    protected IWebDriver Driver;
    protected virtual string PageUrl { get; private set; }
    protected virtual string PageTitle { get; private set; }
       
    protected void OpenPage() 
    {
        Driver.Navigate().GoToUrl(PageUrl);
        EnsurePageLoaded();
    }

    public void EnsurePageLoaded(bool onlyCheckUrlStartsWithExpectedText = true)
    {
        bool urlIsCorrect;
        if (onlyCheckUrlStartsWithExpectedText)
        {
            urlIsCorrect = Driver.Url.StartsWith(PageUrl);
        }
        else
        {
            urlIsCorrect = Driver.Url == PageUrl;
        }        

        bool pageHasLoaded = urlIsCorrect && (Driver.Title == PageTitle);
        if (!pageHasLoaded)
        {
            throw new Exception($"Failed to load page. Page URL = '{Driver.Url}' Page Source: \r\n {Driver.PageSource}");
        }        
    }    
}
