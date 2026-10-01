using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.Repositories
{
    public class DatabaseRepository
    {
        private readonly string connectionString;

        public DatabaseRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<string> GetTableNames()
        {
            List<string> tableNames = new List<string>();

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string sql = @"
                SELECT TABLE_NAME
                FROM INFORMATION_SCHEMA.TABLES
                WHERE TABLE_TYPE = 'BASE TABLE'
                ORDER BY TABLE_NAME";

            using SqlCommand command = new SqlCommand(sql, connection);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string tableName = reader["TABLE_NAME"].ToString() ?? "";

                tableNames.Add(tableName);
            }

            return tableNames;
        }

        public List<Dictionary<string, object>> GetTableData(string tableName)
        {
            List<Dictionary<string, object>> rows =
                new List<Dictionary<string, object>>();

            List<string> allowedTables = GetTableNames();

            if (!allowedTables.Contains(tableName))
            {
                throw new ArgumentException("Invalid table name.");
            }

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string sql = $"SELECT * FROM [{tableName}]";

            using SqlCommand command = new SqlCommand(sql, connection);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Dictionary<string, object> row =
                    new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    object value = reader.GetValue(i);

                    row.Add(columnName, value);
                }

                rows.Add(row);
            }

            return rows;
        }
    }
}
