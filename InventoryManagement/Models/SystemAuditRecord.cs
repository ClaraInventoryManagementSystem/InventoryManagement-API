using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Models
{
    public class SystemAuditRecord
    {

        public override string ToString()
        {
            return string.Format("Action={0} AUDIT_DATE={1},USER={2} ROLE={3} DESC={4},REF_ID={5} REMARKD={6}", 
                                    ACTION,
                                    AUDIT_DATE.ToString(),
                                    USER,
                                    ROLE,
                                    DESCRIPTION,
                                    REFERENCE_ID,
                                    REMARKS);

        }

        public int AUDIT_ID { get; set; }

        public string ACTION { get; set; }

        public DateTime AUDIT_DATE { get; set; }

        public int USER { get; set; }

        public string ROLE { get; set; }

        public string DESCRIPTION { get; set; }

        public string REFERENCE_ID { get; set; }

        public string REMARKS { get; set; }

    }
}
