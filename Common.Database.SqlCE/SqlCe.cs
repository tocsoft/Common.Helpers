using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentNHibernate.Cfg;

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
         { }
    }
}