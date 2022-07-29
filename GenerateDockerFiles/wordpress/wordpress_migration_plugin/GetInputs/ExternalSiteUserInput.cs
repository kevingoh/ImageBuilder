using System;
using System.Diagnostics;

namespace DeployUsingARMTemplate
{
    public class GetExternalSiteUserInput
    {
        // For taking manual user input
        public static string? Sqlfilelocation;

        public static string? wpContentFolderLocation;


        public static void ExternalSiteUserInput()
        {

            Console.Write(".SQL file location:  ");
            Sqlfilelocation = Console.ReadLine();

            Console.Write("WP-content folder location:  ");
            wpContentFolderLocation = Console.ReadLine();
        }
    }
}