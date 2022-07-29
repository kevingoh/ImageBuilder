using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;

namespace DeployUsingARMTemplate
{
    public class ExportMenu
    {
        public static List<ExportOption>? options;
        public static void ExportMenuIndex()
        {
            // Create options that you want your menu to have

            options = new List<ExportOption>
            {
                new ExportOption("Create a new Linux based App Service", () => NewLinuxAppServiceHosting()),
                new ExportOption("I already have a Linux based App Service", () =>  ExistingLinuxAppServiceHosting()),
                new ExportOption("Exit", () => Environment.Exit(0)),
            };

           int index = 0;

           WriteMenu(options, options[index]);

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

        private static async void NewLinuxAppServiceHosting()
        {
            Console.Clear();
            await migratingToNewLinuxService.NewLinuxAppService();
        }
        private static async void ExistingLinuxAppServiceHosting()
        {

            Console.Clear();
            await migratingToExistingLinuxService.ExistingLinuxAppService();
        }

        static void WriteMenu(List<ExportOption> options, ExportOption selectedOption)
        {
            Console.Clear();

            Console.WriteLine("We will now begin the export process to a Linux based App Service!");

            foreach (ExportOption option in options)
            {
                if (option == selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
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

    public class ExportOption
    {
        public string Name { get; }
        public Action Selected { get; }

        public ExportOption(string name, Action selected)
        {
            Name = name;
            Selected = selected;
        }
    }

}