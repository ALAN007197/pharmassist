using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace PharmAssist.Models
{
    public class MedicineModel : BaseDB
    {
        public int patiantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EXpDate { get; set; }
        public int Qty { get; set; }
        public double Price { get; set; }
        
    }

    public class DrPriMedicineModel : BaseDB
    {
        public int medID { get; set; }
        public int patiantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EXpDate { get; set; }
        public int Qty { get; set; }
        public double Price { get; set; }

    }
}
