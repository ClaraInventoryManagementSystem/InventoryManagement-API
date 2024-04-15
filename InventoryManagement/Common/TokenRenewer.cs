using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using InventoryManagement.DataAccess;
using InventoryManagement.DataAccess.Common;
using InventoryManagement.cache;

namespace InventoryManagement.Common
{
    public class TokenRenewer
    {

        public string _token = String.Empty;

        public TokenRenewer(string Token)
        {
            _token = Token;

        }
        public void RenewToken()
        {
            try
            {
                RepositoryCreator r = new RepositoryCreator(ApplicationCache.Config);
                r.TokenRepository.RenewToken(_token);
            }
            catch( Exception eError)
            {
                Log.Logger.Error(eError.Message + "---RenewToken");
            }

        }

        public static void RenewToken(string token)
        {
            var task = new Task(() =>
            {
                TokenRenewer t = new TokenRenewer(token);
                t.RenewToken();
            });
            task.ExecuteinBackGround();

        }
    }
}
