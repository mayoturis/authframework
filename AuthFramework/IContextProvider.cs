using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace AuthFramework
{
    public interface IContextProvider
    {
        DbContext Context { get; }
    }
}
