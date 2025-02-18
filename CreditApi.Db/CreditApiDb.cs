using Dapper.Base;

namespace Credit.Api.Db
{
    public class CreditApiDb : DapperBase
    {
        public CreditApiDb(string connectionString) : base(connectionString)
        {
        }
    }
}