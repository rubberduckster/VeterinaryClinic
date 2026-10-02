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

        public Dictionary<string, string> GetColumnNames(string tableName)
        {
            List<string> allowedTables = GetTableNames();

            if (!allowedTables.Contains(tableName))
            {
                throw new ArgumentException("Invalid table name.");
            }

            Dictionary<string, string> columnNames =
                new Dictionary<string, string>();

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string sql = @"
                SELECT COLUMN_NAME, DATA_TYPE
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_NAME = @TableName
                AND COLUMN_NAME != 'Id'
                ORDER BY ORDINAL_POSITION";

            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@TableName", tableName);

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string columnName = reader["COLUMN_NAME"].ToString() ?? "";
                string dataType = reader["DATA_TYPE"].ToString() ?? "";

                columnNames.Add(columnName, dataType);
            }

            return columnNames;
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

        public void InsertRow(string tableName, Dictionary<string, object> values)
        {
            List<string> allowedTables = GetTableNames();

            if (!allowedTables.Contains(tableName))
            {
                throw new ArgumentException("Invalid table name.");
            }

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            List<string> columnNames = new List<string>();
            List<string> parameterNames = new List<string>();

            foreach (string columnName in values.Keys)
            {
                columnNames.Add($"[{columnName}]");
                parameterNames.Add($"@{columnName}");
            }

            string sql = $@"
                INSERT INTO [{tableName}]
                ({string.Join(", ", columnNames)})
                VALUES ({string.Join(", ", parameterNames)})";

            using SqlCommand command = new SqlCommand(sql, connection);

            foreach (KeyValuePair<string, object> value in values)
            {
                command.Parameters.AddWithValue(
                    $"@{value.Key}",
                    value.Value ?? DBNull.Value);
            }

            command.ExecuteNonQuery();
        }

        public void UpdateRow(string tableName, int id, Dictionary<string, object> values)
        {
            List<string> allowedTables = GetTableNames();

            if (!allowedTables.Contains(tableName))
            {
                throw new ArgumentException("Invalid table name.");
            }

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            List<string> updates = new List<string>();

            foreach (string columnName in values.Keys)
            {
                if (columnName != "Id")
                {
                    updates.Add($"[{columnName}] = @{columnName}");
                }
            }

            string sql = $@"
                UPDATE [{tableName}]
                SET {string.Join(", ", updates)}
                WHERE Id = @Id";

            using SqlCommand command = new SqlCommand(sql, connection);

            foreach (KeyValuePair<string, object> value in values)
            {
                if (value.Key != "Id")
                {
                    command.Parameters.AddWithValue(
                        $"@{value.Key}",
                        value.Value ?? DBNull.Value);
                }
            }

            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery();
        }

        public void DeleteRow(string tableName, int id)
        {
            List<string> allowedTables = GetTableNames();

            if (!allowedTables.Contains(tableName))
            {
                throw new ArgumentException("Invalid table name.");
            }

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string sql = $@"
                DELETE FROM [{tableName}]
                WHERE Id = @Id";

            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery();
        }

        public async Task<Dictionary<string, int>> GetTableRowCounts()
        {
            List<string> tableNames = GetTableNames();

            Dictionary<string, int> tableRowCounts =
                new Dictionary<string, int>();

            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            foreach (string tableName in tableNames)
            {
                string sql = $"SELECT COUNT(*) FROM [{tableName}]";

                using SqlCommand command = new SqlCommand(sql, connection);

                object? result = await command.ExecuteScalarAsync();

                int rowCount = Convert.ToInt32(result);

                tableRowCounts.Add(tableName, rowCount);
            }

            return tableRowCounts;
        }
    }
}
