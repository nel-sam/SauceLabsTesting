using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SauceLabsTesting
{
    internal static class Utilities
    {
        /// <returns>The process ID if the process was started successfully.</returns>
        internal static string RunCmdCommand(string command)
        {
            string output = String.Empty;
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + command;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            
            while (!process.StandardOutput.EndOfStream)
            {
                 output += process.StandardOutput.ReadLine();
            }

            return output;
        }

        internal static void AddFirewallException(string appPath)
        {
            string appName = Path.GetFileName(appPath);

            if (!IsAdmin())
            {
                throw new Exception("Tests must be run as an administrator because a Firewall exception needs to be added." +
                    " You can also manually add the exception before running tests. Add an exception for name=" + appName +
                    " and path=" + appPath);
            }

            string cmd = String.Format("netsh firewall add allowedprogram \"{0}\" {1} ENABLE", appPath, appName);
            RunCmdCommand(cmd);
        }

        internal static bool FirewallExceptionExists(string appName)
        {
            if (Path.IsPathRooted(appName))
            {
                appName = Path.GetFileName(appName);
            }

            string output = RunCmdCommand(String.Format("netsh advfirewall firewall show rule name={0}", appName));
            return !output.Contains("No rules");
        }

        internal static bool IsAdmin(WindowsIdentity user = null)
        {
            if (user == null)
            {
                user = WindowsIdentity.GetCurrent();
            }

            var principal = new WindowsPrincipal(user);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        internal static void KillProcess(string procName)
        {
            RunCmdCommand("taskkill /f /im " + procName);
        }
    }
}
