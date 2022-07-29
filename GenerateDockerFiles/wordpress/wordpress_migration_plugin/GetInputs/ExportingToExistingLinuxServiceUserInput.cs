using System;
using System.Diagnostics;

namespace DeployUsingARMTemplate
{
    public class GetExportToLinuxServiceInput
    {
        public static string? linuxAppServiceName;
        public static string? linuxResourceGroup;

        //TO DO - Can we get app settings from rg and app name and then extract the wordpress db info? 
        // (No documented solution available)
        public static string? wordpressDatabaseHost;
        public static string? wordpressDatabaseName;
        public static string? wordpressDatabaseUserName;
        public static string? wordpressDatabasePassword;

        public static void LinuxServiceInput()
        {
            Console.Write("App Service Name:  ");
            linuxAppServiceName = Console.ReadLine();

            Console.Write("Resource Group Name:  ");
            linuxResourceGroup = Console.ReadLine();

            Console.Write("WordPress Database Host:  ");
            wordpressDatabaseHost = Console.ReadLine();

            Console.Write("WordPress Database Name:  ");
            wordpressDatabaseName = Console.ReadLine();

            Console.Write("WordPress Username:  ");
            wordpressDatabaseUserName = Console.ReadLine();

            Console.Write("WordPress Password:  ");
            PassEncoder.PasswordChecker(wordpressDatabasePassword);
            wordpressDatabasePassword = Console.ReadLine();

        }

    }
}