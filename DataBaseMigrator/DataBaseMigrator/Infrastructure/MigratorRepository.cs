using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using DataBaseMigrator.Extensions;
using DataBaseMigrator.Interface;
using DataBaseMigrator.Models;
using DataBaseMigrator.SQL;

namespace DataBaseMigrator.Infrastructure
{
    public class MigratorRepository : MigratorCore,IMigratorRepository
    {
        public Dictionary<string, DataTable> GetAllTables()
        {
            var Campus = new BaseCore(ConnectionStringManger.CampusBd,
            "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'BASE TABLE' order by TABLE_NAME");
            var Vkd = new BaseCore(ConnectionStringManger.VkdBd,
            "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'View' order by TABLE_NAME");
            return new Dictionary<string, DataTable>
            {
                ["Список Представлень:"]= Vkd.LocalTable,
                ["Список Таблиц:"] = Campus.LocalTable
            };
        }

        public SearchResult GetBySearchCondition(SearchFilters searchFilters)
        {
            //var getCampusInfo = new BaseCore(ConnectionStringManger.CampusBd, SQLFileProvider.SelectByConditionFromCampusTable, new SqlParameter[]
            //    {
            //        new SqlParameter("@Surname","N'%'" + searchFilters.LastName + "'%'"),
            //        new SqlParameter("@Name","N'%'" + searchFilters.FirstName + "'%'"),
            //        new SqlParameter("@DutiesName","N'%'" + searchFilters.Position + "'%'"),
            //    }).LocalTable;
            //var getVKDInfo = new BaseCore(ConnectionStringManger.VkdBd, SQLFileProvider.SelectByConditionFromViewVKD, new SqlParameter[]
            //{
            //    new SqlParameter("@Surname","N'%'" + searchFilters.LastName + "'%'"),
            //    new SqlParameter("@Name","N'%'" + searchFilters.FirstName + "'%"),
            //    new SqlParameter("@DutiesName","N'%'" + searchFilters.Position + "'%'"),
            //}).LocalTable;
            var getCampusInfo = new BaseCore(ConnectionStringManger.CampusBd,
                string.Format(SQLFileProvider.SelectByConditionFromCampusTable, searchFilters.LastName, searchFilters.FirstName, searchFilters.Position,searchFilters.TableName)).LocalTable;
            var getVKDInfo = new BaseCore(ConnectionStringManger.VkdBd,
                string.Format(SQLFileProvider.SelectByConditionFromViewVKD, searchFilters.LastName, searchFilters.FirstName, searchFilters.Position, searchFilters.ViewName)).LocalTable;

            var result = new SearchResult
            {
                CampusResult = getCampusInfo,
                ViewResult = getVKDInfo
            };

            return result;
        }

        //public DataTable GetCafedra() =>  new BaseCore(ConnectionStringManger.CampusBd,
        //    "SELECT distinct [ID_Subdivision],[SubdivName] FROM [eEmployees1]").LocalTable;
        public  void UpdateCampusDatabase(string[] g)
        {
            var Campus = new BaseCore(ConnectionStringManger.CampusBd,String.Format("SELECT * FROM {0}", g[1]));
            var Vkd = new BaseCore(ConnectionStringManger.VkdBd, String.Format("SELECT * FROM {0}", g[0]));
            UpdateDatabase(Campus, Vkd);
        }

        public void PartialyUpdateCampusDatabase(DataToConvert dataToConvert)
        {
            var Campus = new BaseCore(ConnectionStringManger.CampusBd, 
                String.Format(SQLFileProvider.SelectRangeFromCampusTable, dataToConvert.TableName, string.Join(",", dataToConvert.CampusRows)));
            var Vkd = new BaseCore(ConnectionStringManger.VkdBd, 
                String.Format(SQLFileProvider.SelectRangeFromViewVKD, dataToConvert.ViewName, string.Join(",", dataToConvert.VkdRows)));
            UpdateDatabase(Campus, Vkd);
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
                NLogCore.LogAplicationError(ex.Message);
                return false;
            }
            return true;
        }
        public int GetProgressBarCount(string t)
        {
            var Vkd = new BaseCore(ConnectionStringManger.VkdBd, String.Format("SELECT Count( distinct ID_Employee) as cou FROM {0}", t));
            return Convert.ToInt32(Vkd.LocalTable.AsEnumerable().Select(o => o["cou"]).ElementAt(0));
        }

        private void UpdateDatabase(BaseCore campus, BaseCore vkd)
        {
            var t = campus.LocalTable;
            var y = vkd.LocalTable;
            DataCheck(ref t, ref y);
            campus.UpdateDataBase(t);
            DeleteCheck(ref t, ref y, campus);
            campus.UpdateDataBase(t);
        }
    }
    public class BaseCore
    {
        public DataTable LocalTable { get; set; }
        private SqlDataAdapter baseCon1;
        public BaseCore(string con,string query)
        {
            LocalTable = new DataTable();
            baseCon1 = new SqlDataAdapter(query,new SqlConnection(con));
            baseCon1.Fill(LocalTable);
        }

        public void UpdateDataBase(DataTable t)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(baseCon1);
            NLogCore.LogStatusAplication("!----------------------!");
            NLogCore.LogStatusAplication("Start updating database");
            NLogCore.LogStatusAplication("!----------------------!");
            baseCon1.Update(t);
            NLogCore.LogStatusAplication("!----------------------!");
            NLogCore.LogStatusAplication("End updating database");
            NLogCore.LogStatusAplication("!----------------------!");
        }
    }
}