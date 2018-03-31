using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthFramework
{
    public class AdminContext : DbContext
    {
        // "Server=localhost;Database=skuska;Uid=root;Pwd="
        public AdminContext(string connectionString) : base(connectionString)
        {
            Database.SetInitializer<AdminContext>(null);
        }
    }
}
