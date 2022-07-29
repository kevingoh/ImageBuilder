using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Resources.Models;
using System;
using System.Diagnostics;
using System.IO;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace DeployUsingARMTemplate
{
    public class Program : GetWindowsServerUserInput
    {
        public static string? switchCase;
        public static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("To proceed with the migration, you need to login to your Azure Account");
                Console.WriteLine("The program will now direct you to the browser with a prompt to sign in on the terminal");
                await ShellExecute.Login("az login");
                Menu.MenuIndex();

            }

            //<summary>
            // Arm Template for deployment resides in Assets Folder.
            //Importing Methods
            //1. Windows based app service to linux based app service
            //2. From external WordPress site

            //Exporting Methods
            //1. Migrating to new Linux based App Service
            //2. Migrating to Existing Linux based App Service 
            //   [Need to Fix - SSH Connection not running on parallel threads (Needed for migrating Database to the Linux App Service under a vnet)]
            //</summary>

            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}