/*
Humans must learn to apply their intelligence correctly and evolve beyond their current state.
People must change. Otherwise, even if humanity expands into space, it will only create new
conflicts, and that would be a very sad thing. - Aeolia Schenberg, 2091 A.D.
　　　　 ,r‐､　　　　 　, -､
　 　 　 !　 ヽ　　 　 /　　}
　　　　 ヽ､ ,! -─‐- ､{　　ﾉ
　　　 　 ／｡　｡　　　 r`'､´
　　　　/ ,.-─- ､　　 ヽ､.ヽ　　　Haro
　　 　 !/　　　　ヽ､.＿, ﾆ|　　　　　Haro!
 　　　 {　　　 　  　 　 ,'
　　 　 ヽ　 　     　 ／,ｿ
　　　　　ヽ､.＿＿__r',／
 */

using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace GamebarHandler
{
    internal class Program
    {
        
        private const bool IsDebug = false;

        private static void Main(string[] args)
        {
            // Check whether the program running by user
            if (args.Length > 0)
            {
                // Running by windows (with arguments)
                
                // Debug output
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
                    return;
                }
                
                if (OpenSteamBigPicture())
                {
                    Process.Start("steam://open/bigpicture");
                }
            }
            else
            {
                // Running by user (without arguments)
                ShowActionDialog();

            }
        }

        private static void ShowActionDialog()
        {
            var dialog = new TaskDialog
            {
                Caption = "Confirmation",
                InstructionText = "Which action would you like to take?",
                Text = "Please choose onr of the following options:",
                Icon = TaskDialogStandardIcon.Information,
                StandardButtons = TaskDialogStandardButtons.None
            };

            var btnInstall = new TaskDialogCommandLink("install", "Install", "Write registry values to handle game bar");
            var btnUninstall = new TaskDialogCommandLink("uninstall", "Uninstall", "Remove registry values to stop handling game bar");
            var btnCancel = new TaskDialogCommandLink("cancel", "Cancel", "Exit without any changes");

            btnInstall.Click += (s, e) => { SetupGameBarRegistry(); dialog.Close(); };
            btnUninstall.Click += (s, e) => { RemoveGameBarRegistry(); dialog.Close(); };
            btnCancel.Click += (s, e) => { dialog.Close(); Environment.Exit(0); };

            // 添加三个自定义按钮
            dialog.Controls.Add(btnInstall);
            dialog.Controls.Add(btnUninstall);
            dialog.Controls.Add(btnCancel);
                
            dialog.Show();
        }

        private static void SetupGameBarRegistry()
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
        
        private static void RemoveGameBarRegistry()
        {
            try
            {
                Registry.ClassesRoot.DeleteSubKeyTree("ms-gamebar", false);
                Registry.ClassesRoot.DeleteSubKeyTree("ms-gamingoverlay", false);
                ShowMessageBox("Successfully removed registry for ms-gamebar and ms-gamingoverlay.", MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                switch (ex.GetType().ToString())
                {
                    case "System.UnauthorizedAccessException":
                        ShowMessageBox("Access denied - Please make sure you are running this program as administrator!", MessageBoxIcon.Error);
                        break;
                    case "System.ArgumentException":
                        ShowMessageBox("Registry not found or already be removed.", MessageBoxIcon.Warning);
                        break;
                    default:
                        ShowMessageBox("Something went wrong:" + Environment.NewLine + ex, MessageBoxIcon.Error);
                        break;
                }
            }
        }
        
        // Check whether the environment variable GAMEBAR_OPEN_STEAM_BIG_PICTURE is set
        private static bool OpenSteamBigPicture()
        {
            return !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GAMEBAR_OPEN_STEAM_BIG_PICTURE"));
        }
        
        private static void ShowMessageBox(string message, MessageBoxIcon MessageBoxIconType = MessageBoxIcon.Information)
        {
            MessageBox.Show(message, "Gamebar Handler",MessageBoxButtons.OK, MessageBoxIconType);
        }
    }
}