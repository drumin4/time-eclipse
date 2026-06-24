using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTracker
{
    public class GlobalConfig
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter;
        DataTable table;
        string connectionString;

        public GlobalConfig()
        {
            connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=TimeEclipseDb;Integrated Security=True;TrustServerCertificate=True";
        }

        public DataTable GetData(string Query)
        {
            table = new DataTable();
            using(connection = new SqlConnection(connectionString))
            {
                using (adapter = new SqlDataAdapter(Query, connection))
                {
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        public void SetData(string Query)
        {
            using(connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (command = new SqlCommand(Query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
