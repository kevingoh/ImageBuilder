using System;
using System.Diagnostics;
using System.Net.Http;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using System.Threading.Tasks;

namespace DeployUsingARMTemplate
{
    public class WindowsBasedService : GetWindowsServerUserInput
    {
        public static async Task migratingFromWindowsService()
        {

            Console.Clear();
            WindowsServerUserInput();

            TerminalSpinner spinner = new TerminalSpinner();

            // Import wp-content folder
            spinner.Start();
            Console.WriteLine("Importing wp-content folder");

            await ShellExecute.Execute($"curl -X GET -u {windowsWordpressUsername}:{windowsWordpressPassword} https://{windowsSiteName}.scm.azurewebsites.net/api/zip/site/wwwroot/wp-content/ --output ~/wp-content.zip");
            Console.WriteLine("Imported website content");

            spinner.Stop();

            // Importing the Database
            Console.Write("");
            spinner.Start();
            Console.WriteLine("Importing database....");
            await ShellExecute.Execute($"mysqldump --host={windowsUserDatabaseHost}.mysql.database.azure.com --user={windowsDatabaseUserName} --password={windowsDatabasePassword} {windowsDatabaseName}>~/backupdb.sql");
            spinner.Stop();
            Console.WriteLine("Imported db successfully!");


            // TO DO - Check if Blob Storage exists before uploading file

            // Uploading database file as a temp file to Azure Storage
            // In the wordpress docker image, the file is downloaded from Azure Blob Storage - wpsql under the Storage account - wpsqlstorage

                // string localPath = "~/";
                // string fileName = "backupdb.sql";
                // string localFilePath = Path.Combine(localPath, fileName);
                // string connectionString = "DefaultEndpointsProtocol=https;AccountName=wpsqlstorage;AccountKey=yUUb/x23HlOI/P3rf4gWcvUc1gF2+/P5kF3Er+BWptj2PRD9nbTuuOFSJb7Chp9JQxE/N+SlV0A7+AStJOqgdg==;EndpointSuffix=core.windows.net";
                // BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                // string containerName = "wpsqlstorage";
                // BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
                // // Get a reference to a blob
                // BlobClient blobClient = containerClient.GetBlobClient(fileName);

                // Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

                // // Upload data from the local file
                // await blobClient.UploadAsync(localFilePath, true);



            Console.WriteLine("Collected the required inputs. We will now move on to the Migration process");
            // Thread.Sleep(1000 * 10);
        }

    }
}