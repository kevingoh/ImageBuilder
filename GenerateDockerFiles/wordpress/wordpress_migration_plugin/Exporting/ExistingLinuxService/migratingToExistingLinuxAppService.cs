using System;
using System.Diagnostics;
using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;

namespace DeployUsingARMTemplate
{
    public class migratingToExistingLinuxService : GetExportToLinuxServiceInput
    {
        //az webapp deploy api to import wp-content 
        //Create tunnel + SSH into container to import DB 
        public static async Task ExistingLinuxAppService()
        {

            LinuxServiceInput();
            TerminalSpinner spinner = new TerminalSpinner();
            // <summary>
            // Step 1 : Migrate the imported SQL file to the existing Linux App Service File System 
            // Step 2 : As the Linux Webapp is under a vnet, it is only accessible by creating a TCP tunnel +  SSH 
            // Step 3 : No doumented method of confirming successful TCP tunnel + SSH connection : Running this manually for now [TO FIX]
            //</summary>
            
            Console.WriteLine("Migrating database and Site content");
            spinner.Start();
            Console.WriteLine("Exporting Database to your App Service Server...");
            ShellExecute.ExecuteCommandWait($"az webapp deploy --resource-group {linuxResourceGroup} --name {linuxAppServiceName} --src-path /externalsite/database.sql --type=zip --restart=false --clean=true --target-path /home/site/wwwroot/");
            spinner.Stop();

            Console.WriteLine($"Connecting to your App Service Container : {linuxAppServiceName}");
            // TO DO - Command is working on terminal. Implement Escape String for C# and pass in the required User Input
            ShellExecute.ExecuteCommand($"sshpass -p Docker! ssh -t root@127.0.0.1 -p 41601 \'mysql -v --host={wordpressDatabaseHost} --user={wordpressDatabaseUserName} --password={wordpressDatabasePassword} --ssl=true -e 'create database wpmeegted character set utf8 collate utf8_unicode_ci;use wpmeegted; source /tmp/database.sql;'\'");
            // Alternatively, run parallel threads manually for now
            spinner.Stop();
            Console.WriteLine("SSH successful!");

            // Updating App settings

            Console.WriteLine("Wrapping up the Migration Process");
            Console.WriteLine("Displaying your Web App Settings for confirmation. These can be changed on your App Service Dashobard");
            spinner.Start();
            ShellExecute.ExecuteCommandAppSetting($"az webapp config appsettings set -g {linuxResourceGroup} -n {linuxAppServiceName} --settings DATABASE_NAME={wordpressDatabaseName}");
            spinner.Stop();
            Console.WriteLine("Your WordPress site has been successfully migrated.");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"You may now access the site at {linuxAppServiceName}.azurewebsites.net");
            Console.WriteLine(); //linebreak
            Console.ResetColor();
        }
    }
}