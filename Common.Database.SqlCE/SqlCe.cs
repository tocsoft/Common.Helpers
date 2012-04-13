using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

namespace Common
{
    public class SqlCeDatabase: Database
    {
        public SqlCeDatabase(string connectionString, params Type[] mappingfiles)
             : base(connectionString,mappingfiles)
         {
         }

         public SqlCeDatabase(string connectionString, params Assembly[] mappingfiles)
            : base(connectionString, mappingfiles)
         {
         }

         public SqlCeDatabase(string connectionString, Action<MappingConfiguration> mappings)
             : base(connectionString, mappings)
         {
         }

         protected override FluentNHibernate.Cfg.Db.IPersistenceConfigurer GetDbConfig(string connectionString)
         {
             var dataSource = connectionString.Split(';')
                 .Select(x => x.Split(new char[] { '=' }, 2))
                 .Where(x => x[0].Trim() == "Data Source")
                 .Select(x => x[1])
                 .FirstOrDefault();

             if (dataSource.EndsWith(".sdf"))
             {
                 var dir = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
                 var dbFile = dataSource.Replace("|DataDirectory|", dir);
                 //we want a sqlce db in here

                if (!File.Exists(dbFile))//if file doesn't exist then create it
                {
                     var engine = new System.Data.SqlServerCe.SqlCeEngine(connectionString);
                     engine.CreateDatabase();
                }
                return MsSqlCeConfiguration.Standard.ConnectionString(connectionString);
             }

             else
             {
                 return base.GetDbConfig(connectionString);
             }
         }
    }
}