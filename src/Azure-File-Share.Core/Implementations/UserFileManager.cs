using Azure_File_Share.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure; // Namespace for Azure Configuration Manager
using Microsoft.WindowsAzure.Storage; // Namespace for Storage Client Library
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage
using Microsoft.WindowsAzure.Storage.File; // Namespace for File storage
using System.IO;

namespace Azure_File_Share.Core.Implementations
{
    public class UserFileManager
    {
        public UserFile userFile;
        CloudStorageAccount storageAccount;

        public UserFileManager(CloudStorageAccount storAcc)
        {
            //this.userFile = file;
            this.storageAccount = storAcc;
            //UploadFile();
        }

        public string GetFileName()
        {
            return this.userFile.Id.ToString().Split('-')[0];
        }

        public string GetAzureFileNameFromUrl(string url)
        {
            return url.Split('_').Last();
        }
        public string GetFileNameFromUrl(string url)
        {
            return url.Substring(0, url.LastIndexOf('_'));
        }

        public string GenerateFileId(string name)
        {
            var id = String.Format("{0}_{1}", name, Guid.NewGuid());
            return id.Length > 254 ? id.Substring(0, 254) : id;
        }

        public async Task<byte[]> DownloadFile(string fileUrl)
        {
            // Create a CloudFileClient object for credentialed access to File storage.
            CloudFileClient fileClient = this.storageAccount.CreateCloudFileClient();

            // Get a reference to the file share we created previously.
            CloudFileShare share = fileClient.GetShareReference("shared-files");

            // Ensure that the share exists.
            if (await share.ExistsAsync())
            {
                // Get a reference to the root directory for the share.
                CloudFileDirectory rootDir = share.GetRootDirectoryReference();

                // Get a reference to the directory we created previously.
                CloudFileDirectory sampleDir = rootDir.GetDirectoryReference("testdir");

                // Ensure that the directory exists.
                if (await sampleDir.ExistsAsync())
                {
                    // Get a reference to the file we created previously.
                    CloudFile file = sampleDir.GetFileReference(fileUrl);

                    // Ensure that the file exists.
                    if (await file.ExistsAsync())
                    {
                        // Write the contents of the file to the console window.
                        var ms = new MemoryStream();
                        await file.DownloadToStreamAsync(ms);
                        ms.Position = 0;
                        byte[] result;
                        using (var streamReader = new MemoryStream())
                        {
                            ms.CopyTo(streamReader);
                            result = streamReader.ToArray();
                        }
                        return result;
                        //await file.DownloadToFileAsync($"Downloads", FileMode.OpenOrCreate);
                        ////work version
                        //var ss = file.DownloadTextAsync().Result;
                        //Console.WriteLine(file.DownloadTextAsync().Result);
                    }
                }
            }
            return null;
        }
        public async Task<string> UploadFile(FileStream fileStream, string fileName)
        {
            //FileInfo file = new FileInfo(@"C:\Users\Kostenko\Downloads\20160104_195759.jpg");
            CloudFileClient fileClient = storageAccount.CreateCloudFileClient();
            CloudFileShare share = fileClient.GetShareReference("shared-files");
            // Ensure that the share exists.
            if (await share.ExistsAsync())
            {
                // Get a reference to the root directory for the share.
                CloudFileDirectory rootDir = share.GetRootDirectoryReference();

                // Get a reference to the directory we created previously.
                CloudFileDirectory sampleDir = rootDir.GetDirectoryReference("testdir");

                // Ensure that the directory exists.
                if (await sampleDir.ExistsAsync())
                {
                    var cloudFile = sampleDir.GetFileReference(this.GenerateFileId(fileName));
                    fileStream.Position = 0;
                    await cloudFile.UploadFromStreamAsync(fileStream);
                    return cloudFile.Name;
                    //return cloudFile.StorageUri.PrimaryUri.ToString();
                }
            }
            return null;
        }
    }
}
