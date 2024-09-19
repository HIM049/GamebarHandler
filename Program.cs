using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace GamebarHandler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                Console.WriteLine("Handled the Gamebar stuff!");
                Console.WriteLine("Press any key to exit the program...");
                Console.ReadKey();
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
                            newRegCom.SetValue("", $"\"{ProgramPath}\" called-from-uri");
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Saved registry values for ms-gamebar URI to open {Process.GetCurrentProcess().MainModule.FileName}.");
                    Console.ResetColor();
                    Console.WriteLine("Whenever windows tries to open the Gamebar now, it should open this program instead, which will then silently close.");
                    Console.WriteLine("Press any key to exit the program...");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    switch (ex.GetType().ToString())
                    {
                        case "System.UnauthorizedAccessException":
                            Console.WriteLine("Access denied - Please make sure you are running this program as administrator!");
                            break;
                        default:
                            Console.WriteLine("Something went wrong:" + Environment.NewLine + ex);
                            break;
                    }
                    Console.ResetColor();
                    Console.WriteLine("Press any key to exit the program...");
                    Console.ReadKey();
                }
            }
        }
    }
}