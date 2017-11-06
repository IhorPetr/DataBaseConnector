using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using DataBaseMigrator.Infrastructure;



namespace DataBaseMigrator.Models
{
    public class EnterStatus
    {
        [Display(Name = "Сервер:")]
        public  string ServerName1 { get; set; }
        [Display(Name = "Назва БД:")]
        public  string BDName1 { get; set; }
        [Display(Name = "Логін:")]
        public  string Login1 { get; set; }
        [Display(Name = "Пароль:")]
        [DataType(DataType.Password)]
        public  string Password1 { get; set; }
        [Display(Name = "Сервер:")]
        public  string ServerName2 { get; set; }
        [Display(Name = "Назва БД:")]
        public  string BDName2 { get; set; }
        [Display(Name = "Логін:")]
        public  string Login2 { get; set; }
        [Display(Name = "Пароль:")]
        [DataType(DataType.Password)]
        public  string Password2 { get; set; }
        public string GetVkdConnectionString() =>
            new SqlConnectionStringBuilder {
                DataSource = ServerName1,
                InitialCatalog= BDName1,
                PersistSecurityInfo=true,
                ConnectTimeout=3,
                UserID=Login1,
                Password=Password1
            }.ToString();
        public  string GetCampusConnectionString() =>
            new SqlConnectionStringBuilder
            {
                DataSource = ServerName2,
                InitialCatalog = BDName2,
                PersistSecurityInfo = true,
                ConnectTimeout = 3,
                UserID = Login2,
                Password = Password2
            }.ToString();
    }
}