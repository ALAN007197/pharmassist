using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace PharmAssist.Models
{
    public class PriscriptionModel
    {
        public MedicineModel SelectedMedicine;
        public int MedId { get; set; } 
        public bool Morning { get; set; }
        public bool Noon { get; set; }
        public bool Evening { get; set; }
        public bool Befor { get; set; }
        public string NoDays { get; set; }
        public double RecordTotal { get; set; }
        public double UnitPrice { get; set; }
        public int Qty { get; set; }
        public int patiantId { get; set; }

        [Ignore]
        public bool IsEnabled { get; set; }
    }

    public class PriscriptionModelDB :BaseDB
    {
        public int MedId { get; set; }
        public int patiantId { get; set; }
        public bool Morning { get; set; }
        public bool Noon { get; set; }
        public bool Evening { get; set; }
        public bool Befor { get; set; }
        public string NoDays { get; set; }
        public double RecordTotal { get; set; }
        public double UnitPrice { get; set; }
        public int TotalQty { get; set; }
        public int Qty { get; set; }
    }
}
