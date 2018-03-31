using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuthFramework
{
    public interface IUser 
    {
        string Username { get; set; }
        string Password { get; set; }
    }
}
