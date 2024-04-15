using InventoryManagement.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.DataAccess.Elab.Interface
{
    public interface IContactRepository
    {
        IEnumerable<ContactVM> GetAll();
        IEnumerable<ContactVM> GetById(int ID);
        IEnumerable<ContactVM> GetContactByCustomerId(int ID);
    }
}
