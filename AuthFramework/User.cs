﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuthFramework
{
    public class User : IUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
