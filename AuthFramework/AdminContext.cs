using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthFramework
{
    internal class AdminContext : DbContext
    {
        public AdminContext(string connectionString) : base(connectionString)
        {
            Database.SetInitializer<AdminContext>(null);
        }
    }
}
