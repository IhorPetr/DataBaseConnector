using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DataBaseMigrator.Models
{
    public class SearchResult
    {
        public DataTable ViewResult { get; set; }
        public DataTable CampusResult { get; set; }
    }
}