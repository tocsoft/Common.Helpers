﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System.Reflection;
using System.Web;
using FluentNHibernate.Cfg.Db;

namespace Common
{
    public class Database
    {
        public enum DbTypes { 
            SqlCE,
            SqlServer
        }

         private string ItemKey = "NH_Session_" + Guid.NewGuid().ToString();
         private ISessionFactory sessionFactory;

         public Database(string connectionString, params Type[] mappingfiles)
             : this(connectionString,(Action<MappingConfiguration>)(m =>
             {
                 foreach (var t in mappingfiles)
                     m.FluentMappings.Add(t);
             }))
         {

         }
         public Database(string connectionString, params Assembly[] mappingfiles)
             : this(connectionString, (Action<MappingConfiguration>)(m =>
             {
                 foreach (var a in mappingfiles)
                     m.FluentMappings.AddFromAssembly(a);
             }))
         {

         }

         public Database(string connectionString, Action<MappingConfiguration> mappings)
        {
            DbTypes type = DbTypes.SqlServer;
            if (connectionString.ToLower().Contains(".sdf"))
                type = DbTypes.SqlCE;

            sessionFactory = 
                Fluently.Configure().Database(() =>{
                    if (type == DbTypes.SqlCE)
                        return MsSqlCeConfiguration.Standard.ConnectionString(connectionString);
                    else
                        return MsSqlConfiguration.MsSql2005.ConnectionString(connectionString);
                }
                ).Mappings(mappings)
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true))
                .BuildSessionFactory();
        }
        public ISession Session
        {
            get
            {
                ISession sess = null;
                var ctx = HttpContext.Current;

                sess = ctx.Items[ItemKey] as ISession;
                if (sess == null)
                {
                    sess = sessionFactory.OpenSession();

                    ctx.Items[ItemKey] = sess;

                }

                return sess;
            }
        }

      
        public T UnitOfWork<T>(Func<ISession, T> uow) {
            
            using(ISession sess = sessionFactory.OpenSession()){
                T val = uow(sess);
                sess.Flush();
                return val;
            }
        }

    }
}