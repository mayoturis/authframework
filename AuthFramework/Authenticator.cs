using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuthFramework
{
    public class Authenticator
    {
        private readonly IContextProvider contextProvider;

        public Authenticator(IContextProvider contextProvider)
        {
            this.contextProvider = contextProvider;
        }

        public void Register(IUser user)
        {
            this.contextProvider.Context.Database.SqlQuery<string>($"CREATE USER '{user.UserName}'@'localhost' IDENTIFIED BY PASSWORD '{user.Password}'");
        }

    }
}
