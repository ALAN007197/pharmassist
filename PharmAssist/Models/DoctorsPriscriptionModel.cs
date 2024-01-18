using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace PharmAssist.Models
{
    public class DoctorsPriscriptionModel
    {   
        public  OpMethod PatientDetails { get; set; }
        public  List<PriscriptionModel> MedList { get; set; }
        public  double Total  { get; set; }

    }
    public class DoctorsPriscriptionModelDB :BaseDB
    {   
        public int PatientId { get; set; }
        public  double Total  { get; set; }
    }
}
