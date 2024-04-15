using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.DataAccess.Common.Interface
{

        public interface IRepository<TItem> where TItem : class, new()
        {
            int GetCount(params object[] values);
            IEnumerable<TItem> GetByQuery(params object[] values);

            TItem GetById(int id);

            int Create(TItem item);

            int Update(TItem item);

            int Delete(TItem item);
        }
        public interface ITransaction
        {
            DbTransaction Transaction { get; }
            ////MySqlTransaction
            void BeginTransaction();
            void Commit();
            void RollBack();
        }

    
}
