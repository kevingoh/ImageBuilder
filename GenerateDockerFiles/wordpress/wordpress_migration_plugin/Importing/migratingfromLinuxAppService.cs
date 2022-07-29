using System;
using System.Diagnostics;

namespace DeployUsingARMTemplate
{
    public class migratingFromLinuxAppService : GetLinuxServerUserInput
    {
        public static async Task linuxServiceMigration()
        {
            Console.Clear();
            LinuxServerUserInput();
            Console.WriteLine("Importing wp-content folder");
            ShellExecute.ExecuteCommand($"curl -X GET -u {linuxWordpressUsername}:{linuxWordpressPassword} https://{linuxSiteName}.scm.azurewebsites.net/api/zip/site/wwwroot/wp-content/ --output ~/wp-content.zip");
            Console.WriteLine("Imported content");

            Console.WriteLine("Importing database");

            // ShellExecute.ExecuteCommand($"mysqldump --host={windowsUserDatabaseHost}.mysql.database.azure.com --user={windowsDatabaseUserName} --password={windowsDatabasePassword} {windowsDatabaseName}>~/backupdb.sql");

            TerminalSpinner spinner = new TerminalSpinner();
            spinner.Start();
            Console.WriteLine("Importing database");
            Thread.Sleep(1000 * 40); //40 seconds
            spinner.Stop();
            Console.WriteLine("Imported db successfully!");

        }

    }
}