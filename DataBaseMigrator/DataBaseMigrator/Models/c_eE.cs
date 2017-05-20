namespace DataBaseMigrator.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class c_eE
    {
        public long? RowNumber { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_employee { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(255)]
        public string Surname { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Patronymic { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_Sex { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(10)]
        public string SexName { get; set; }

        public int? id_sector { get; set; }

        [StringLength(50)]
        public string SectorName { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_Subdivision { get; set; }

        [StringLength(255)]
        public string SubdivName { get; set; }

        public int? ID_Duties { get; set; }

        [StringLength(80)]
        public string DutiesName { get; set; }

        public int? Id_DutiesSubtype { get; set; }

        public string DutiesSubTypeName { get; set; }

        public int? ID_Group { get; set; }

        [StringLength(50)]
        public string GroupName { get; set; }

        public DateTime? ContractDocumentEndDate { get; set; }

        public int? ID_DCAcademicDegree { get; set; }

        [StringLength(50)]
        public string AcademicDegreeName { get; set; }

        public DateTime? AcademicDegreeDate { get; set; }

        public int? ID_DCAcademicStatus { get; set; }

        [StringLength(50)]
        public string AcademicStatusName { get; set; }

        public DateTime? AcademicStatusDate { get; set; }

        public int? IDEmploymentForm { get; set; }

        [StringLength(100)]
        public string EmploymentActivityCategory { get; set; }

        [StringLength(100)]
        public string EmploymentPersonStatusType { get; set; }

        [StringLength(80)]
        public string DoplataName { get; set; }

        [StringLength(50)]
        public string NadbavkaName { get; set; }

        public int? IdStaffHolidays { get; set; }

        [StringLength(255)]
        public string StaffHolidaysType { get; set; }

        [StringLength(255)]
        public string StaffHolidaysSubtype { get; set; }

        [StringLength(255)]
        public string StaffHolidaysKind { get; set; }

        public int? DoplatiDuties { get; set; }

        public int? DoplatidutiesSubtype { get; set; }

        public int? DoplatiSector { get; set; }

        public int? Doplatisubdivision { get; set; }

        public int? DoplatiGroup { get; set; }

        public int? Nadbavkaduties { get; set; }

        public int? NadbavkaDutiesSubtype { get; set; }

        public int? NadbavkaSector { get; set; }

        public int? NadbavkaSubdivision { get; set; }

        public int? NadbavkaGroup { get; set; }

        public string NadbavkaDutiesName { get; set; }

        public string NadbavkaDutiesSubtypeName { get; set; }

        public string NadbavkaSectorName { get; set; }

        public string NadbavkagroupName { get; set; }

        public string NadbavkaSubdivisionName { get; set; }

        public string DoplatiDutiesName { get; set; }

        public string DoplatiDutiesSubTypeName { get; set; }

        public string DoplatiSectorName { get; set; }

        public string DoplatiSubdivisionName { get; set; }

        public string DoplatiGroupName { get; set; }

        public int? ID_AcademicDegree { get; set; }

        public int? ID_AcademicStatus { get; set; }
    }
}
