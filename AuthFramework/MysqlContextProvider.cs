using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthFramework
{
    public class MysqlContextProvider : IContextProvider
    {
        public TContext CreateContext<TContext>() where TContext : DbContext
        {
            return this.CreateContext<TContext>(Configurator.AuthenticatedUser);
        }

        public TContext CreateContext<TContext>(IUser user) where TContext : DbContext
        {
            return (TContext) Activator.CreateInstance(typeof(TContext),
                BuildConnectionString(
                    Configurator.AdminCredentials.Server, 
                    Configurator.AdminCredentials.Database,
                    user.Username,
                    user.Password
                    ));
        }

        protected string BuildConnectionString(string server, string database, string username, string password)
        {
            return $"Server={server};Database={database};Uid={username};Pwd={password}";
        }
        
    }
}
