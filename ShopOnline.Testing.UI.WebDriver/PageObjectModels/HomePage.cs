namespace ShopOnline.Testing.UI.WebDriver.PageObjectModels;

public class HomePage: Page
{
    protected override string PageUrl => "https://localhost:7294";
    protected override string PageTitle => "ShopOnline.Web";

    public HomePage(IWebDriver driver)
    {
        Driver = driver;
        Driver.Manage().Window.Maximize();
    }

    public void NavigateTo()
    {
        OpenPage();
        WaitLoading();
    }

    private void WaitLoading()
    {
        WebDriverWait wait = new WebDriverWait(Driver, timeout: TimeSpan.FromSeconds(10));
        var element = wait.Until(ExpectedConditions.ElementExists(By.LinkText("Home")));
    }
}
