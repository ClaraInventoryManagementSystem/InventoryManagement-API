using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Common
{
    public class SecurityException : Exception
    {

        public SecurityException(string Message): base(Message)
        {

        }
    }
}
