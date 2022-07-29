using System;
using System.Diagnostics;

namespace DeployUsingARMTemplate
{
    public class GetLinuxServerUserInput
    {
        public static string? linuxWordpressUsername;
        public static string? linuxWordpressPassword;
        public static string? linuxSiteName;
        public static string? linuxUserDatabaseHost;
        public static string? linuxDatabaseUserName;
        public static string? linuxDatabaseName;
        public static string? linuxDatabasePassword;


        public static void LinuxServerUserInput()
        {

            Console.Write("Website Name:  ");
            linuxSiteName = Console.ReadLine();

            Console.Write("WordPress User Name:  ");
            linuxWordpressUsername = Console.ReadLine();

            Console.Write("WordPress Password:  ");
            PassEncoder.PasswordChecker(linuxWordpressPassword);
            linuxWordpressPassword = Console.ReadLine();

             Console.Write("WordPress Database Host:  ");
            linuxUserDatabaseHost = Console.ReadLine();

             Console.Write("WordPress Database Name:  ");
            linuxDatabaseName = Console.ReadLine();

             Console.Write("WordPress Database Username:  ");
            linuxDatabaseUserName = Console.ReadLine();

             Console.Write("WordPress Database Password:  ");
            linuxDatabasePassword = Console.ReadLine();

        }
    }
}