namespace ShopOnline.Testing.UI.WebDriver;
public class UnitTest1: IClassFixture<WebDriverFixture>
{    
    private readonly WebDriverFixture _webDriverFixture;
    private readonly IWebDriver _driver;

    public UnitTest1(WebDriverFixture webDriverFixture)
    {   
        _webDriverFixture = webDriverFixture;
        _driver = webDriverFixture.Driver;
    }        

    [Fact]
    [Trait("Category", "Smoke")]
    public void LoadHomePage()
    {   
        var homePage = new HomePage(_driver);
        homePage.NavigateTo();
    }

    [Fact]
    [Trait("Category", "Smoke")]
    public void LoadHomePage2()
    {
        var homePage = new HomePage(_driver);
        homePage.NavigateTo();
    }

    [Fact]
    [Trait("Category", "Smoke")]
    public void LoadHomePage3()
    {
        var homePage = new HomePage(_driver);
        homePage.NavigateTo();
    }
}