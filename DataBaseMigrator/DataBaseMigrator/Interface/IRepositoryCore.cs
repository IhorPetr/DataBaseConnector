using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataBaseMigrator.Interface
{
    public interface IRepositoryCore
    {
        Dictionary<string, DataTable> GetAllTables();
    }
}
