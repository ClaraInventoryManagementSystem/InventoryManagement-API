using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Models;
using InventoryManagement.DataAccess.Common;
using Serilog;


namespace InventoryManagement.cache
{
    public class UIColumnCache : CacheBase
    {
        public UIColumnCache(IConfiguration config) : base(config)
        {

            Build(true); 

        }
        private Dictionary<int, List<TableColumnData>> _columnsByScreen = null;
        public override bool Build(bool bForece = false)
      {
            try
            {
                
                if(_columnsByScreen == null || bForece)
                {
                    RepositoryCreator objReposiroty = new RepositoryCreator(_config);
                    IEnumerable<TableColumnData> list=   objReposiroty.RoleUIColumnRepository.GetAllColumns();
                    if( list != null)
                    {
                        _lock.EnterWriteLock();
                        if (_columnsByScreen != null)
                            _columnsByScreen.Clear();
                        else
                            _columnsByScreen = new Dictionary<int, List<TableColumnData>>();

                        foreach (TableColumnData data in list)
                        {
                            if(_columnsByScreen.ContainsKey(data.SCREEN_ID))
                            {
                                List<TableColumnData> _ScreenList = _columnsByScreen[data.SCREEN_ID];
                                _ScreenList.Add(data);
                                _columnsByScreen[data.SCREEN_ID] = _ScreenList;
                            }
                            else
                            {
                                List<TableColumnData> _screenList = new List<TableColumnData>();
                                _screenList.Add(data);
                                _columnsByScreen[data.SCREEN_ID] = _screenList;
                            }
                        }

                        return true;

                    }
                }

            }
            catch (Exception eError)
            {
                Log.Logger.Information("BuildUIColumnCache exception" + eError.Message);
                throw eError;
            }
            finally 
            {
                if(_lock.IsWriteLockHeld)
                _lock.ExitWriteLock();
                Log.Logger.Information("UIScreen cache build successfully");
            }
            return false;
      
      }

        public string GetDbColumnName(string DisplayName)
        {
            try
            {
                _lock.EnterReadLock();
                foreach(List<TableColumnData> dcd in _columnsByScreen.Values)
                {
                    foreach( TableColumnData tc in dcd)
                    {
                        if(tc.DISPLAY_NAME == DisplayName)
                        {
                            return tc.FIELD_NAME;
                        }
                    }
                }
            }
            catch(Exception eError)
            {
                throw eError;
            }
            finally
            {
                _lock.ExitReadLock();
            }
            return DisplayName;
        }



        private static UIColumnCache _Instance = null;

        public static void InitCache(IConfiguration conifg)
        {
            if (_Instance == null)
            {
                _Instance = new UIColumnCache(conifg);
            }
        }

        public static UIColumnCache Instance
        {
            get
            {
                return _Instance;
            }
        }

        public bool HasAccess( int RoleAccess, int RoleId)
        {
            return  ((RoleId & (1 << RoleAccess)) != 0) ;
        }

        public  IEnumerable<TableColumnData> GetEditableColumns(int ScreenID, int RoleId)
        {
            IList<TableColumnData> readOnlyColumns = new List<TableColumnData>();

            IEnumerable<TableColumnData> list = GetScreenColumnsbyRole(ScreenID, RoleId);
            if(list != null)
            {
                
                foreach (TableColumnData t in list)
                {
                    if (t.CAN_EDIT == 1)
                        readOnlyColumns.Add(t);
                }
                
            }
            return readOnlyColumns;
        }

       public  IEnumerable<TableColumnData> GetScreenColumnsbyRole(int ScreenID, int RoleId)
        {
            try
            {
                _lock.EnterReadLock();
                if( _columnsByScreen.ContainsKey(ScreenID) )
                {
                    List<TableColumnData> columnList = _columnsByScreen[ScreenID];
                    if( columnList != null)
                    {
                        IList<TableColumnData> columnAccessList = new List<TableColumnData>();
                        foreach (TableColumnData column in columnList)
                        {
                            if ((column.ROLE_ACCESS & (1 << RoleId)) != 0)
                            {
                                columnAccessList.Add(new TableColumnData(column));

                            }
                        }
                        return columnAccessList;
                    }
                }

            }
            catch( Exception eError)
            {
                throw eError;
            }
            finally
            {
                _lock.ExitReadLock();

            }
            return null;

        }
    }
}
