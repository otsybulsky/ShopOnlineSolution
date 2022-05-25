namespace ShopOnline.Testing.UI.WebDriver;

public class UnitTest1
{
    [Fact]
    [Trait("Category", "Smoke")]
    public void LoadApplicationPage()
    {
        using (IWebDriver driver = new EdgeDriver())
        {
            driver.Navigate().GoToUrl("https://google.com");
        }
    }
}