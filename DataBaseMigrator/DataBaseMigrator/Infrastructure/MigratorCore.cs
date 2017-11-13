using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.Pkcs;
using System.Web.UI;
using DataBaseMigrator.Models.Config;

namespace DataBaseMigrator.Infrastructure
{
    public abstract class MigratorCore
    {
        protected void DataCheck(ref DataTable campus, ref DataTable Vkd)
        {
            int _maximum = campus.Rows.Count == 0 ? 1 : campus.Rows.Cast<DataRow>().Select(p => Convert.ToInt32(p["eEmployees1Id"])).Max() + 1;

            var EmployeeIdlist = Vkd.AsEnumerable().Select(y =>new
            {
                EmpId=y["ID_employee"],
                SubId=y["ID_Subdivision"]
            }).Distinct().ToList();
           
            var vkdRows = Vkd.Rows.Cast<DataRow>().ToList();
            var campusRows = campus.Rows.Cast<DataRow>().ToList();
            for (int rowNumber = 0; rowNumber < EmployeeIdlist.Count; rowNumber++)
            {
                var row = EmployeeIdlist[rowNumber];

                NLogCore.LogStatusAplication(String.Format("DataCheck : {0} of {1}", rowNumber, EmployeeIdlist.Count));

                var gottenEmployes = vkdRows.Where(o => o["ID_employee"].ToString() == row.EmpId.ToString() 
                && o["ID_Subdivision"].ToString() == row.SubId.ToString())
                .OrderBy(e=>e["RowNumber"].ToString())
                .ToList();
                var existingEmployes = campusRows.Where(t => t["ID_employee"].ToString() == row.EmpId.ToString() 
                && t["ID_Subdivision"].ToString() == row.SubId.ToString() 
                && t["vcChangeStatus"].ToString() != "Видалено")
                .OrderBy(e => e["eEmployees1Id"].ToString())
                .ToList();

                if (gottenEmployes.Count == existingEmployes.Count)
                {
                    for (int i = 0; i < gottenEmployes.Count(); i++)
                    {
                        //if (existingEmployes[i]["vcChangeStatus"].ToString() != "Видалено")
                        //{
                            RowUpdate(existingEmployes[i], gottenEmployes[i]);
                        //}
                        //else
                        //{
                        //    var sourceRow = gottenEmployes[i];
                        //    var newRow = campus.NewRow();
                        //    CloneRow(sourceRow, _maximum, ref newRow);

                        //    campus.Rows.Add(newRow);
                        //    NLogCore.LogStatusAplication("Додано запис з ідентифікатором eEmployees1Id=" + _maximum.ToString());
                        //    _maximum = _maximum + 1;
                        //}
                    }
                }

                if (gottenEmployes.Count > existingEmployes.Count)
                {
                    for (int i = 0; i < gottenEmployes.Count(); i++)
                    {
                        if (i<existingEmployes.Count)
                        {
                            RowUpdate(existingEmployes[i], gottenEmployes[i]);
                        }
                        else
                        {
                            var sourceRow = gottenEmployes[i];
                            var newRow = campus.NewRow();
                            CloneRow(sourceRow, _maximum, ref newRow);

                            campus.Rows.Add(newRow);
                            NLogCore.LogStatusAplication("Додано запис з ідентифікатором eEmployees1Id=" + _maximum.ToString());
                            _maximum = _maximum + 1;
                        }
                    }
                }

                if (gottenEmployes.Count < existingEmployes.Count)
                {
                    //int index1 = existingEmployes.Count;
                    //int index2 = gottenEmployes.Count;
                    //bool check = false;

                    //for (int i = 0; i < index1; i++)
                    //{
                    //    for (int j = 0; j < index2; j++)
                    //    {
                    //        if (check)
                    //        {
                    //            i = 0; j = 0; check = false;
                    //        }

                    //        if (existingEmployes[i]["ID_Subdivision"].ToString() == gottenEmployes[j]["ID_Subdivision"].ToString())
                    //        {
                    //            RowUpdate(existingEmployes[i], gottenEmployes[j]);
                    //            existingEmployes.RemoveAt(i);
                    //            gottenEmployes.RemoveAt(j);
                    //            index1 = index1 - 1;
                    //            index2 = index2 - 1;
                    //            check = true;
                    //        }
                    //    }
                    //}

                    //for (int i = 0; i < index1; i++)
                    //{
                    //for (int j = 0; j < index2; j++)
                    //{
                    //    if (check)
                    //    {
                    //         j = 0; check = false;
                    //    }
                    int index2 = 0;

                    foreach (var gotEmployee in gottenEmployes.ToArray())
                    {
                        if (existingEmployes.Any(r => r["ID_Duties"].ToString() == gotEmployee["ID_Duties"].ToString())
                            && existingEmployes.Any(r => r["ID_Group"].ToString() == gotEmployee["ID_Group"].ToString())
                            && existingEmployes.Any(r => r["IDEmploymentForm"].ToString() == gotEmployee["IDEmploymentForm"].ToString()))
                        {
                            var getthis = existingEmployes.Find(r =>
                                r["ID_Duties"].ToString() == gotEmployee["ID_Duties"].ToString() &&
                                r["ID_Group"].ToString() == gotEmployee["ID_Group"].ToString() &&
                                r["IDEmploymentForm"].ToString() == gotEmployee["IDEmploymentForm"].ToString());
                            // var 
                            var index1 = existingEmployes.FindIndex(r =>
                                r["ID_Duties"].ToString() == gotEmployee["ID_Duties"].ToString() &&
                                r["ID_Group"].ToString() == gotEmployee["ID_Group"].ToString() &&
                                r["IDEmploymentForm"].ToString() == gotEmployee["IDEmploymentForm"].ToString());
                            RowUpdate(getthis, gotEmployee);
                            existingEmployes.RemoveAt(index1);
                            gottenEmployes.RemoveAt(index2);
                            //index1 = index1 - 1;
                            //index2 = index2 - 1;
                            //check = true;
                        }
                        index2++;
                    }
                

                   
                   // }

                    if (existingEmployes.Count != 0)
                    {
                        for (int i = 0; i < existingEmployes.Count; i++)
                        {
                            //if (existingEmployes[i]["vcChangeStatus"].ToString() != "Видалено")
                            //{
                                existingEmployes[i]["vcChangeStatus"] = "Видалено";
                                existingEmployes[i]["vcChangeDate"] = DateTime.Now;

                                NLogCore.LogStatusAplication("Запис з ідентифікатором eEmployees1Id=" + existingEmployes[i]["eEmployees1Id"].ToString() + " видалено");
                           // }
                        }
                    }

                    if (gottenEmployes.Count != 0)
                    {
                        for (int i = 0; i < gottenEmployes.Count; i++)
                        {
                            var sourceRow = gottenEmployes[i];
                            var newRow = campus.NewRow();
                            CloneRow(sourceRow, _maximum, ref newRow);

                            campus.Rows.Add(newRow);
                            NLogCore.LogStatusAplication("Додано запис з ідентифікатором eEmployees1Id=" + _maximum.ToString());
                            _maximum = _maximum + 1;
                        }
                    }
                }
            }
        }
        protected void DeleteCheck(ref DataTable campus, ref DataTable Vkd)
        {
            var employeesId = campus.AsEnumerable().Select(o=>new
            {
                EmpId = o["ID_employee"],
                SubId = o["ID_Subdivision"]
            }).Distinct();

            foreach (var ident in employeesId)
            {

                var empl = Vkd.AsEnumerable().Where(p => p["ID_employee"].ToString() == ident.EmpId.ToString()
                && p["ID_Subdivision"].ToString() == ident.SubId.ToString())
                    .Select(u => u["Surname"]).ToList();

                if (!empl.Any())
                {
                    var employees = campus.Select("ID_employee=" + ident);

                    foreach (var row in employees)
                    {
                        if (row["vcChangeStatus"].ToString() != "Видалено")
                        {
                            row["vcChangeStatus"] = "Видалено";
                            row["vcChangeDate"] = DateTime.Now;

                            NLogCore.LogStatusAplication("Запис з ідентифікатором eEmployees1Id=" + row["eEmployees1Id"] + " видалено");
                        }
                    }
                }
            }
        }
        #region DataBaseConventorHelper
        private void RowUpdate(DataRow oldRow, DataRow newRow)
        {
           List<string> columns = new List<string>();
           List<string> newVals = new List<string>();
           bool flag = false;
           foreach (var columname in CountNameColumn.NameColumn)
           {
               flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, columname);
           }
               
           if (flag)
           {
               oldRow["UserAccountId"] = GetUserAccountID(newRow); 
               oldRow["vcChangeDate"] = DateTime.Now;
               oldRow["vcChangeStatus"] = "Оновлено";
               for (int i = 0; i < newVals.Count; i++)
               {
                  NLogCore.LogStatusAplication("Запис з ідентифікатором eEmployees1Id=" +
                                               oldRow["eEmployees1Id"].ToString() +
                                               " змінено: значення поля " +
                                               columns[i].ToString() + " замінено на " +
                                               newVals[i].ToString());
                }                    
           }      
        }
        private bool CellUpdate(ref DataRow oldRow, DataRow newRow, ref List<string> columns, ref List<string> newVals, string colName)
        {
            bool flag = false;
            if (colName == "IdRtStaffHoliday")
            {
                try
                {
                    if (oldRow["IdRtStaffHoliday"].ToString() != newRow["IdStaffHolidays"].ToString())
                    {
                        oldRow["IdRtStaffHoliday"] = newRow["IdStaffHolidays"];
                        columns.Add(colName);
                        newVals.Add(newRow["IdStaffHolidays"].ToString());
                        flag = true;
                    }
                }
                catch
                {
                    if (oldRow["IdRtStaffHoliday"].ToString() != newRow["IdRtStaffHoliday"].ToString())
                    {
                        oldRow["IdRtStaffHoliday"] = newRow["IdRtStaffHoliday"];
                        columns.Add(colName);
                        newVals.Add(newRow[colName].ToString());
                        flag = true;
                    }
                }
                return flag;
            }
            if (oldRow[colName].ToString() != newRow[colName].ToString())
            {
                oldRow[colName] = newRow[colName];
                columns.Add(colName);
                newVals.Add(newRow[colName].ToString());
                flag = true;
            }
            return flag;
        }
        private void CloneRow(DataRow sourceRow, int employeesId, ref DataRow newRow)
        {
                newRow["eEmployees1Id"] = employeesId;
                foreach (var columname in CountNameColumn.NameColumn)
                {
                    try
                    {
                        newRow[columname] = sourceRow[columname];
                    }
                    catch
                    {
                        newRow["IdRtStaffHoliday"] = sourceRow["IdStaffHolidays"];
                    }
                }
                newRow["UserAccountId"] = GetUserAccountID(sourceRow);
                newRow["vcChangeDate"] = DateTime.Now;
                newRow["vcChangeStatus"] = "Створено";
            
        }

        private object GetUserAccountID(DataRow currentrow)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStringManger.CampusBd))
            {
                connection.Open();
                var Employee = currentrow["ID_employee"].ToString();
                var cmd = new SqlCommand($"select distinct UserAccountId from eEmployees1 where ID_employee=@ID_employee"
                    , connection);
                cmd.Parameters.Add("@ID_employee", SqlDbType.NVarChar);
                cmd.Parameters["@ID_employee"].Value = Employee;
                var result = cmd.ExecuteReader();
                object userid = null;
                //result.IsDBNull();
                if (result.HasRows)
                {
                    result.Read();
                    if (result["UserAccountId"] != DBNull.Value)
                    {
                        userid =  result.GetInt32(0);
                    }
                    else
                    {
                        userid = DBNull.Value;
                    }
                }
                return userid;
            }
        }
        #endregion
    }
}