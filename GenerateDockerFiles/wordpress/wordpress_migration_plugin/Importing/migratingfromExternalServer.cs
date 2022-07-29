using System;
using System.Diagnostics;

namespace DeployUsingARMTemplate
{
    public class migratingFromExternalSite : GetExternalSiteUserInput
    {

        public static string? wpressfile_address;
        public static async Task wpressImport()
        {

            Console.Clear();
            Console.WriteLine("We will require the downloaded .WPRESS file for Migration");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Install the All-in-One free Migration plugin -> Go to exports -> Download a local copy of your site -> The extension of this file should be .WPRESS");
            Console.WriteLine(); //linebreak
            Console.ResetColor(); //reset to default values

            Console.WriteLine("Point us to the location of your Downloaded .WPRESS file. Make sure to include the name of the file correctly");
            // Ask for .WPRESS file
            wpressfile_address = Console.ReadLine();
            TerminalSpinner spinner = new TerminalSpinner();
            spinner.Start();
            ShellExecute.ExecuteCommand($"npx wpress-extract {wpressfile_address} --out ./externalsite");
            spinner.Stop();
            Console.WriteLine("You can find a local copy of your exported site in the externalsite folder");

            Console.WriteLine("Collected the needed files. Starting Migration now");

            ShellExecute.ExecuteCommand($"zip {wpressfile_address} wp-content.xip && cp {wpressfile_address} C:/");
        }

        // To ask for manual input from user
        // public static async Task manualImport()
        // {
        //     // TO DO - Add switch cases; try out exporting db with gomigo website
        //     Console.WriteLine("We will require the wp-content folder and the database of your WordPress site for Migration");
        //     Console.WriteLine("Point us to the location of the wp-content folder");

        //     // Take manual input for now
        //     ExternalSiteUserInput();


        //}

    }
}