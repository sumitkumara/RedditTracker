using RedditTracker.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditTracker.Services
{
    public class SqlService : ISqlService
    {
        public string ConnectionString { get; set; }

        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection(ConnectionString);
        }
        public async Task ExecuteScalarAsync(string command)
        {
            // log commands
            using var connection = GetSqlConnection();
            await connection.OpenAsync();
            //await connection.ExecuteScalarAsync(command);
        }

        //public async Task ExecuteAsync(string command, DynamicParameters parameters = null)
        //{
        //    using var connection = GetSqlConnection();
        //    await connection.OpenAsync();
        //    await connection.ExecuteAsync(command, parameters, commandType: CommandType.StoredProcedure);
        //}

    }
}
