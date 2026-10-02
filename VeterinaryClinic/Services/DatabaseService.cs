using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Repositories;

namespace VeterinaryClinic.Services
{
    public class DatabaseService
    {
        private readonly DatabaseRepository databaseRepository;

        public DatabaseService(DatabaseRepository databaseRepository)
        {
            this.databaseRepository = databaseRepository;
        }

        public List<string> GetTableNames()
        {
            return databaseRepository.GetTableNames();
        }

        public Dictionary<string, string> GetColumnNames(string tableName)
        {
            return databaseRepository.GetColumnNames(tableName);
        }

        public List<Dictionary<string, object>> GetTableData(string tableName)
        {
            return databaseRepository.GetTableData(tableName);
        }

        public void InsertRow(string tableName, Dictionary<string, object> values)
        {
            databaseRepository.InsertRow(tableName, values);
        }

        public void UpdateRow(string tableName, int id, Dictionary<string, object> values)
        {
            databaseRepository.UpdateRow(tableName, id, values);
        }

        public void DeleteRow(string tableName, int id)
        {
            databaseRepository.DeleteRow(tableName, id);
        }

        public async Task<Dictionary<string, int>> GetTableRowCounts()
        {
            return await databaseRepository.GetTableRowCounts();
        }
    }
}
