using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DataBaseMigrator.Interface;

namespace DataBaseMigrator.Infrastructure
{
    public class RepositoryCore : IRepositoryCore
    {
        public Dictionary<string, DataTable> GetAllTables()
        {
            var Campus = new BaseCore(ConnectionStringManger.CampusBd,
            "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'BASE TABLE'");
            var Vkd = new BaseCore(ConnectionStringManger.VkdBd,
            "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'View'");
            var cur = new Dictionary<string, DataTable>();
            cur.Add("Список Представлень:", Vkd.LocalTable);
            cur.Add("Список Таблиц:", Campus.LocalTable);
            return cur;
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
                NLogCore.LogStatusAplication(ex);
                return false;
            }
            return true;
        }
    }
    class BaseCore
    {
        public DataTable LocalTable { get; set; }
        public BaseCore(string con,string query)
        {
            LocalTable = new DataTable();
            var baseCon1 = new SqlDataAdapter(query,new SqlConnection(con));
            baseCon1.Fill(LocalTable);
        }
    }
}