using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataBaseMigrator.Infrastructure;
using System.Data;

namespace DataBaseMigrator.Models
{
    public class MigratorCore
    {
        private EmployeeTableContext<DataTable> Campus;
        private EmployeeTableContext<c_eE> VKD;
        public MigratorCore(string campusConnect,string VKDConnect)
        {
            this.Campus= new EmployeeTableContext<DataTable>(campusConnect);
            this.VKD = new EmployeeTableContext<c_eE>(VKDConnect);
        }

    }
}