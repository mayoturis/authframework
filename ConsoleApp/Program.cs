using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthFramework;
using MySql.Data.MySqlClient;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AuthContext())
            {
                context.Database.ExecuteSqlCommand($"CREATE USER @name @'localhost' IDENTIFIED BY @password ;", new MySqlParameter("@name", "majkdo"), new MySqlParameter("@password", "heslo"));
            }
        }
    }
}
