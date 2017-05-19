using Microsoft.Extensions.Configuration;
using System.IO;

namespace Azure_File_Share.Helpers
{
    public class CommonFunctions
    {
        public string GetAzureConfigString()
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
