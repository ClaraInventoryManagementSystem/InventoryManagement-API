using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Models
{
    public class MrInBoxAction
    {
        public int Id { get; set; }
        public int Action { get; set; } // 1 - approve 2- reject
        public string Remarks { get; set; }
    }
}
