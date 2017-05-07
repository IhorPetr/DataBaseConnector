using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace DataBaseMigrator.Models
{
    public class EnterStatus
    {
        [Required]
        [Display(Name = "Сервер:")]
        public string ServerName1 { get; set; }
        [Required]
        [Display(Name = "Назва БД:")]
        public string BDName1 { get; set; }
        [Required]
        [Display(Name = "Логін:")]
        public string Login1 { get; set; }
        [Required]
        [Display(Name = "Пароль:")]
        [DataType(DataType.Password)]
        public string Password1 { get; set; }
        [Required]
        [Display(Name = "Сервер:")]
        public string ServerName2 { get; set; }
        [Required]
        [Display(Name = "Назва БД:")]
        public string BDName2 { get; set; }
        [Required]
        [Display(Name = "Логін:")]
        public string Login2 { get; set; }
        [Required]
        [Display(Name = "Пароль:")]
        [DataType(DataType.Password)]
        public string Password2 { get; set; }

    }
}