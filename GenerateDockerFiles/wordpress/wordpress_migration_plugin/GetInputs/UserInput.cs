using System;
using System.Diagnostics;

namespace DeployUsingARMTemplate
{
    public class GetUserInput
    {

        // Input for creating a New Linux based App Service
        public static string GenerateNumber(int length)
        {
            Random random = new Random(length);
            string r = "";
            int i;
            for (i = 1; i < length; i++)
            {
                r += random.Next(0, 9).ToString();
            }
            return r;
        }

        public static string? name;
        public static string? location;

        public static string? wordpressAdminEmail;
        public static string? wordpressUsername;
        public static string? wordpressPassword;
        public static string? ResourceGroup = $"wpmigrated-rg{GenerateNumber(2)}";
        public static string? hostingPlanName = ResourceGroup;
        public static string ServerName = $"wp{GenerateNumber(2)}-db";

        public static string? databaseName = $"{name}-database";
        public static void UserInput()
        {

            Console.WriteLine("Before beginning the migration process, please provide your inputs for a Custom Website. Any fields left blank will be given a unique name");
            Console.WriteLine("P.S - You can change these later in your App Settings as well");
            Console.Write("Website Name:  ");
            name = Console.ReadLine();

            Console.Write("Location:  ");
            location = Console.ReadLine();

            Console.Write("WordPress Admin Email:  ");
            wordpressAdminEmail = Console.ReadLine();

            Console.Write("WordPress UserName:  ");
            wordpressUsername = Console.ReadLine();

            Console.Write("WordPress Password:  ");
            PassEncoder.PasswordChecker(wordpressPassword);
            wordpressPassword = Console.ReadLine();

            Console.WriteLine("Thankyou for the inputs. Deployment will begin now");
        }
    }
}