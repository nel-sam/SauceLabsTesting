using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using OpenQA.Selenium;

namespace SauceLabsTesting
{
    [SetUpFixture]
    public class Initialization
    {
        [OneTimeSetUp]
        public static void AssemblyInitialize()
        {
            if (Constants.DebugMode)
            {
                WriteDriverBinaries();
                ConfigureFirewall();
            }
        }

        [OneTimeTearDown]
        public static void AssemblyCleanup()
        {
            if (Constants.DebugMode)
            {
                // We don't want to leave executables around
                if (File.Exists(Constants.ChromeDrvFullPath))
                {
                    File.Delete(Constants.ChromeDrvFullPath);
                }

                if (File.Exists(Constants.IEDrvFullPath))
                {
                    File.Delete(Constants.IEDrvFullPath);
                }
            }
        }

        private static void ConfigureFirewall()
        {
            if (Constants.TestOnChrome && !Utilities.FirewallExceptionExists(Constants.ChromeDrvFullPath))
                Utilities.AddFirewallException(Constants.ChromeDrvFullPath);

            if (Constants.TestOnIE && !Utilities.FirewallExceptionExists(Constants.IEDrvFullPath))
                Utilities.AddFirewallException(Constants.IEDrvFullPath);
        }

        private static void WriteDriverBinaries()
        {
            if (!File.Exists(Constants.ChromeDrvFullPath) && Constants.TestOnChrome)
            {
                WriteResourceToFile(Path.GetFileName(Constants.ChromeDrvFullPath), Constants.ChromeDrvFullPath);
            }

            if (!File.Exists(Constants.IEDrvFullPath) && Constants.TestOnIE)
            {
                WriteResourceToFile(Path.GetFileName(Constants.IEDrvFullPath), Constants.IEDrvFullPath);
            }
        }

        public static void WriteResourceToFile(string simpleResourceName, string fileName)
        {
            // Search for resource name rather than assume it's correct.
            // This helps because resource names must include the namespace (including folders), so moving files
            // would break this method if the files were ever moved. 
            string trueResName = Assembly.GetExecutingAssembly().GetManifestResourceNames().FirstOrDefault(x => x.Contains(simpleResourceName));

            using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(trueResName))
            {
                using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    resource.CopyTo(file);
                }
            }
        }
    }

    public class UITestBase
    {
        internal IWebDriver driver;
        internal string browser;
        internal string version;
        internal string os;
        internal string deviceName;
        internal string deviceOrientation;

        public UITestBase(string browser, string version, string os, string deviceName, string deviceOrientation)
        {
            this.browser = browser;
            this.version = version;
            this.os = os;
            this.deviceName = deviceName;
            this.deviceOrientation = deviceOrientation;
        }

        public void SetupBrowser()
        {
            // If we are running in 
            if (Constants.DebugMode)
            {
                if (driver != null)
                    driver.Quit();

                if (Constants.TestOnFireFox)
                {
                    driver = new FirefoxDriver();
                }

                if (Constants.TestOnIE)
                {
                    var ieOptions = new InternetExplorerOptions();
                    ieOptions.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                    driver = new InternetExplorerDriver(Constants.TempFolder, ieOptions);
                }

                if (Constants.TestOnChrome)
                {
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--test-type");
                    driver = new ChromeDriver(Constants.TempFolder, chromeOptions);
                }
            }
            else
            {
                DesiredCapabilities caps = new DesiredCapabilities();
                caps.SetCapability(CapabilityType.BrowserName, browser);
                caps.SetCapability(CapabilityType.Version, version);
                caps.SetCapability(CapabilityType.Platform, os);
                caps.SetCapability("deviceName", deviceName);
                caps.SetCapability("deviceOrientation", deviceOrientation);
                caps.SetCapability("username", Constants.SauceLabsAccountName);
                caps.SetCapability("accessKey", Constants.SauceLabsAccountKey);
                caps.SetCapability("name", TestContext.CurrentContext.Test.Name);

                driver = new RemoteWebDriver(new Uri("http://ondemand.saucelabs.com:80/wd/hub"), caps, TimeSpan.FromSeconds(600));
            }
        }

        // Explicit waits are sometimes needed. This method 
        // helps make it easier to change how we wait later on.
        public void Wait(int ms = 250)
        {
            System.Threading.Thread.Sleep(ms);
        }
    }
}