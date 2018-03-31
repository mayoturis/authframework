using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthFramework
{
    internal class MysqlAdminContextProvider : MysqlContextProvider, IAdminContextProvider
    {
        public AdminContext CreateAdminContext()
        {
            return new AdminContext(BuildConnectionString(
                    Configurator.AdminCredentials.Server,
                    Configurator.AdminCredentials.Database,
                    Configurator.AdminCredentials.Username,
                    Configurator.AdminCredentials.Password
                ));
        }
    }
}
