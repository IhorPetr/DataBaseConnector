using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataBaseMigrator.Infrastructure
{
    public static class ConnectionStringManger
    {
        public static string CampusBd { get; set; }
        public static string VkdBd { get; set; }
        public static bool ExistConnect() => String.IsNullOrEmpty(CampusBd) || String.IsNullOrEmpty(VkdBd) ? false : true;
    }
}