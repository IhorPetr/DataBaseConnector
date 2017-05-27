using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DataBaseMigrator.Infrastructure
{
    public abstract class MigratorCore
    {
        protected void DataCheck(ref DataTable campus, ref DataTable Vkd)
        {
            int _maximum = campus.Rows.Count == 0 ? 1 : campus.Rows.Cast<DataRow>().Select(p => Convert.ToInt32(p["eEmployees1Id"])).Max() + 1;

            var EmployeeIdlist = Vkd.AsEnumerable().Select(y => y["ID_employee"]).Distinct().ToList();
            var vkdRows = Vkd.Rows.Cast<DataRow>().ToList();
            var campusRows = campus.Rows.Cast<DataRow>().ToList();
            for (int rowNumber = 0; rowNumber < EmployeeIdlist.Count; rowNumber++)
            {
                var row = EmployeeIdlist[rowNumber];
                NLogCore.LogStatusAplication(String.Format("DataCheck : {0} of {1}", rowNumber, EmployeeIdlist.Count));

                var gottenEmployes = vkdRows.Where(o => o["ID_employee"].ToString() == row.ToString()).ToList();
                var existingEmployes = campusRows.Where(t => t["ID_employee"].ToString() == row.ToString()).ToList();

                if (gottenEmployes.Count == existingEmployes.Count)
                {
                    for (int i = 0; i < gottenEmployes.Count(); i++)
                    {
                        RowUpdate(existingEmployes[i], gottenEmployes[i]);
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
                    int index1 = existingEmployes.Count;
                    int index2 = gottenEmployes.Count;
                    bool check = false;

                    for (int i = 0; i < index1; i++)
                    {
                        for (int j = 0; j < index2; j++)
                        {
                            if (check)
                            {
                                i = 0; j = 0; check = false;
                            }

                            if (existingEmployes[i]["ID_Subdivision"].ToString() == gottenEmployes[j]["ID_Subdivision"].ToString())
                            {
                                RowUpdate(existingEmployes[i], gottenEmployes[j]);
                                existingEmployes.RemoveAt(i);
                                gottenEmployes.RemoveAt(j);
                                index1 = index1 - 1;
                                index2 = index2 - 1;
                                check = true;
                            }
                        }
                    }

                    for (int i = 0; i < index1; i++)
                    {
                        for (int j = 0; j < index2; j++)
                        {
                            if (check)
                            {
                                i = 0; j = 0; check = false;
                            }

                            if (existingEmployes[i]["ID_Duties"].ToString() == gottenEmployes[j]["ID_Duties"].ToString() || existingEmployes[i]["ID_Group"].ToString() == gottenEmployes[j]["ID_Group"].ToString() || existingEmployes[i]["IDEmploymentForm"].ToString() == gottenEmployes[j]["IDEmploymentForm"].ToString())
                            {
                                RowUpdate(existingEmployes[i], gottenEmployes[j]);
                                existingEmployes.RemoveAt(i);
                                gottenEmployes.RemoveAt(j);
                                index1 = index1 - 1;
                                index2 = index2 - 1;
                                check = true;
                            }
                        }
                    }

                    if (existingEmployes.Count != 0)
                    {
                        for (int i = 0; i < existingEmployes.Count; i++)
                        {
                            if (existingEmployes[i]["vcChangeStatus"].ToString() != "Видалено")
                            {
                                existingEmployes[i]["vcChangeStatus"] = "Видалено";
                                existingEmployes[i]["vcChangeDate"] = DateTime.Now;

                                NLogCore.LogStatusAplication("Запис з ідентифікатором eEmployees1Id=" + existingEmployes[i]["eEmployees1Id"].ToString() + " видалено");
                            }
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
            var employeesId = campus.AsEnumerable().Select(o => o["ID_Employee"]).Distinct();

            foreach (var ident in employeesId)
            {

                var empl = Vkd.AsEnumerable().Where(p => p["ID_employee"].ToString() == ident.ToString())
                    .Select(u => u["Surname"]).ToList();

                if (!empl.Any())
                {
                    var employees = campus.Select("ID_Employee=" + ident);

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
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "ID_Employee") ;
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "Surname");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "Name");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "Patronymic");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "ID_Sex");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "SexName");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "id_sector");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "SectorName");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "ID_Subdivision");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "SubdivName");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "ID_Duties");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "DutiesName");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "Id_DutiesSubtype");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "DutiesSubTypeName");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "ID_Group");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "GroupName");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "ContractDocumentEndDate");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "ID_AcademicDegree");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "ID_DCAcademicDegree");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "AcademicDegreeName");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "AcademicDegreeDate");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "ID_AcademicStatus");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "ID_DCAcademicStatus");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "AcademicStatusName");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "AcademicStatusDate");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "IDEmploymentForm");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "EmploymentActivityCategory");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "EmploymentPersonStatusType");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "DoplataName");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "NadbavkaName");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "IdRtStaffHoliday");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "StaffHolidaysType");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "StaffHolidaysSubtype");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "StaffHolidaysKind");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "DoplatiDuties");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "DoplatiDutiesSubtype");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "DoplatiSector");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "DoplatiSubdivision");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "DoplatiGroup");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "NadbavkaDuties");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "NadbavkaDutiesSubtype");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "NadbavkaSector");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "NadbavkaSubdivision");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "NadbavkaGroup");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "NadbavkaDutiesName");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "NadbavkaDutiesSubtypeName");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "NadbavkaSectorName");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "NadbavkaSubdivisionName");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "NadbavkaGroupName");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "DoplatiDutiesName");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "DoplatiDutiesSubtypeName");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "DoplatiSectorName");
            flag |= CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "DoplatiSubdivisionName");
            if ((CellUpdate(ref oldRow, newRow, ref columns, ref newVals, "DoplatiGroupName") || flag) || (oldRow["vcChangeStatus"].ToString() == "Видалено"))
            {
                oldRow["vcChangeDate"] = DateTime.Now;
                oldRow["vcChangeStatus"] = "Оновлено";
                for (int i = 0; i < newVals.Count; i++)
                {
                    NLogCore.LogStatusAplication("Запис з ідентифікатором eEmployees1Id=" + oldRow["eEmployees1Id"].ToString() + " змінено: значення поля " + columns[i].ToString() + " замінено на " + newVals[i].ToString());
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
            newRow["ID_employee"] = sourceRow["ID_employee"];
            newRow["Surname"] = sourceRow["Surname"];
            newRow["Name"] = sourceRow["Name"];
            newRow["Patronymic"] = sourceRow["Patronymic"];
            newRow["ID_Sex"] = sourceRow["ID_Sex"];
            newRow["SexName"] = sourceRow["SexName"];
            newRow["id_sector"] = sourceRow["id_sector"];
            newRow["SectorName"] = sourceRow["SectorName"];
            newRow["ID_Subdivision"] = sourceRow["ID_Subdivision"];
            newRow["SubdivName"] = sourceRow["SubdivName"];
            newRow["ID_Duties"] = sourceRow["ID_Duties"];
            newRow["DutiesName"] = sourceRow["DutiesName"];
            newRow["Id_DutiesSubtype"] = sourceRow["Id_DutiesSubtype"];
            newRow["DutiesSubTypeName"] = sourceRow["DutiesSubTypeName"];
            newRow["ID_Group"] = sourceRow["ID_Group"];
            newRow["GroupName"] = sourceRow["GroupName"];
            newRow["ContractDocumentEndDate"] = sourceRow["ContractDocumentEndDate"];
            newRow["ID_AcademicDegree"] = sourceRow["ID_AcademicDegree"];
            newRow["ID_DCAcademicDegree"] = sourceRow["ID_DCAcademicDegree"];
            newRow["AcademicDegreeName"] = sourceRow["AcademicDegreeName"];
            newRow["AcademicDegreeDate"] = sourceRow["AcademicDegreeDate"];
            newRow["ID_AcademicStatus"] = sourceRow["ID_AcademicStatus"];
            newRow["ID_DCAcademicStatus"] = sourceRow["ID_DCAcademicStatus"];
            newRow["AcademicStatusName"] = sourceRow["AcademicStatusName"];
            newRow["AcademicStatusDate"] = sourceRow["AcademicStatusDate"];
            newRow["IDEmploymentForm"] = sourceRow["IDEmploymentForm"];
            newRow["EmploymentActivityCategory"] = sourceRow["EmploymentActivityCategory"];
            newRow["EmploymentPersonStatusType"] = sourceRow["EmploymentPersonStatusType"];
            newRow["DoplataName"] = sourceRow["DoplataName"];
            newRow["NadbavkaName"] = sourceRow["NadbavkaName"];
            try
            {
                newRow["IdRtStaffHoliday"] = sourceRow["IdRtStaffHoliday"];
            }
            catch
            {
                newRow["IdRtStaffHoliday"] = sourceRow["IdStaffHolidays"];
            }
            newRow["StaffHolidaysType"] = sourceRow["StaffHolidaysType"];
            newRow["StaffHolidaysSubtype"] = sourceRow["StaffHolidaysSubtype"];
            newRow["StaffHolidaysKind"] = sourceRow["StaffHolidaysKind"];
            newRow["DoplatiDuties"] = sourceRow["DoplatiDuties"];
            newRow["DoplatidutiesSubtype"] = sourceRow["DoplatidutiesSubtype"];
            newRow["DoplatiSector"] = sourceRow["DoplatiSector"];
            newRow["Doplatisubdivision"] = sourceRow["Doplatisubdivision"];
            newRow["DoplatiGroup"] = sourceRow["DoplatiGroup"];
            newRow["Nadbavkaduties"] = sourceRow["Nadbavkaduties"];
            newRow["NadbavkaDutiesSubtype"] = sourceRow["NadbavkaDutiesSubtype"];
            newRow["NadbavkaSector"] = sourceRow["NadbavkaSector"];
            newRow["NadbavkaSubdivision"] = sourceRow["NadbavkaSubdivision"];
            newRow["NadbavkaGroup"] = sourceRow["NadbavkaGroup"];
            newRow["NadbavkaDutiesName"] = sourceRow["NadbavkaDutiesName"];
            newRow["NadbavkaDutiesSubtypeName"] = sourceRow["NadbavkaDutiesSubtypeName"];
            newRow["NadbavkaSectorName"] = sourceRow["NadbavkaSectorName"];
            newRow["NadbavkagroupName"] = sourceRow["NadbavkagroupName"];
            newRow["NadbavkaSubdivisionName"] = sourceRow["NadbavkaSubdivisionName"];
            newRow["DoplatiDutiesName"] = sourceRow["DoplatiDutiesName"];
            newRow["DoplatiDutiesSubTypeName"] = sourceRow["DoplatiDutiesSubTypeName"];
            newRow["DoplatiSectorName"] = sourceRow["DoplatiSectorName"];
            newRow["DoplatiSubdivisionName"] = sourceRow["DoplatiSubdivisionName"];
            newRow["DoplatiGroupName"] = sourceRow["DoplatiGroupName"];
            newRow["vcChangeDate"] = DateTime.Now;
            newRow["vcChangeStatus"] = "Створено";
        }
        #endregion
    }
}