using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InventoryManagement.Models
{
    public class Paging
    {
        public int TotalRecords { get; set; }

   
        public int MaxPage { get; set; }

        [JsonIgnore]
        public int PageNo { get; set; }

        [JsonIgnore]
        public int pageSize { get; set; }

        public string ContentRange { get; set; }
        public string PreviousPage { get; set; }

        public string NextPage { get; set; }
    }
}
