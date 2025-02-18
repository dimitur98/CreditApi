namespace Dapper.Base
{
    public interface IDapperBase
    {
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null);

        Task<int> ExecuteAsync(string sql, object param = null);
    }
}