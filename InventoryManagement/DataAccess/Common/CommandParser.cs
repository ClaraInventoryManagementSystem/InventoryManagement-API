using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.DataAccess.Common;
using System.IO;
using System.Data;
using System.Xml.Linq;
//using InventoryManagement.Infrastructure;

using System.Data.Common;

namespace InventoryManagement.DataAccess.BaseDataAccessRepository
{
    public class CommandParser : ICommandParser
    {

        private static ICommandParser singletonObject = new CommandParser();
        private XDocument xdoc = null;

        private CommandParser()
        {
            //xdoc = XDocument.Load(Path.Combine(Utility.CurrentDirectory, Utility.CurrentAppSettings.DataAccessConfigPath));

            //xdoc = XDocument.Load(Path.Combine(Utility.CurrentDirectory, "Config\\DataAccessConfiguration.xml"));
        }

        public static ICommandParser Current
        {
            get
            {
                return singletonObject;
            }
        }

        public DbCommand GetCommand<TItem>(DbConnection connection, Method method, params object[] values)
        {

            var ns = typeof(TItem).ToString().Split('.');
            var type = ns[ns.Length - 1];

            return GetCommand(type, connection, method, values);
            //throw new NotImplementedException();
        }

        public DbCommand GetCommand(string key, DbConnection connection, Method method, params object[] values)
        {
            var objectElement = xdoc.Descendants("Object").Where(o => o.Attribute("Key").Value.Equals(key));
            var commandElement = objectElement.Descendants("DbCommand").Where(d => d.Attribute("Method").Value.Equals(method.ToString()));
            var found = commandElement.Count() == 1;

            if (!found)
            {
                for (int index = 0; index < commandElement.Count(); index++)
                {
                    int count = 0;
                    var cmdElement = commandElement.ElementAt(index);
                    foreach (var elem in cmdElement.Descendants("Parameter"))
                    {
                        var att = elem.Attribute("Value");

                        if (att != null && att.Value.StartsWith("%"))
                            count++;
                    }

                    if (count == values.Length)
                    {
                        commandElement = cmdElement.DescendantsAndSelf();
                        found = true;
                        break;
                    }
                }
            }

            if (!found)
                throw new InvalidOperationException("DbCommand could not be resolved");

            var dbCommand = connection.CreateCommand();

            var commandType = commandElement.Attributes("CommandType").FirstOrDefault();
            if (commandType != null)
            {
                dbCommand.CommandType = commandType.Value.Equals("StoredProcedure") ?
                CommandType.StoredProcedure : CommandType.Text;
            }

            dbCommand.CommandText = commandElement.Attributes("CommandText").First().Value;

            if (dbCommand.CommandType == CommandType.StoredProcedure)
            {
                foreach (var parameterNode in commandElement.Descendants("Parameter"))
                {
                    var parameter = dbCommand.CreateParameter();
                    var paramName = parameterNode.Attribute("Name");
                    if (paramName != null && !string.IsNullOrWhiteSpace(paramName.Value))
                    {
                        parameter.ParameterName = "@" + paramName.Value;

                        var val = parameterNode.Attribute("Value");
                        if (val != null)
                        {
                            if (val.Value.StartsWith("%"))
                            {
                                var pos = int.Parse(val.Value.TrimStart('%'));
                                parameter.Value = pos < values.Length ? values[pos] : null;
                            }
                            else
                            {
                                parameter.Value = val.Value;
                            }
                        }

                        var direction = parameterNode.Attribute("Direction");
                        if (direction != null)
                        {
                            parameter.Direction = (ParameterDirection)Enum.Parse(typeof(ParameterDirection), direction.Value);
                        }

                        var size = parameterNode.Attribute("Size");
                        if (size != null)
                        {
                            parameter.Size = int.Parse(size.Value);
                        }

                        var dbType = parameterNode.Attribute("DbType");
                        if (dbType != null)
                        {
                            parameter.DbType = (DbType)Enum.Parse(typeof(DbType), dbType.Value);
                        }

                        var allowNull = false;
                        if (parameterNode.Attribute("AllowNull") != null)
                        {
                            allowNull = parameterNode.Attribute("AllowNull").Value.Equals("true");
                            //if (parameter.Value.ToString().Equals("0"))
                            //{
                            //    parameter.Value = null; 
                            //}
                        }

                        if (!(parameter.Value == null && parameter.Direction == ParameterDirection.Input) || allowNull)
                        {
                            dbCommand.Parameters.Add(parameter);
                        }
                    }
                }
            }
            else
            {
                dbCommand.CommandText = string.Format(dbCommand.CommandText, values);
            }

            return dbCommand;
        }

        public DbCommand GetCommand(string query, DbConnection connection)
        {
            var dbCommand = connection.CreateCommand();

            dbCommand.CommandType = CommandType.Text;

            dbCommand.CommandText = query;

            return dbCommand;
        }
    }


}
