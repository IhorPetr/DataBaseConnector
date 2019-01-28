using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataBaseMigrator.Models
{
    public class DataToConvert
    {
        public string[] CampusRows { get; set; }
        public string[] VkdRows { get; set; }
        public string ViewName { get; set; }
        public string TableName { get; set; }
    }
}