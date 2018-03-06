using System;
using System.Configuration;
using System.IO;

namespace SauceLabsTesting
{
    internal static class Constants
    {
        #region Set in app.config
        // SauceLabs credentials
        internal static string SauceLabsAccountName = ConfigurationManager.AppSettings.Get("SauceLabsAccountName");
        internal static string SauceLabsAccountKey = ConfigurationManager.AppSettings.Get("SauceLabsAccountKey");
        
        internal static string Url = ConfigurationManager.AppSettings.Get("Url");
        internal static string Browser = ConfigurationManager.AppSettings.Get("Browser") ?? String.Empty;
        internal static bool TestOnChrome = Browser.Equals("chrome", StringComparison.InvariantCultureIgnoreCase);
        internal static bool TestOnIE = Browser.Equals("IE", StringComparison.InvariantCultureIgnoreCase);
        internal static bool TestOnFireFox = Browser.Equals("FireFox", StringComparison.InvariantCultureIgnoreCase);
#if DEBUG
        internal static bool DebugMode = true;
#else
        internal static bool DebugMode = false;
#endif
        #endregion

        internal static string TempFolder = Path.GetTempPath();
        internal static string ChromeDrvFullPath = Path.Combine(TempFolder, "chromedriver.exe");
        internal static string IEDrvFullPath = Path.Combine(TempFolder, "IEDriverServer.exe");
    }
}
