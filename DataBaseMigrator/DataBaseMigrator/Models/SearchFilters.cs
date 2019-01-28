using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataBaseMigrator.Models
{
    public class SearchFilters
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Position { get; set; }
        public string ViewName { get; set; }
        public string TableName { get; set; }
    }
}