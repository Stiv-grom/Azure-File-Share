using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using Azure_File_Share.Helpers;
using Microsoft.AspNetCore.WebUtilities;
using System.Globalization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Azure_File_Share.Controllers
{
    public class UploadController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();
            var azureConnString = this.GetAzureConfigString();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            #region upload_to_azure
            //FileInfo file = new FileInfo(@"C:\...\file.txt");
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
            //CloudFileClient fileClient = storageAccount.CreateCloudFileClient();
            //CloudFileShare share = fileClient.GetShareReference(ConfigurationManager.AppSettings["ShareReference"]);
            //CloudFileDirectory root = share.GetRootDirectoryReference();
            //CloudFileDirectory dir = root.GetDirectoryReference("TESTDIR");

            //var cloudFile = dir.GetFileReference("myfile.txt");

            //using (FileStream fs = file.OpenRead())
            //{
            //    cloudFile.UploadFromStream(fs);
            //}
            #endregion


            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            //return Ok(new { count = files.Count, size, filePath });
            return Json(new { hasError = true}); // return file url
        }

        private string GetAzureConfigString()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var connectionStringConfig = builder.Build();

            // chain calls together as a fluent API
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return config.GetSection("ConnectionStrings:StorageConnectionString").Value;
        }
    }
}
