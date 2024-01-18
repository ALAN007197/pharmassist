using System;
using SQLite;

namespace PharmAssist.Models
{
    public class OpMethod :BaseDB
    {
        public int OpticketId { get; set; }    
        public string OpName { get; set; }
        public string OpAddress { get; set; }
        public string OpAge { get; set; }
        public string OpMobileNo { get; set; }
        public string OpGender { get; set; }
        public string OpDepartMent { get; set; }
        public string OpDoctor { get; set; }
        public bool IsDoctorComplete { get; set; }
        public bool IsPharmacyCompleted { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
