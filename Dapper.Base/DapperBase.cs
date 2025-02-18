using Microsoft.Data.Sqlite;
using System.Data;

namespace Dapper.Base
{
    public abstract class DapperBase : IDapperBase
    {
        private readonly string _connectionString;

        public DapperBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null)
        {
            using var connection = this.GetConnection();
            return await connection.QueryAsync<T>(sql, param: param);
        }

        public async Task<int> ExecuteAsync(string sql, object param = null)
        {
            using var connection = this.GetConnection();
            return await connection.ExecuteAsync(sql, param: param);
        }

        private IDbConnection GetConnection()
        {
            return new SqliteConnection(_connectionString); ;
        }
    }
}