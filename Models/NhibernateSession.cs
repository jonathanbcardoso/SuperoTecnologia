using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace SuperoTarefas.Models
{
    public class NHibernateSession
    {
        public static ISession OpenSession(string hibernatePath)
        {
                var configuration = new Configuration();
                var configurationPath = HttpContext.Current.Server.MapPath(@"~\Models\hibernate.cfg.xml");
                configuration.Configure(configurationPath);
                var tarefasConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Mappings\" + hibernatePath);
                configuration.AddFile(tarefasConfigurationFile);
                ISessionFactory sessionFactory = configuration.BuildSessionFactory();
                return sessionFactory.OpenSession();  
        }
    }
}