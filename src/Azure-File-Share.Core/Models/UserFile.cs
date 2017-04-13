using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Azure_File_Share.Core.Models
{
    public class UserFile
    {
        public UserFile(string id, DateTime creationDate)
        {
            this.CreationDate = creationDate;
            this.Id = id;
        }
        public DateTime CreationDate { get; set; }
        public string Id { get; set; }
    }
}
