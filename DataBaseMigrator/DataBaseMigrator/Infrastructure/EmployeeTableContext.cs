using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DataBaseMigrator.Models;
using System.Data;

namespace DataBaseMigrator.Infrastructure
{
    public class EmployeeTableContext<T> : DbContext where T : class
    {
        public EmployeeTableContext(string connectString): base(connectString)
        {  }
        public DbSet<T>  EmployeeTarget  { get; set; } 
    }
}