using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthFramework
{
    internal interface IAdminContextProvider : IContextProvider
    {
        AdminContext CreateAdminContext();
    }
}
