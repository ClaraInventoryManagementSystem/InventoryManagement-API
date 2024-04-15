using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Models
{
    public class DashBoarInfo
    {
        public string CUSTOMER_SHORT_NAME { get; set; }
        public string CUSTOMER_ID { get; set; }
        public string Amount { get; set; }
       // public string Name { get; set; }
    }

    public class RevenueInfo
    {
        public string CUSTOMER_SHORT_NAME { get; set; }
        public string CUSTOMER_ID { get; set; }
        public string Amount { get; set; }
        //public string Name { get; set; }
    }

    public class AllARTypeDetail
    {
        public string SHORT_NAME { get; set; }      
        public string COUNT { get; set; }        
    }


    public class LabGraphicDetails
    {
        public string DISPLAY_NAME { get; set; }
        public string Count { get; set; }
        public string MonthSeries { get; set; }
    }

    public class TotalSamplesByCustomers
    {
        public string CUSTOMER_NAME { get; set; }
        public string Chemical { get; set; }
        public string Micro { get; set; }
        public string Both { get; set; }
    }

    public class LabLiveTestDetails
    {

        public static void  ComputeTotals(List<LabLiveTestDetails> list)
        {
            int TotalAssignPedning = 0;
            int TotalUndertesting = 0;
            int TotalunderReview = 0;
            int TotalReleased = 0;
            foreach(LabLiveTestDetails l in list)
            {
                TotalAssignPedning += Convert.ToInt32(l.assignpending);
                TotalUndertesting += Convert.ToInt32(l.undertesting);
                TotalunderReview += Convert.ToInt32(l.underreview);
                TotalReleased += Convert.ToInt32(l.released);
            }
            LabLiveTestDetails tCounts = new LabLiveTestDetails();
            tCounts.TEST_STATUS = "Total";
            tCounts.assignpending = TotalAssignPedning.ToString();
            tCounts.undertesting = TotalUndertesting.ToString();
            tCounts.underreview = TotalunderReview.ToString();
            tCounts.released = TotalReleased.ToString();
            list.Add(tCounts);




        }
        public string TEST_STATUS { get; set; }

        public string assignpending { get; set; }
        public string undertesting { get; set; }
        public string underreview { get; set; }
        public string released { get; set; }

       // public string COUNT { get; set; }
    }

}
