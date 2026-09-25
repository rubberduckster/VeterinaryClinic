using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.Helpers
{
    internal class FileLogger
    {
        public static void Log(string operation, string table, int recordId, string oldData, string newData)
        {
            string logEntry = $"{DateTime.Now} | {operation} | {table} | {recordId} | {oldData} | {newData}{Environment.NewLine}";

            File.AppendAllText("database.log", logEntry);
        }
    }
}
