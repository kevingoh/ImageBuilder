using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;

namespace DeployUsingARMTemplate
{
    public class Menu
    {
        public static string? exportOption;
        public static List<Option>? options;
        public static void MenuIndex()
        {

            // Create options that you want your menu to have
            options = new List<Option>
            {
                new Option("Windows based App Service", () => WindowsAppServiceHosting()),
                //new Option("Linux based App Service", () =>  LinuxAppServiceHosting()),
                new Option("External (Not on Azure services)", () =>  ExternalHosting()),
                new Option("Exit", () => Environment.Exit(0)),
            };

            // Set the default index of the selected item to be the first
            int index = 0;

            // Write the menu out
            WriteMenu(options, options[index]);

            // Store key info in here
            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey();

                // Handle each key input (down arrow will write the menu again with a different selected item)
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (index + 1 < options.Count)
                    {
                        index++;
                        WriteMenu(options, options[index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteMenu(options, options[index]);
                    }
                }
                // Handle different action for the option
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    options[index].Selected.Invoke();
                    index = 0;
                }
            }
            while (keyinfo.Key != ConsoleKey.X);

            Console.ReadKey();

        }
        
        //Actions

        private static async void ExternalHosting()
        {
            Console.Clear();
            await migratingFromExternalSite.wpressImport();
            // ExternalSiteExportMenu.ExternalSiteExportMenuIndex();
            Console.Clear();
            ExportMenu.ExportMenuIndex();
            Environment.Exit(0);
        }

        private static async void WindowsAppServiceHosting()
        {
            Console.Clear();
            await WindowsBasedService.migratingFromWindowsService();
            Console.Clear();
            ExportMenu.ExportMenuIndex();
            Environment.Exit(0);
        }

        private static async void LinuxAppServiceHosting()
        {

            Console.Clear();
            await migratingFromLinuxAppService.linuxServiceMigration();
            Console.Clear();
            ExportMenu.ExportMenuIndex();
            Environment.Exit(0);

        }

        static void WriteMenu(List<Option> options, Option selectedOption)
        {
            Console.Clear();

            Console.WriteLine("Welcome to WordPress Migrate!");
            Console.WriteLine("Where do you host your WordPress site?");

            foreach (Option option in options)
            {
                if (option == selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("--> ");
                }
                else
                {
                    Console.Write(" ");
                }

                Console.WriteLine(option.Name);
            }
        }

    }

    public class Option
    {
        public string Name { get; }
        public Action Selected { get; }

        public Option(string name, Action selected)
        {
            Name = name;
            Selected = selected;
        }
    }

}