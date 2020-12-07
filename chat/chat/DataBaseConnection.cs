using MySql.Data.MySqlClient;
using MySql.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace chat
{
    class DataBaseConnection : IDisposable
    {
        string _connectionString;
        MySqlConnection _mySQLConnection;

        //Server=myServerAddress;
        //Database=myDataBase;
        //Uid=myUsername;
        // Pwd=myPassword;
        public DataBaseConnection(string Server, string DataBase, string UserName, string Password  )
        {
            _connectionString = $"Server={Server};Database={DataBase};Uid={UserName};Pwd={Password};Port=3306;";
            _mySQLConnection = new MySqlConnection(_connectionString);
            _mySQLConnection.Open();
        }

        public bool InsertUser(string UserName, string Password)
        {
            try
            {
                string query = $"INSERT INTO logins (user,password) VALUES ('{UserName}','{Password}');";
                MySqlCommand command = new MySqlCommand(query, _mySQLConnection);
                command.ExecuteNonQuery();
                return true;
            } catch
            {
                return false;
            }
        }
        public int GetUserId(string UserName, string Password)
        {

            try
            {
                string query = $"SELECT id FROM logins WHERE user ='{UserName}' and password='{Password}'";
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, _mySQLConnection);
                DataTable table = new DataTable();
                dataAdapter.Fill(table);
                int userId = Convert.ToInt32(table.Rows[0].ItemArray[0]);
                return userId;
            } catch
            {
                return 0;
            }
           
        }
        public void Dispose()
        {
            _mySQLConnection.Close();
        }


    }
}
