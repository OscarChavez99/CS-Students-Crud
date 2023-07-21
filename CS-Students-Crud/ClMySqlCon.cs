using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS_Students_Crud
{
    internal class ClMySqlCon: ClConnection
    {
        private MySqlConnection con;
        private string connectionString;
        public ClMySqlCon()
        {
            connectionString =
                "host=" + host + ";" +
                "database=" + database + ";" +
                "user=" + user + ";" +
                "password=" + password + ";";

            con = new MySqlConnection(connectionString);
        }

        public MySqlConnection GetConnection()
        {
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot connect database, error:\n"+ex.ToString());
            }
            return con;
        }
    }
}
