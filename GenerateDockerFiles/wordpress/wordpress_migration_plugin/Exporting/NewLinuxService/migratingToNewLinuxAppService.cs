using System;
using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Resources.Models;
using System.Diagnostics;


namespace DeployUsingARMTemplate
{
    public class migratingToNewLinuxService : GetUserInput
    {
        private static ResourceIdentifier? _resourceGroupId = null;
        public static TerminalSpinner spinner = new TerminalSpinner();

        public static async Task RunSample(ArmClient client)
        {
            try
            {

                // Create resource group.
                Console.WriteLine($"Creating a resource group with name: {ResourceGroup}");

                var subscription = await client.GetDefaultSubscriptionAsync();

                var rgLro = await subscription.GetResourceGroups().CreateOrUpdateAsync(WaitUntil.Completed, ResourceGroup, new ResourceGroupData(AzureLocation.WestUS));
                var resourceGroup = rgLro.Value;
                _resourceGroupId = resourceGroup.Id;

                Console.WriteLine($"Created a resource group: {_resourceGroupId}");

                Console.WriteLine($"Starting a deployment for an Azure App Service: {name}");


                var templateContent = File.ReadAllText(Path.Combine(".", "Asset", "WordPress.json")).TrimEnd();
                var deploymentContent = new ArmDeploymentContent(new ArmDeploymentProperties(ArmDeploymentMode.Incremental)
                {
                    Template = BinaryData.FromString(templateContent),
                    Parameters = BinaryData.FromObjectAsJson(new
                    {
                        name = new
                        {
                            value = name
                        },
                        location = new
                        {
                            value = location
                        },
                        wordpressAdminEmail = new
                        {
                            value = wordpressAdminEmail
                        },
                        wordpressUsername = new
                        {
                            value = wordpressUsername
                        },
                        wordpressPassword = new
                        {
                            value = wordpressPassword
                        },
                        hostingPlanName = new
                        {
                            value = hostingPlanName
                        },
                        serverFarmResourceGroup = new
                        {
                            value = ResourceGroup
                        },
                        serverName = new
                        {
                            value = ServerName
                        },
                        databaseName = new
                        {
                            value = databaseName
                        },
                        cdnProfileName = new
                        {
                            value = $"wordpressprofilename-{GenerateNumber(6)}-cdnprofile"
                        },
                        cdnEndpointName = new
                        {
                            value = $"wordpressendpoint-{GenerateNumber(6)}-endpoint"
                        }
                        // cdnEndpointProperties = new
                        // {
                        //     value = new
                        //     {
                        //         originHostHeader = $"{name}.azurewebsites.net",
                        //         origins = new
                        //         {
                        //             name = $"{name}.azurewebsites.net",
                        //             hostName = $"{name}.azurewebsites.net",
                        //             hostNameoriginHostHeader = $"{name}.azurewebsites.net"
                        //         }
                        //     }
                        // },
                        // // CDN endpoints can be defined here if needed
                    })
                });

                Console.WriteLine("Sourcing metadata and syncing your inputs...");
                await resourceGroup.GetArmDeployments().CreateOrUpdateAsync(WaitUntil.Completed, name, deploymentContent);

            }
            finally
            {
                try
                {
                    if (_resourceGroupId is not null)
                    {
                        Console.WriteLine($"Completed the WordPress deployment: {name}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        public static async Task NewLinuxAppService()
        {

            UserInput();

            //Authenticating

            var credential = new DefaultAzureCredential();

            var subscriptionId = Environment.GetEnvironmentVariable("AZURE_SUBSCRIPTION_ID");

            var client = new ArmClient(credential, subscriptionId);

            await RunSample(client);
            

            // Importing wp-content folder

            spinner.Start();
            Console.WriteLine("Starting WordPress site migration...");
            ShellExecute.ExecuteCommandWait($"az webapp deploy --resource-group {ResourceGroup} --name {name} --src-path C:/wp-content.zip --type=zip --restart=false --clean=true --target-path /home/site/wwwroot/wp-content/");
            spinner.Stop();
            Console.WriteLine("WordPress site migration completed");

            // Updating App settings

            Console.WriteLine("Wrapping up the Migration Process");
            ShellExecute.ExecuteCommandAppSetting($"az webapp config appsettings set -g {ResourceGroup} -n {name} --settings DATABASE_NAME=wpmigrateddb");
            spinner.Stop();
            Console.WriteLine("Your WordPress site has been successfully migrated.");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"You may now access the site at {name}.azurewebsites.net");
            Console.ResetColor();

        }
    }

}