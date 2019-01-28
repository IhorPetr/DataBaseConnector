using System;
using System.Collections.Generic;
using System.Data;
using DataBaseMigrator.Models;

namespace DataBaseMigrator.Interface
{
    public interface IMigratorRepository
    {
        Dictionary<string, DataTable> GetAllTables();
        void UpdateCampusDatabase(string[] g);
        int GetProgressBarCount(string t);
        void PartialyUpdateCampusDatabase(DataToConvert dataToConvert);
        SearchResult GetBySearchCondition(SearchFilters searchFilters);
    }
}
