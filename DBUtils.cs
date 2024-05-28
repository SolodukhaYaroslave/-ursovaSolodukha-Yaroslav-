using lab_3;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary
{
    internal class DBUtils
    {
        public static MySqlConnection GetDBConnection(string user)
        {
            string host = "localhost";
            int port = 3306;
            string database = "coursework";
            string username = user;
            string password = "12345678";

            return DBMySQLUtils.GetDBConnection(host, port, database, username, password);
        }
    }
}
