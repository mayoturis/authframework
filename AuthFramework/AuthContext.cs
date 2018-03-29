using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthFramework
{
    public class AuthContext : DbContext
    {
        public AuthContext() : base("Server=localhost;Database=skuska;Uid=root;Pwd=")
        {
            Database.SetInitializer<AuthContext>(null);
        }
    }
}
