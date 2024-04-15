using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Models
{
    public class TableData
    {

        public string Name { get; set; }

        public IEnumerable<TableColumnData> Columns { get; set; }

        public IEnumerable<Dictionary<string,string>> Rows { get; set; }

        public Paging Paging { get; set; }

        public bool HasRows
        {
            get
            {
                if (Rows != null && Rows.Count() > 0) return true;
                return false;

            }
        }

        public static bool HasAnyRows( IList<TableData> list )
        {
            foreach (TableData t in list)
                if (!t.HasRows) return false;

            return true;
        }


        
    }
}
