using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using System.Threading;

namespace InventoryManagement.Common
{
    public static class Extensions
    {

        private  static void  ThreadProc( Task t)
        {
            try
            {
                t.RunSynchronously();

            }
            catch( Exception eError)
            {
                Log.Logger.Error(eError.Message);
            }

        }

        
        public static void ExecuteinBackGround(this Task t)
        {
            Thread thread = new Thread(() => ThreadProc(t));
            thread.Start();


        }
    }
}
