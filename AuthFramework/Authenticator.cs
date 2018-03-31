using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace AuthFramework
{
    public class Authenticator
    {
        private readonly IAdminContextProvider contextProvider = new MysqlAdminContextProvider();

        // registers new user
        public void Register(IUser user)
        {
            this.contextProvider.CreateAdminContext().Database.ExecuteSqlCommand(
                $"CREATE USER @name @'{Configurator.AdminCredentials.Server}' IDENTIFIED BY @password ;" +
                $"GRANT INSERT, SELECT, UPDATE ON {Configurator.AdminCredentials.Database}.* TO @name @'{Configurator.AdminCredentials.Server}' ;", 
                new MySqlParameter("@name", user.Username), 
                new MySqlParameter("@password", user.Password));
        }

        // returns true if credentials are correct, otherwise false
        public bool Authenticate(IUser user)
        {
            if (this.CanAuthenticate(user)) // user was found
            {
                Configurator.AuthenticatedUser = user;
                return true;
            }

            return false;
        }

        public bool CanAuthenticate(IUser user)
        {
            var result = this.contextProvider.CreateAdminContext().Database.SqlQuery<string>(
                $"SELECT 1 FROM mysql.user WHERE Host = '{Configurator.AdminCredentials.Server}' and User = @name and Password = PASSWORD(@password) ;",
                new MySqlParameter("@name", user.Username), new MySqlParameter("@password", user.Password)).FirstOrDefault();

            return result != null && result == "1";
        }

        public void SetUser(IUser user)
        {
            Configurator.AuthenticatedUser = user;
        }

        public void Delete(string userName)
        {
            this.contextProvider.CreateAdminContext().Database.ExecuteSqlCommand($"DROP USER @name @'{Configurator.AdminCredentials.Server}'", new MySqlParameter("@name", userName));
        }

    }
}
