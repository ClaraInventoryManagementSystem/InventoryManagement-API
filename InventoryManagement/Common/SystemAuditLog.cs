using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Models;
using InventoryManagement.cache;
using InventoryManagement.DataAccess;
using InventoryManagement.DataAccess.Common;
using Serilog;

namespace InventoryManagement.Common
{
    public static class SystemAuditLog
    {

        private static void _ProcessAuditLog(SystemAuditRecord r)
        {
            try
            {
                IConfiguration config = ApplicationCache.Config;
                if( config != null)
                {
                    RepositoryCreator c = new RepositoryCreator(config);
                    if(! c.AuditDataRepository.AddNewSystemAuditLog(r) )
                    {
                        Log.Error("Faild to add Audit Log data=" + r.ToString());

                    }

                }

            }
            catch( Exception eError)
            {
                Log.Logger.Error("Failed to process Audit log error=" + eError.Message);
                Log.Logger.Error("Audit Data= "+ r.ToString());
            }

        }

        public static void ProcessSystemAuditLog(AuditLogAction Action, 
                                                int User,
                                                int RoleId, 
                                                string Description = null,
                                                string Ref = null,
                                                string Remarks = null)
        {
            SystemAuditRecord r = new SystemAuditRecord
            {
                ACTION = Action.ToString(),
                AUDIT_DATE = DateTime.Now,
                USER = User,
                ROLE = RoleId.ToString(),
                DESCRIPTION = Description,
                REFERENCE_ID = Ref,
                REMARKS = Remarks
            };

            var task = new Task(() =>
            {
                _ProcessAuditLog(r);

            });
            task.ExecuteinBackGround();




        }
    }
}
