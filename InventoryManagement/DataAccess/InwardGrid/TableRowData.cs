using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace InventoryManagement.Models
{
    public class TableRowData
    {

        public string AR_NUMBER { get; set; }

        public int INWARD_REGISTER_ID { get; set; }
        public DateTime REGISTRED_DATE { get; set; }

        public string CUSTOMER_NAME { get; set; }

        public int CUSTOMER_ID { get; set; }

        public string LOCATION_SHORT_NAME { get; set; }

        public int CUSTOMER_LOC_ID { get; set; }

        public string CONTACT_PERSON { get; set; }

        public int CUSTOMER_LOC_CNCT_ID { get; set; }

        public int AR_STATUS { get; set; }

        public int AR_CATEGORY { get; set; }

        public int AR_SUB_CATEGORY { get; set; }

        public string PO_NUMBER { get; set; } 

        public string OTHER_CUSTOMER_ID { get; set; }

        public double INVOICE_AMOUNT { get; set; }

        public bool IS_RUSH { get; set; }

        public int CREATED_BY { get; set; }

        public int LAST_MODIFIED_BY { get; set; }

        public string LAST_MODIFIED_DATE { get; set; }


        public string MATERIAL_SHORT_NAME { get; set; }

        public int MATERIAL_CONDIATION { get; set; }

        public int MATERIAL_CONDITION_REMARKS { get; set; }

        public int RETURN_MATERIAL { get; set; }

        public string TEST_METHOD { get; set; }

        public string TEST_METHOD_REF { get; set; }

        public int MATERIAL_TYPE { get; set; }

        public int MATERIAL_CATEGORY { get; set; }

        public int CONTROLLED_SUBSTANCE { get; set; }

        public int DISPOSABLE_TYPE { get; set; }

        public int NUMBER_OF_LOTS { get; set; }
        
        public int TOTAL_TESTS_COUNT { get; set; }

    }
}
