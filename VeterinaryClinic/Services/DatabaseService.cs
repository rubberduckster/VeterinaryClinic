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

        public List<Dictionary<string, object>> GetTableData(string tableName)
        {
            return databaseRepository.GetTableData(tableName);
        }
    }
}
