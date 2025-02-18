using Credit.Api.Db.Models;
using Dapper.Base;

namespace Credit.Api.Db.Repositories
{
    public class InvoiceRepository : BaseReposiotry, IInvoiceRepository
    {
        public InvoiceRepository(IDapperBase dapper) : base(dapper) { }

        public async Task<IEnumerable<Invoice>> GetByCreditIdsAsync(IEnumerable<uint> creditIds)
        {
            var sql = @"SELECT *
                FROM Invoice
                WHERE CreditId IN @creditIds";

            return await _dapper.QueryAsync<Invoice>(sql, param: new { creditIds });
        }
    }
}