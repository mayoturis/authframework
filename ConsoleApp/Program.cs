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
            using (var context = new AdminContext())
            {
                context.Database.ExecuteSqlCommand($"DROP USER @name @'localhost'", new MySqlParameter("@name", "edfg"));
                //foreach (var r in result.ToList())
                    //Console.Out.WriteLine(r);

            }
        }
    }
}
