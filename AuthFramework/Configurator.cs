using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuthFramework
{
    public static class Configurator
    {
        private static ConnectionCredentials _adminCredentials;
        internal static ConnectionCredentials AdminCredentials
        {
            get
            {
                if (Configurator._adminCredentials == null)
                    throw new InvalidOperationException("Connection credentials where not set");

                return Configurator._adminCredentials;
            }
            private set { Configurator._adminCredentials = value; }
        }

        private static IUser _authenticatedUser;

        internal static IUser AuthenticatedUser
        {
            get
            {
                if (Configurator._authenticatedUser == null) 
                    throw new InvalidOperationException("No user was authenticated");

                return Configurator._authenticatedUser;
            }
            set { Configurator._authenticatedUser = value; }
        }

        public static void ConfigureAdminAccount(string server, string database, string adminUsername, string adminPassword)
        {
            Configurator.AdminCredentials = new ConnectionCredentials()
            {
                Server = server,
                Database = database,
                Username = adminUsername,
                Password = adminPassword
            };
        }
    }
}
