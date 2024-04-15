using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InventoryManagement.Models.ViewModel
{
    public class ContactVM
    {
        [JsonPropertyName("ContactId")]
        public int Customer_Loc_Cnct_Id { get; set; }

        [JsonPropertyName("Name")]
        public string Contact_Person { get; set; }
        [JsonPropertyName("Email")]
        public string Contact_Email { get; set; }
        [JsonPropertyName("Phone")]
        public string Contact_Phone { get; set; }
        [JsonPropertyName("Extension")]
        public string Contact_Phone_Ext { get; set; }
        [JsonPropertyName("Fax")]
        public string Contact_Fax { get; set; }
    }
}
