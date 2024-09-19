using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace GamebarHandler
{
    internal class Program
    {

        public static bool IsDebug = false;

        private static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (IsDebug)
                {
                    switch (args.First())
                    {
                        case "ms-gamebar":
                            ShowMessageBox("Handled a ms-gamebar URI call!");
                            break;
                        case "ms-gamingoverlay":
                            ShowMessageBox("Handled a ms-gamingoverlay URI call!");
                            break;
                        default:
                            ShowMessageBox("Handled an unknown URI call? The URI passed was \"" + args.First() + "\".");
                            break;
                    }
                }
            }
            else
            {
                try
                {
                    using (RegistryKey newRegKey = Registry.ClassesRoot.CreateSubKey("ms-gamebar", RegistryKeyPermissionCheck.ReadWriteSubTree))
                    {
                        newRegKey.SetValue("", "URL:GamebarHandler");
                        newRegKey.SetValue("URL Protocol", "");

                        using (RegistryKey newRegCom = newRegKey.CreateSubKey(@"shell\open\command"))
                        {
                            string ProgramPath = Process.GetCurrentProcess().MainModule.FileName;
                            newRegCom.SetValue("", $"\"{ProgramPath}\" ms-gamebar");
                        }
                    }
                    using (RegistryKey newRegKey = Registry.ClassesRoot.CreateSubKey("ms-gamingoverlay", RegistryKeyPermissionCheck.ReadWriteSubTree))
                    {
                        newRegKey.SetValue("", "URL:GamebarHandler");
                        newRegKey.SetValue("URL Protocol", "");

                        using (RegistryKey newRegCom = newRegKey.CreateSubKey(@"shell\open\command"))
                        {
                            string ProgramPath = Process.GetCurrentProcess().MainModule.FileName;
                            newRegCom.SetValue("", $"\"{ProgramPath}\" ms-gamingoverlay");
                        }
                    }
                    ShowMessageBox($"Successfully saved registry values so ms-gamebar and ms-gamingoverlay URIs (commands) will open \"{Process.GetCurrentProcess().MainModule.FileName}\" instead." + Environment.NewLine + "Whenever Windows tries to open the Gamebar\\Gaming Overlay now, it should open this program instead, which will then silently close.");
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    switch (ex.GetType().ToString())
                    {
                        case "System.UnauthorizedAccessException":
                            ShowMessageBox("Access denied - Please make sure you are running this program as administrator!", MessageBoxIcon.Error);
                            break;
                        default:
                            ShowMessageBox("Something went wrong:" + Environment.NewLine + ex, MessageBoxIcon.Error);
                            break;
                    }
                }
            }
        }

        public static void ShowMessageBox(string message, MessageBoxIcon MessageBoxIconType = MessageBoxIcon.Information)
        {
            MessageBox.Show(message, "Gamebar Handler",MessageBoxButtons.OK, MessageBoxIconType);
        }
    }
}