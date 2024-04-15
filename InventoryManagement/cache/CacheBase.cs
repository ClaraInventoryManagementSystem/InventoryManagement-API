using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryManagement.cache
{
    public abstract class CacheBase
    {

        protected ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        protected IConfiguration _config = null;

        public CacheBase(IConfiguration config)
        {
            _config = config;
        }

        public abstract bool Build(bool bForece = false);

        

    }
}
