using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace ShopOnline.Testing.UI.WebDriver;
public class WebDriverFixture : IDisposable
{
    public EdgeDriver Driver { get; private set; }    

    public WebDriverFixture()
    {
        var options = new EdgeOptions();
        options.AddArguments("--headless");

        _ = new DriverManager().SetUpDriver(new EdgeConfig());

        Driver = new EdgeDriver(options);
    }

    public void Dispose()
    {
        Driver.Quit();
        Driver.Dispose(); 
    }
}
