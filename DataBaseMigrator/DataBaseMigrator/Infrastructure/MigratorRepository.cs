using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DataBaseMigrator.Interface;

namespace DataBaseMigrator.Infrastructure
{
    public class MigratorRepository : MigratorCore,IMigratorRepository
    {
        public Dictionary<string, DataTable> GetAllTables()
        {
            var Campus = new BaseCore(ConnectionStringManger.CampusBd,
            "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'BASE TABLE'");
            var Vkd = new BaseCore(ConnectionStringManger.VkdBd,
            "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'View'");
            return new Dictionary<string, DataTable>
            {
                ["Список Представлень:"]= Vkd.LocalTable,
                ["Список Таблиц:"] = Campus.LocalTable
            };
        } 
        public void UpdateCampusDatabase(string[] g)
        {
            var Campus = new BaseCore(ConnectionStringManger.CampusBd,String.Format("SELECT * FROM {0}", g[1]));
            var Vkd = new BaseCore(ConnectionStringManger.VkdBd, String.Format("SELECT * FROM {0}", g[0]));
            var t = Campus.LocalTable;
            var y = Vkd.LocalTable;
            DataCheck(ref t,ref y);
            Campus.LocalTable = t;
            Campus.UpdateDataBase();
            DeleteCheck(ref t,ref y);
            Campus.LocalTable = t;
            Campus.UpdateDataBase();
        }
        public static bool TestConnection(string t)
        {
            try
            {
                using (var test = new SqlConnection(t))
                {
                    test.Open();
                    test.Close();
                }
            }
            catch (SqlException ex)
            {
                NLogCore.LogAplicationError(ex);
                return false;
            }
            return true;
        }
    }
    class BaseCore
    {
        public DataTable LocalTable { get; set; }
        private SqlDataAdapter baseCon1;
        public BaseCore(string con,string query)
        {
            LocalTable = new DataTable();
            baseCon1 = new SqlDataAdapter(query,new SqlConnection(con));
            baseCon1.Fill(LocalTable);
        }
        public void UpdateDataBase()
        {
            NLogCore.LogStatusAplication("!----------------------!");
            NLogCore.LogStatusAplication("Start updating database");
            NLogCore.LogStatusAplication("!----------------------!");
            baseCon1.Update(LocalTable);
            NLogCore.LogStatusAplication("!----------------------!");
            NLogCore.LogStatusAplication("End updating database");
            NLogCore.LogStatusAplication("!----------------------!");
        }
    }
}