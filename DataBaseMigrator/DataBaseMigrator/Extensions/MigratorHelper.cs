using DataBaseMigrator.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataBaseMigrator.Extensions
{
    public static class MigratorHelper
    {
        public static IEnumerable<eEmployees1> ToCampusTable(this DataTable dataTable)
        {
            var converted =  (from rw in dataTable.AsEnumerable()
                select new eEmployees1()
                {
                    eEmployees1Id = rw.Field<int>("eEmployees1Id"),
                    UserAccountId = rw.Field<int?>(""),
                    ID_employee = rw.Field<int>("ID_employee"),
                    Surname = rw.Field<string>("Surname"),
                    Name = rw.Field<string>("Name"),
                    Patronymic = rw.Field<string>("Patronymic"),
                    ID_Sex = rw.Field<int>("ID_Sex"),
                    SexName = rw.Field<string>("SexName"),
                    id_sector = rw.Field<int?>("id_sector"),
                    SectorName = rw.Field<string>("SectorName"),
                    ID_Subdivision = rw.Field<int>("ID_Subdivision"),
                    SubdivName = rw.Field<string>("SubdivName"),
                    ID_Duties = rw.Field<int?>("ID_Duties"),
                    DutiesName = rw.Field<string>("DutiesName"),
                    Id_DutiesSubtype = rw.Field<int?>("Id_DutiesSubtype"),
                    DutiesSubTypeName = rw.Field<string>("DutiesSubTypeName"),
                    ID_Group = rw.Field<int?>("ID_Group"),
                    GroupName = rw.Field<string>("GroupName"),
                    ContractDocumentEndDate = rw.Field<DateTime?>("ContractDocumentEndDate"),
                    ID_AcademicDegree = rw.Field<int?>("ID_AcademicDegree"),
                    ID_DCAcademicDegree = rw.Field<int?>("ID_DCAcademicDegree"),
                    AcademicDegreeName = rw.Field<string>("AcademicDegreeName"),
                    AcademicDegreeDate = rw.Field<DateTime?>("AcademicDegreeDate"),
                    ID_DCAcademicStatus = rw.Field<int?>("ID_DCAcademicStatus"),
                    AcademicStatusName = rw.Field<string>("AcademicStatusName"),
                    AcademicStatusDate = rw.Field<DateTime?>("AcademicStatusDate"),
                    IDEmploymentForm = rw.Field<int?>("IDEmploymentForm"),
                    EmploymentActivityCategory = rw.Field<string>("EmploymentActivityCategory"),
                    EmploymentPersonStatusType = rw.Field<string>("EmploymentPersonStatusType"),
                    DoplataName = rw.Field<string>("DoplataName"),
                    NadbavkaName = rw.Field<string>("NadbavkaName"),
                    IdRtStaffHoliday = rw.Field<int?>("IdRtStaffHoliday"),
                    StaffHolidaysType = rw.Field<string>("StaffHolidaysType"),
                    StaffHolidaysSubtype = rw.Field<string>("StaffHolidaysSubtype"),
                    StaffHolidaysKind = rw.Field<string>("StaffHolidaysKind"),
                    DoplatiDuties = rw.Field<int?>("DoplatiDuties"),
                    DoplatidutiesSubtype = rw.Field<int?>("DoplatidutiesSubtype"),
                    DoplatiSector = rw.Field<int?>("DoplatiSector"),
                    Doplatisubdivision = rw.Field<int?>("Doplatisubdivision"),
                    DoplatiGroup = rw.Field<int?>("DoplatiGroup"),
                    Nadbavkaduties = rw.Field<int?>("Nadbavkaduties"),
                    NadbavkaDutiesSubtype = rw.Field<int?>("NadbavkaDutiesSubtype"),
                    NadbavkaSector = rw.Field<int?>("NadbavkaSector"),
                    NadbavkaSubdivision = rw.Field<int?>("NadbavkaSubdivision"),
                    NadbavkaGroup = rw.Field<int?>("NadbavkaGroup"),
                    NadbavkaDutiesName = rw.Field<string>("NadbavkaDutiesName"),
                    NadbavkaDutiesSubtypeName = rw.Field<string>("NadbavkaDutiesSubtypeName"),
                    NadbavkaSectorName = rw.Field<string>("NadbavkaSectorName"),
                    NadbavkagroupName = rw.Field<string>("NadbavkagroupName"),
                    NadbavkaSubdivisionName = rw.Field<string>("NadbavkaSubdivisionName"),
                    DoplatiDutiesName = rw.Field<string>("DoplatiDutiesName"),
                    DoplatiDutiesSubTypeName = rw.Field<string>("DoplatiDutiesSubTypeName"),
                    DoplatiSectorName = rw.Field<string>("DoplatiSectorName"),
                    DoplatiSubdivisionName = rw.Field<string>("DoplatiSubdivisionName"),
                    DoplatiGroupName = rw.Field<string>("DoplatiGroupName"),
                    vcChangeDate = rw.Field<DateTime?>("vcChangeDate"),
                    vcChangeStatus = rw.Field<string>("vcChangeStatus"),
                    PersonId = rw.Field<int?>("PersonId"),
                    vcContractEnd = rw.Field<string>("vcContractEnd")
                }).ToList();
            return converted;
        }
    }
}