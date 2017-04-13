using Azure_File_Share.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Azure_File_Share.Core.Implementations
{
    public class UserFileManager
    {
        public UserFile userFile;

        public UserFileManager(UserFile file)
        {
            this.userFile = file;
        }

        public string GetFileName()
        {
            return this.userFile.Id.ToString().Split('-')[0];
        }

        public string GenerateFileId(string name)
        {
            var id = String.Format("{0}-{1}", name, new Guid());
            return id.Length > 254 ? id.Substring(0, 254) : id;
        }
    }
}
