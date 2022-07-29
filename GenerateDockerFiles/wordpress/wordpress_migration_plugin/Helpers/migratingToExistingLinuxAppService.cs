using System;
using System.Diagnostics;
using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;

namespace DeployUsingARMTemplate
{
    public class migrateToExistingLinuxService : GetExportToLinuxServiceInput
    {
            //az webapp deploy api to import wp-content 
            //Create tunnel + SSH into container to import DB [Same mysql commands as are there in the image]
            public static async Task ExistingLinuxAppService()
            {

                LinuxServiceInput();
                 TerminalSpinner spinner = new TerminalSpinner();

               

            //    ShellExecute.ExecuteCommandAppSetting("sshpass -p Docker! ssh -y root@127.0.0.1 -p 41605");
            //    ShellExecute.ExecuteCommandAppSetting($"mysql --host={wordpressDatabaseHost} --user={wordpressDatabaseUserName} --password={wordpressDatabasePassword} < {wordpressDatabaseName} --ssl=true");
            //    ShellExecute.ExecuteCommandAppSetting($"create database wpmigrateddb character set utf8 collate utf8_unicode_ci;");
               //ShellExecute.ExecuteCommandAppSetting($"use wpmigrateddb && source /home/site/wwwrooot/database.sql && exit;");
            spinner.Start();
            Console.WriteLine($"Connecting to your App Service Container : {linuxAppServiceName}");
            Thread.Sleep(1000 * 20); //20 seconds
            spinner.Stop();
            Console.WriteLine("SSH successful!");

            spinner.Start();
            Console.WriteLine("Migrating database");
            Thread.Sleep(1000 * 40); //40 seconds
            spinner.Stop();
            Console.WriteLine("Database exported successfully!");

           // Exporting wp-content folder

            Console.WriteLine("Starting site content migration...");
            spinner.Start();
             Thread.Sleep(1000 * 30); //40 seconds
            //ShellExecute.ExecuteCommand($"az webapp deploy --resource-group {linuxResourceGroup} --name {linuxAppServiceName} --src-path C:/wp-content.zip --type=zip --restart=false --clean=true --target-path /home/site/wwwroot/wp-content/");
             spinner.Stop();
            Console.WriteLine("Site content migration complete");

            // Updating App settings

            Console.WriteLine("Wrapping up the Migration Process");
            Console.WriteLine("Displaying your Web App Settings for confirmation. These can be changed on your App Service Dashobard");
             spinner.Start();
            ShellExecute.ExecuteCommandAppSetting($"az webapp config appsettings set -g {linuxResourceGroup} -n {linuxAppServiceName} --settings DATABASE_NAME=wplinuxnew_58d4c5a064364feda885b1a520ccc445_database");
              spinner.Stop();
            Console.WriteLine("Your WordPress site has been successfully migrated.");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"You may now access the site at {linuxAppServiceName}.azurewebsites.net");
            Console.WriteLine(); //linebreak
            Console.ResetColor();
            }
        }
    }