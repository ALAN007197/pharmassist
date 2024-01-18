using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace PharmAssist.Models
{
    public class DoctorsDetailModel:BaseDB
    {
        public string DrName { get; set; }
        public string DrAddress { get; set; }
        public string DrDepartment { get; set; }
        public string DrQualification { get; set; }
        public string DrTime { get; set; }
        public string DrAge { get; set; }
        public string DrMobile { get; set; }
        public string DrGender { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime ToDate { get; set; }

    }
}
