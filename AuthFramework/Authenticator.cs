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

        /// <summary>
        /// Registers new user with INSERT, SELECT and UPDATE right on configured database
        /// </summary>
        /// <param name="user">User to be registered</param>
        public void Register(IUser user)
        {
            this.contextProvider.CreateAdminContext().Database.ExecuteSqlCommand(
                $"CREATE USER @name @'{Configurator.AdminCredentials.Server}' IDENTIFIED BY @password ;" +
                $"GRANT INSERT, SELECT, UPDATE ON {Configurator.AdminCredentials.Database}.* TO @name @'{Configurator.AdminCredentials.Server}' ;", 
                new MySqlParameter("@name", user.Username), 
                new MySqlParameter("@password", user.Password));
        }

        /// <summary>
        /// Determines whether user can be successfully authenticated
        /// </summary>
        /// <param name="user">user to be authenticated</param>
        /// <returns>true if credentials are correct, false otherwise</returns>
        public bool CanAuthenticate(IUser user)
        {
            var result = this.contextProvider.CreateAdminContext().Database.SqlQuery<string>(
                $"SELECT 1 FROM mysql.user WHERE Host = '{Configurator.AdminCredentials.Server}' and User = @name and Password = PASSWORD(@password) ;",
                new MySqlParameter("@name", user.Username), new MySqlParameter("@password", user.Password)).FirstOrDefault();

            return result != null && result == "1";
        }

        /// <summary>
        /// Determines whether user can be successfully authenticated. 
        /// If yes than given user will be used for all default DB authentications.
        /// This can be useful, because in the places where we are creating transactions we don't 
        /// have to have instance of given user
        /// </summary>
        /// <param name="user">user to be authenticated</param>
        /// <returns>true if credentials are correct, false otherwise</returns>
        public bool Authenticate(IUser user)
        {
            if (this.CanAuthenticate(user)) // user was found
            {
                SetUser(user);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets user to be used for DB authentication without first determining if credentials are correct
        /// </summary>
        /// <param name="user">user to be used for DB authentication</param>
        public void SetUser(IUser user)
        {
            Configurator.AuthenticatedUser = user;
        }

        /// <summary>
        /// Deletes user with given username
        /// </summary>
        /// <param name="userName"></param>
        public void Delete(string userName)
        {
            this.contextProvider.CreateAdminContext().Database.ExecuteSqlCommand($"DROP USER @name @'{Configurator.AdminCredentials.Server}'", new MySqlParameter("@name", userName));
        }

    }
}
