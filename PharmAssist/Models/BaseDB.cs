using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace PharmAssist.Models
{
    public class BaseDB
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }
    }
}
