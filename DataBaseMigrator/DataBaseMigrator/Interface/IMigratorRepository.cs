using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataBaseMigrator.Interface
{
    public interface IMigratorRepository
    {
        Dictionary<string, DataTable> GetAllTables();
        void UpdateCampusDatabase(string[] g);
    }
}
